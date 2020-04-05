using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
    public GameObject textPrefab;
    public GameObject selectableTextPrefab;
    public GameObject techPrefab;
    public HealthBar healthBar;
    public Text itemUseMessage;
    public Transform techBase;
    public Transform equipmentBase;
    public Transform abilityBase;
    public Transform inventoryBase;
    public Text rollBoundsText;
    public Text keyCount;

    public int lineSeparation = 25;

    private List<Selectable> selectableEquipped;
    private List<Selectable> selectableInventory;

    void Start()
    {
        itemUseMessage.text = "";
        UpdateHealthUI();
        UpdateRollBoundsUI();
        UpdateTechUI();
        UpdateEquipmentUI();
        UpdateAbilityUI();
        UpdateInventoryUI();
        keyCount.text = PlayerStatus.KeyCount.ToString();
    }

    private void UpdateHealthUI()
    {
        // For testing
        PlayerStatus.InitializeIfNecessary();
        healthBar.UpdateHealth(PlayerStatus.Health, PlayerStatus.MaxHealth);
    }

    private void UpdateRollBoundsUI()
    {
        Tuple<int, int> baseRoll = new Tuple<int, int>(
            PlayerStatus.BaseMinRoll, PlayerStatus.BaseMaxRoll);
        foreach (IRollGenerationModifier mod in PlayerStatus.Mods.GetRollGenerationModifiers())
        {
            baseRoll = mod.apply(baseRoll.Item1, baseRoll.Item2);
        }
        rollBoundsText.text = "Roll: [" + baseRoll.Item1 + "-" + baseRoll.Item2 + "]";
    }

    private void UpdateTechUI()
    {
        for (int i = 0; i < PlayerStatus.EnabledTechs.Count; i++)
        {
            Tech tech = PlayerStatus.EnabledTechs[i];
            GameObject newTech = Instantiate(techPrefab, techBase);
            newTech.GetComponent<Text>().text = tech.GetDisplayName();
            Transform backgroundImage = newTech.transform.Find("Background");
            backgroundImage.Find("TechIcon").GetComponent<Image>().sprite = 
                Cache.GetTechIcon(tech.name);
            Tooltip tooltip = newTech.GetComponent<Tooltip>();
            tooltip.SetTooltipText(tech.description);
            newTech.GetComponent<RectTransform>().Translate(i * lineSeparation * Vector3.down);
        }
    }

    private void UpdateEquipmentUI()
    {
        if (selectableEquipped != null)
        {
            foreach (Selectable currentUISelectable in selectableEquipped)
            {
                Destroy(currentUISelectable.gameObject);
            }
        }

        List<UIText> equipmentUIText = new List<UIText>();
        if (PlayerStatus.EquippedWeapon != null)
        {
            equipmentUIText.Add(new UIText
            {
                displayName = "Weapon: " + PlayerStatus.EquippedWeapon.GetDisplayName(),
                tooltipText = PlayerStatus.EquippedWeapon.tooltipText,
                callbackIdentifier = PlayerStatus.EquippedWeapon.name,
                sprite = Cache.GetItemIcon(PlayerStatus.EquippedWeapon.name)
            });
        }
        else
        {
            equipmentUIText.Add(new UIText
            {
                displayName = "Weapon: None"
            });
        }
        foreach (Item trinket in PlayerStatus.EquippedTrinkets)
        {
            equipmentUIText.Add(new UIText
            {
                displayName = trinket.GetDisplayName(),
                tooltipText = trinket.tooltipText,
                callbackIdentifier = trinket.name,
                sprite = Cache.GetItemIcon(trinket.name)
            });
        }

        selectableEquipped = UpdateSelectableListUI(
            equipmentUIText, UseEquipmentItem, 
            DeselectOtherEquipmentItems, equipmentBase);
    }

    private void DeselectOtherInventoryItems(Selectable remainSelected)
    {
        DeselectOthers(selectableInventory, remainSelected);
    }

    private void DeselectOtherEquipmentItems(Selectable remainSelected)
    {
        DeselectOthers(selectableEquipped, remainSelected);
    }

    private void DeselectOthers(List<Selectable> allSelectables, Selectable remainSelected)
    {
        foreach (Selectable selectable in allSelectables)
        {
            if (selectable != remainSelected)
            {
                selectable.Deselect();
            }
        }
    }

    private void UseInventoryItem(string itemName)
    {
        DeselectAll(selectableInventory);
        Item selected = Cache.GetItem(itemName);
        if (!PlayerStatus.Inventory.ContainsKey(selected))
        {
            return;
        }

        string useMessage = null;
        if (PlayerStatus.UseItem(selected, false))
        {
            switch (selected.itemType)
            {
                case ItemType.EQUIPMENT:
                    UpdateEquipmentUI();
                    UpdateInventoryUI();
                    UpdateRollBoundsUI();
                    break;
                case ItemType.USABLE_ANYTIME:
                    UpdateHealthUI();
                    UpdateInventoryUI();
                    break;
            }
        }
        else
        {
            useMessage = selected.failedUseMessage;
            if (selected.itemType == ItemType.USABLE_ONLY_IN_BATTLE)
            {
                useMessage = "This item can only be used in battle";
            }
        }
        if (useMessage != null)
        {
            itemUseMessage.gameObject.SetActive(true);
            itemUseMessage.text = useMessage;
            itemUseMessage.GetComponent<SelfDeactivate>().ResetTimer();
        }
    }

    private void UseEquipmentItem(string itemName)
    {
        DeselectAll(selectableEquipped);
        Item selected = Cache.GetItem(itemName);
        if (selected == null)
        {
            return;
        }

        if (PlayerStatus.EquippedWeapon == selected)
        {
            PlayerStatus.EquippedWeapon = null;
        }
        else
        {
            PlayerStatus.EquippedTrinkets.Remove(selected);
        }
        PlayerStatus.AddItem(selected);

        UpdateInventoryUI();
        UpdateEquipmentUI();
        UpdateRollBoundsUI();
    }

    private void DeselectAll(List<Selectable> selectables)
    {
        foreach (Selectable selectable in selectables)
        {
            selectable.Deselect();
        }
    }

    private void UpdateAbilityUI()
    {
        UpdateListUI(PlayerStatus.GetAbilities().
            Select(a => new UIText 
            { 
                displayName = a.GetDisplayName(), 
                tooltipText = a.description,
                sprite = Cache.GetAbilityIcon(a.name)
            }).ToList(), abilityBase);
    }

    private void UpdateInventoryUI()
    {
        if (selectableInventory != null)
        {
            foreach (Selectable currentUISelectable in selectableInventory)
            {
                Destroy(currentUISelectable.gameObject);
            }
        }

        selectableInventory = UpdateSelectableListUI(PlayerStatus.Inventory.Keys.Select(
            i => new UIText 
            { 
                displayName = PlayerStatus.Inventory[i] + "x " + i.GetDisplayName(), 
                callbackIdentifier = i.name, 
                tooltipText = i.tooltipText,
                sprite = Cache.GetItemIcon(i.name)
            }).ToList(), UseInventoryItem, DeselectOtherInventoryItems, inventoryBase);
    }

    private List<Selectable> UpdateSelectableListUI(List<UIText> uiTexts, 
        Action<string> confirmCallback, Action<Selectable> selectCallback, Transform uiParent)
    {
        List<Selectable> selectables = new List<Selectable>();
        for (int i = 0; i < uiTexts.Count; i++)
        {
            GameObject newText = InstantiateTextPrefab(selectableTextPrefab,
                uiTexts[i], uiParent, i);
            Selectable selectable = newText.GetComponent<Selectable>();
            selectable.SetConfirmCallback(uiTexts[i].callbackIdentifier, confirmCallback);
            selectable.SetSelectedCallback(selectCallback);
            selectables.Add(selectable);
        }
        return selectables;
    }

    private void UpdateListUI(List<UIText> uiTexts, Transform uiParent)
    {
        for (int i = 0; i < uiTexts.Count; i++)
        {
            InstantiateTextPrefab(textPrefab, uiTexts[i], uiParent, i);
        }
    }

    private GameObject InstantiateTextPrefab(GameObject prefab, 
        UIText uiText, Transform uiParent, int index)
    {
        GameObject newText = Instantiate(prefab, uiParent);
        newText.GetComponent<Text>().text = uiText.displayName;
        newText.GetComponent<RectTransform>().Translate(index * lineSeparation * Vector3.down);
        Tooltip tooltip = newText.GetComponent<Tooltip>();
        if (tooltip != null)
        {
            if (uiText.tooltipText == null)
            {
                tooltip.SetEnabled(false);
            }
            else
            {
                tooltip.SetTooltipText(uiText.tooltipText);
            }
        }
        Image iconImage = newText.transform.Find("Icon").GetComponent<Image>();
        iconImage.enabled = uiText.sprite != null;
        iconImage.sprite = uiText.sprite;
        return newText;
    }
}

class UIText
{
    public string displayName;
    public string tooltipText;
    public string callbackIdentifier;
    public Sprite sprite;
}

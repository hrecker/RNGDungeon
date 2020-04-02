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

    public int lineSeparation = 25;

    private List<Selectable> selectableEquipped;
    private List<Selectable> selectableInventory;

    void Start()
    {
        itemUseMessage.text = "";
        UpdateHealthUI();
        UpdateTechUI();
        UpdateEquipmentUI();
        UpdateAbilityUI();
        UpdateInventoryUI();
    }

    private void UpdateHealthUI()
    {
        // For testing
        PlayerStatus.InitializeIfNecessary();
        healthBar.UpdateHealth(PlayerStatus.Health, PlayerStatus.MaxHealth);
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
            newTech.transform.Find("Description").GetComponent<Text>().text = tech.description;
            newTech.GetComponent<RectTransform>().Translate(2.5f * i * lineSeparation * Vector3.down);
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

        List<string> equipmentDisplayNames = new List<string>();
        List<string> equipmentNames = new List<string>();
        if (PlayerStatus.EquippedWeapon != null)
        {
            equipmentDisplayNames.Add("Weapon: " + PlayerStatus.EquippedWeapon.GetDisplayName());
            equipmentNames.Add(PlayerStatus.EquippedWeapon.name);
        }
        else
        {
            equipmentDisplayNames.Add("Weapon: None");
            equipmentNames.Add(null);
        }
        foreach (Item trinket in PlayerStatus.EquippedTrinkets)
        {
            equipmentDisplayNames.Add(trinket.GetDisplayName());
            equipmentNames.Add(trinket.name);
        }

        selectableEquipped = UpdateSelectableListUI(
            equipmentDisplayNames, equipmentNames, 
            UseEquipmentItem, DeselectOtherEquipmentItems, equipmentBase);
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
        UpdateListUI(PlayerStatus.GetAbilities().Select(a => a.GetDisplayName()).ToList(), abilityBase);
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
            i => PlayerStatus.Inventory[i] + "x " + i.GetDisplayName()).ToList(), PlayerStatus.Inventory.Keys.Select(
            i =>  i.name).ToList(), UseInventoryItem, DeselectOtherInventoryItems, inventoryBase);
    }

    private List<Selectable> UpdateSelectableListUI(List<string> displayValues, List<string> callbackParams, 
        Action<string> confirmCallback, Action<Selectable> selectCallback, Transform uiParent)
    {
        List<Selectable> selectables = new List<Selectable>();
        for (int i = 0; i < displayValues.Count; i++)
        {
            GameObject newText = InstantiateTextPrefab(selectableTextPrefab, 
                displayValues[i], uiParent, i);
            Selectable selectable = newText.GetComponent<Selectable>();
            selectable.SetConfirmCallback(callbackParams[i], confirmCallback);
            selectable.SetSelectedCallback(selectCallback);
            selectables.Add(selectable);
        }
        return selectables;
    }

    private void UpdateListUI(List<string> displayValues, Transform uiParent)
    {
        for (int i = 0; i < displayValues.Count; i++)
        {
            InstantiateTextPrefab(textPrefab, displayValues[i], uiParent, i);
        }
    }

    private GameObject InstantiateTextPrefab(GameObject prefab, 
        string displayValue, Transform uiParent, int index)
    {
        GameObject newText = Instantiate(prefab, uiParent);
        newText.GetComponent<Text>().text = displayValue;
        newText.GetComponent<RectTransform>().Translate(index * lineSeparation * Vector3.down);
        return newText;
    }
}

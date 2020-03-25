using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
    public GameObject textPrefab;
    public GameObject selectableTextPrefab;
    public RectTransform redHealthBar;
    public Text healthText;
    public Transform statusBase;
    public Transform equipmentBase;
    public Transform abilityBase;
    public Transform inventoryBase;

    public int lineSeparation = 25;

    private List<Selectable> selectableEquipped;
    private List<Selectable> selectableInventory;

    void Start()
    {
        UpdateHealthUI();
        UpdateStatusUI();
        UpdateEquipmentUI();
        UpdateAbilityUI();
        UpdateInventoryUI();
    }

    private void UpdateHealthUI()
    {
        // For testing
        PlayerStatus.InitializeIfNecessary();

        healthText.text = PlayerStatus.Health + "/" + PlayerStatus.MaxHealth;
        float redHealthPercentage = 100 *
            (PlayerStatus.MaxHealth - PlayerStatus.Health) / PlayerStatus.MaxHealth;
        redHealthBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, redHealthPercentage);
    }

    private void UpdateStatusUI()
    {
        //TODO get statuses from player status once they are added there
        List<string> statuses = new List<string>() { "EXAMPLE", "++MaxRoll" };
        UpdateListUI(statuses, statusBase);
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
            equipmentDisplayNames.Add("Weapon: " + PlayerStatus.EquippedWeapon.name);
            equipmentNames.Add(PlayerStatus.EquippedWeapon.name);
        }
        else
        {
            equipmentDisplayNames.Add("Weapon: None");
            equipmentNames.Add(null);
        }
        if (PlayerStatus.EquippedArmor != null)
        {
            equipmentDisplayNames.Add("Armor: " + PlayerStatus.EquippedArmor.name);
            equipmentNames.Add(PlayerStatus.EquippedArmor.name);
        }
        else
        {
            equipmentDisplayNames.Add("Armor: None");
            equipmentNames.Add(null);
        }
        foreach (Item trinket in PlayerStatus.EquippedTrinkets)
        {
            equipmentDisplayNames.Add(trinket.name);
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

        switch (selected.itemType)
        {
            case ItemType.EQUIPMENT:
                //Equip
                switch (selected.equipSlot)
                {
                    case EquipSlot.ARMOR:
                        Item currentArmor = PlayerStatus.EquippedArmor;
                        if (currentArmor != null)
                        {
                            PlayerStatus.AddItem(currentArmor);
                        }
                        PlayerStatus.EquippedArmor = selected;
                        PlayerStatus.UseItem(selected);
                        break;
                    case EquipSlot.WEAPON:
                        Item currentWeapon = PlayerStatus.EquippedWeapon;
                        if (currentWeapon != null)
                        {
                            PlayerStatus.AddItem(currentWeapon);
                        }
                        PlayerStatus.EquippedWeapon = selected;
                        PlayerStatus.UseItem(selected);
                        break;
                    case EquipSlot.TRINKET:
                        PlayerStatus.EquippedTrinkets.Add(selected);
                        PlayerStatus.UseItem(selected);
                        break;
                }
                UpdateEquipmentUI();
                UpdateInventoryUI();
                break;
            case ItemType.USABLE_ANYTIME:
                //TODO Use
                UpdateInventoryUI();
                break;
            case ItemType.USABLE_ONLY_IN_BATTLE:
                //TODO some kind of message indicating the item can't be used right now
                break;
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

        if (PlayerStatus.EquippedArmor == selected)
        {
            PlayerStatus.EquippedArmor = null;
        }
        else if (PlayerStatus.EquippedWeapon == selected)
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
        UpdateListUI(PlayerStatus.Abilities.Select(a => a.name).ToList(), abilityBase);
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
            i => PlayerStatus.Inventory[i] + "x " + i.name).ToList(), PlayerStatus.Inventory.Keys.Select(
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

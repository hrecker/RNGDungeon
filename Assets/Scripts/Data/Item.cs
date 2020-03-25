using System;

[Serializable]
public class Item
{
    public string name;
    public string tooltipText;
    public ItemType itemType;
    public EquipSlot equipSlot;
    public ItemEffect itemEffect;
}

[Serializable]
public enum EquipSlot
{
    NONE = 0,
    WEAPON = 1,
    ARMOR = 2,
    TRINKET = 3 // Any number of trinkets can be equipped
}

[Serializable]
public enum ItemType
{
    USABLE_ANYTIME = 0,
    USABLE_ONLY_IN_BATTLE = 1,
    EQUIPMENT = 2
}

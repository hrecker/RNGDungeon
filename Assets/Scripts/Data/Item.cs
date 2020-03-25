using System;

[Serializable]
public class Item
{
    public string name;
    public bool inBattleItem; // Whether this item is usable in battle or is an equippable item
    public string tooltipText;
    public ItemEffect itemEffect;
}

using System;

[Serializable]
public class Item
{
    public string name;
    public string displayName;
    public string tooltipText;
    public int numRollsInEffect; // For in-battle items
    public int playerHealthChange; // For healing items
    public string playerStatusMessage;
    public string enemyStatusMessage;
    public string failedUseMessage;
    public ItemType itemType;
    public Rarity rarity;
    public EquipSlot equipSlot;
    public ModType modType;
    public ModEffect modEffect;

    // For items that represent modifiers
    public Modifier CreateItemModifier()
    {
        Modifier result = null;
        switch (modType)
        {
            case ModType.BLOCK:
                result = new BlockingModifier();
                break;
            case ModType.RECOIL:
                result = new RecoilModifer();
                break;
            case ModType.WEAPON:
                result = new RollBuffModifier(
                    modEffect.playerMinRollChange, modEffect.playerMaxRollChange);
                break;
        }
        if (result != null)
        {
            //TODO allow effects that just last until the battle ends?
            result.isRollBounded = numRollsInEffect > 0;
            result.numRollsRemaining = numRollsInEffect;
            result.triggerChance = modEffect.baseModTriggerChance;
        }
        return result;
    }

    // For items like health potions that represent single use effects
    public bool ApplyEffect()
    {
        //TODO implementation for things besides health potions
        if (PlayerStatus.Health >= PlayerStatus.MaxHealth)
        {
            return false;
        }
        PlayerStatus.Health += playerHealthChange;
        return true;
    }

    public string GetDisplayName()
    {
        return string.IsNullOrEmpty(displayName) ? name : displayName;
    }
}

[Serializable]
public enum EquipSlot
{
    NONE = 0,
    WEAPON = 1,
    TRINKET = 2 // Any number of trinkets can be equipped
}

[Serializable]
public enum ItemType
{
    USABLE_ANYTIME = 0, //Stuff like health potions - doesn't generally create modifier
    USABLE_ONLY_IN_BATTLE = 1, // Battle items will always be temporary modifiers
    EQUIPMENT = 2
}

[Serializable]
public enum Rarity
{
    COMMON = 0,
    UNCOMMON = 1,
    RARE = 2,
    NEVER = 3 // Items that are never dropped randomly
}

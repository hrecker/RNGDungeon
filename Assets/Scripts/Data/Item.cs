using System;

[Serializable]
public class Item
{
    public string name;
    public string tooltipText;
    public int numRollsInEffect; // For in-battle items
    public int playerHealthChange; // For healing items
    public ItemType itemType;
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
                result = new BlockingRollResultModifier();
                break;
            case ModType.RECOIL:
                result = new RecoilRollResultModifer();
                break;
            case ModType.WEAPON:
                result = new WeaponRollGenerationModifier(
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
    public void ApplyEffect()
    {
        //TODO implementation for things besides health potions
        PlayerStatus.Health += playerHealthChange;
    }
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
    USABLE_ANYTIME = 0, //Stuff like health potions - doesn't generally create modifier
    USABLE_ONLY_IN_BATTLE = 1, // Battle items will always be temporary modifiers
    EQUIPMENT = 2
}

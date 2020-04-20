using System;
using Modifiers;
using Modifiers.Generic;
using Modifiers.Item;

namespace Data
{
    [Serializable]
    public class Item
    {
        public string name;
        public string displayName;
        public string tooltipText;
        public int numRollsInEffect; // For in-battle items
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
                    result.battleEffect = RollBoundedBattleEffect.RECOIL;
                    break;
                case ModType.WEAPON:
                    result = new RollBuffModifier(
                        modEffect.playerMinRollChange, modEffect.playerMaxRollChange);
                    break;
                case ModType.HEALTHCHANGE:
                    result = new HealthChangeModifier(modEffect.playerHealthChange,
                        modEffect.playerMaxHealthChange);
                    break;
                case ModType.PANACEA:
                    result = new PanaceaModifier();
                    break;
            }
            if (result != null)
            {
                result.isRollBounded = numRollsInEffect > 0;
                result.numRollsRemaining = numRollsInEffect;
                result.triggerChance = modEffect.baseModTriggerChance;
                result.priority = modEffect.modPriority;
                result.actor = modEffect.actor;
            }
            return result;
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
}

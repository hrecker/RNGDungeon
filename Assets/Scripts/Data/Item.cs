﻿using System;
using System.Collections.Generic;
using Modifiers;
using Modifiers.Generic;
using Modifiers.Item;
using Modifiers.StatusEffect;

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
        public List<Modifier> CreateItemModifiers()
        {
            List<Modifier> result = new List<Modifier>();
            switch (modType)
            {
                case ModType.BLOCK:
                    result.Add(new BlockingModifier());
                    break;
                case ModType.RECOIL:
                    Modifier mod = new RecoilModifer();
                    mod.battleEffect = RollBoundedBattleEffect.RECOIL;
                    result.Add(mod);
                    break;
                case ModType.WEAPON:
                    result.Add(new RollBuffModifier(
                        modEffect.playerMinRollChange, modEffect.playerMaxRollChange));
                    break;
                case ModType.HEALTHCHANGE:
                    result.Add(new HealthChangeModifier(modEffect.playerHealthChange,
                        modEffect.playerMaxHealthChange));
                    break;
                case ModType.PANACEA:
                    result.Add(new PanaceaModifier());
                    break;
                case ModType.HOLYWATER:
                    result.Add(new LuckBuffModifier(2, null, false));
                    break;
                case ModType.ENERGYDRINK:
                    result.Add(new EnergyDrinkModifier());
                    break;
                case ModType.POISONEDSWORD:
                    result.Add(new RollBuffModifier(modEffect.playerMinRollChange, 
                        modEffect.playerMaxRollChange));
                    result.Add(new InflictStatusOnHitModifier(
                        Battle.StatusEffect.POISON, 3));
                    break;
                case ModType.PUNISHINGSWORD:
                    result.Add(new RollBuffModifier(modEffect.playerMinRollChange,
                        modEffect.playerMaxRollChange));
                    result.Add(new StatusPunishingRollBuffModifier(2, 2));
                    break;
                case ModType.CURSEDSWORD:
                    result.Add(new RollBuffModifier(modEffect.playerMinRollChange,
                        modEffect.playerMaxRollChange));
                    result.Add(new CursedSwordModifier(3));
                    break;
                case ModType.DEMONICSWORD:
                    result.Add(new RollBuffModifier(modEffect.playerMinRollChange,
                        modEffect.playerMaxRollChange));
                    result.Add(new LowHealthBuffModifier(1, 1));
                    break;
                case ModType.SPECIALTYSWORD:
                    result.Add(new RollBuffModifier(modEffect.playerMinRollChange,
                        modEffect.playerMaxRollChange));
                    result.Add(new TechBuffModifier(2, 2));
                    break;
                case ModType.INVIGORATINGPOTION:
                    Modifier buffMod = new RollBuffModifier(modEffect.playerMinRollChange,
                        modEffect.playerMaxRollChange);
                    buffMod.battleEffect = RollBoundedBattleEffect.BUFF;
                    result.Add(buffMod);
                    break;
            }
            if (result.Count > 0)
            {
                foreach (Modifier mod in result)
                {
                    mod.isRollBounded = numRollsInEffect > 0;
                    mod.numRollsRemaining = numRollsInEffect;
                    mod.triggerChance = modEffect.baseModTriggerChance;
                    mod.priority = modEffect.modPriority;
                    mod.actor = modEffect.actor;
                }
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

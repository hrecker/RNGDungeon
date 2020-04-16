using System;
using Modifiers;
using Modifiers.Ability;
using Modifiers.Generic;

namespace Data
{
    [Serializable]
    public class Ability
    {
        public string name;
        public string displayName;
        public string description;
        public ModType modType;
        public ModEffect modEffect;

        public Modifier CreateAbilityModifier()
        {
            Modifier result = null;
            switch (modType)
            {
                case ModType.HIGHROLLER:
                    result = new HighRollerModifier();
                    break;
                case ModType.STALWART:
                    result = new StalwartModifier();
                    break;
                case ModType.VAMPIRISM:
                    result = new VampirismModifier();
                    break;
                case ModType.HEROIC:
                    result = new HeroicModifier(modEffect.playerMinRollChange,
                        modEffect.playerMaxRollChange);
                    break;
                case ModType.LUCKYHORSESHOE:
                    result = new LuckyHorseshoeModifier();
                    break;
                case ModType.HEALTHCHANGE:
                    result = new HealthChangeModifier(modEffect.playerHealthChange,
                        modEffect.playerMaxHealthChange);
                    break;
                case ModType.PROCRASTINATION:
                    result = new ProcrastinationModifier(modEffect.playerMinRollChange, 
                        modEffect.playerMaxRollChange);
                    break;
            }
            if (result != null)
            {
                result.triggerChance = modEffect.baseModTriggerChance;
                result.priority = modEffect.modPriority;
            }
            return result;
        }

        public string GetDisplayName()
        {
            return string.IsNullOrEmpty(displayName) ? name : displayName;
        }
    }
}

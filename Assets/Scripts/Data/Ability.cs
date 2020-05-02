using System;
using Modifiers;
using Modifiers.Ability;
using Modifiers.Generic;
using Modifiers.StatusEffect;

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
                    result = new LowHealthBuffModifier(modEffect.playerMinRollChange,
                        modEffect.playerMaxRollChange);
                    break;
                case ModType.LUCKYHORSESHOE:
                    result = new LuckBuffModifier(3);
                    break;
                case ModType.HEALTHCHANGE:
                    result = new HealthChangeModifier(modEffect.playerHealthChange,
                        modEffect.playerMaxHealthChange);
                    break;
                case ModType.PROCRASTINATION:
                    result = new ProcrastinationModifier(modEffect.playerMinRollChange, 
                        modEffect.playerMaxRollChange);
                    break;
                case ModType.SPIKY:
                    result = new RecoilModifer("Spiky!");
                    break;
                case ModType.RECOVERY:
                    result = new RecoveryModifier(modEffect.playerHealthChange);
                    break;
                case ModType.VENOMOUS:
                    result = new InflictStatusOnHitModifier(Battle.StatusEffect.POISON, true,
                        "Venomous!", 3);
                    break;
                case ModType.CAREPACKAGE:
                    result = new HealthChangeModifier(
                        PlayerStatus.Status.MaxHealth - PlayerStatus.Status.Health, 0);
                    break;
                case ModType.HIGHSTAMINA:
                    result = new HighStaminaModifier();
                    break;
                case ModType.CONTAGIOUS:
                    result = new ContagiousModifier(2, 2, "Contagious!");
                    break;
                case ModType.SNOWBALL:
                    result = new SnowballModifier();
                    break;
                case ModType.SKILLED:
                    result = new TechBuffModifier(modEffect.playerMinRollChange,
                        modEffect.playerMaxRollChange);
                    break;
                case ModType.LIFEDRAIN:
                    result = new LifeDrainModifier(modEffect.playerHealthChange);
                    break;
                case ModType.OVERWHELMINGSPEED:
                    result = new OverwhelmingSpeedModifier(modEffect.playerMinRollChange, 
                        modEffect.playerMaxRollChange);
                    break;
                case ModType.THICKSKIN:
                    result = new ThickSkinModifier();
                    break;
                case ModType.RETALIATION:
                    result = new RetaliationModifier();
                    break;
                case ModType.OVERKILL:
                    result = new OverkillModifier();
                    break;
                case ModType.TAUNTING:
                    result = new InflictStatusOnHitModifier(Battle.StatusEffect.ENRAGED,
                        true, "Taunting!", 3);
                    break;
                case ModType.PATIENCE:
                    result = new PatienceModifier(modEffect.playerMinRollChange,
                        modEffect.playerMaxRollChange);
                    break;
                case ModType.RELIC:
                    result = new RelicModifier();
                    break;
            }
            if (result != null)
            {
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
}

using System;
using Modifiers;
using Modifiers.Tech;
using Modifiers.StatusEffect;
using Battle;

namespace Data
{
    // Player battle techniques
    [Serializable]
    public class Tech
    {
        public string name;
        public string displayName;
        public string description;
        public string playerStatusMessage;
        public string enemyStatusMessage;
        public int cooldownRolls;
        public int numRollsInEffect;
        public ModType modType;
        public ModEffect modEffect;

        public Modifier CreateTechModifier(BattleController battleController)
        {
            Modifier result = null;
            switch (modType)
            {
                case ModType.HEAVYSWING:
                    result = new HeavySwingModifier();
                    break;
                case ModType.RAGE:
                    result = new RageModifier();
                    break;
                case ModType.BULWARK:
                    result = new BulwarkModifier(modEffect.playerMinRollChange);
                    break;
                case ModType.SIDESWIPE:
                    result = new InflictStatusOnHitModifier(StatusEffect.BREAK,
                        "Sideswipe!", 2, battleController);
                    break;
                case ModType.TOPPLE:
                    result = new ToppleModifier(modEffect.playerMinRollChange,
                        modEffect.playerMaxRollChange);
                    break;
                case ModType.INFECT:
                    result = new InflictStatusOnHitModifier(StatusEffect.POISON,
                        "Infect!", 3, battleController);
                    break;
            }
            if (result != null)
            {
                result.isRollBounded = true;
                result.numRollsRemaining = numRollsInEffect;
                result.triggerChance = modEffect.baseModTriggerChance;
                result.priority = modEffect.modPriority;
                result.actor = modEffect.actor;
            }
            return result;
        }

        private int currentCooldown;

        public int GetCurrentCooldown()
        {
            return currentCooldown;
        }

        public void ActivateCooldown()
        {
            currentCooldown = cooldownRolls;
        }

        public void DecrementCooldown()
        {
            currentCooldown--;
        }

        public void ResetCooldown()
        {
            currentCooldown = 0;
        }

        public string GetDisplayName()
        {
            return string.IsNullOrEmpty(displayName) ? name : displayName;
        }
    }
}

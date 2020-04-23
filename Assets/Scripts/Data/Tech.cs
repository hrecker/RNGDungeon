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

        public Modifier CreateTechModifier()
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
                        false, "Sideswipe!", 2);
                    break;
                case ModType.TOPPLE:
                    result = new ToppleModifier(modEffect.playerMinRollChange,
                        modEffect.playerMaxRollChange);
                    break;
                case ModType.INFECT:
                    result = new InflictStatusOnHitModifier(StatusEffect.POISON,
                        false, "Infect!", 3);
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

        public int GetBaseCooldown()
        {
            return cooldownRolls;
        }

        public void ActivateCooldown()
        {
            // Apply tech cooldown mods
            int cooldown = cooldownRolls;
            foreach (ITechModifier mod in PlayerStatus.Status.Mods.GetTechModifiers())
            {
                cooldown = mod.ApplyTechCooldownModifier(cooldown);
            }
            cooldown = Math.Max(cooldown, 0);
            currentCooldown = cooldown;
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

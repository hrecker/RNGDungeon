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
        // This bool can be used to schedule this tech to 
        // activate it's cooldown after the roll completes
        public bool scheduledCooldownActivate;

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
                case ModType.OMEGASLASH:
                    result = new OmegaSlashModifier(modEffect.playerMinRollChange,
                        modEffect.playerMaxRollChange);
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
                cooldown = mod.ApplyTechCooldownModifier(this, cooldown);
            }
            // Cooldown can't be reduced below 1
            cooldown = Math.Max(cooldown, 1);
            currentCooldown = cooldown;
        }

        public void DecrementCooldown()
        {
            if (currentCooldown > 0)
            {
                currentCooldown--;
            }
        }

        // Updates cooldown post-roll. In almost every case this just decrements.
        public void UpdateCooldownPostRoll()
        {
            if (scheduledCooldownActivate)
            {
                ActivateCooldown();
                scheduledCooldownActivate = false;
            }
            else
            {
                DecrementCooldown();
            }
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

using Battle;

namespace Modifiers.Generic
{
    // Applies a one time health change effect
    public class HealthChangeModifier : Modifier, IOneTimeEffectModifier
    {
        private int healthChange, maxHealthChange;
        private string modMessage;

        public HealthChangeModifier(int healthChange, int maxHealthChange) :
            this(healthChange, maxHealthChange, null) { }

        public HealthChangeModifier(int healthChange, int maxHealthChange,
            string modMessage)
        {
            this.healthChange = healthChange;
            this.maxHealthChange = maxHealthChange;
            this.modMessage = modMessage;
        }

        public void ApplyOneTimeEffectMod()
        {
            Status().MaxHealth += maxHealthChange;
            Status().Health += healthChange;
            if (modMessage != null)
            {
                BattleController.AddModMessage(actor, modMessage);
            }
        }

        public bool CanApply()
        {
            return Status().Health > 0 && (maxHealthChange != 0 || 
                (healthChange > 0 && Status().Health < Status().MaxHealth) || 
                healthChange < 0);
        }
    }
}

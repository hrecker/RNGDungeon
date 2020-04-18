namespace Modifiers.Generic
{
    // Applies a one time health change effect
    public class HealthChangeModifier : Modifier, IOneTimeEffectModifier
    {
        private int healthChange, maxHealthChange;

        public HealthChangeModifier(int healthChange, int maxHealthChange)
        {
            this.healthChange = healthChange;
            this.maxHealthChange = maxHealthChange;
        }

        public void ApplyOneTimeEffectMod()
        {
            Status().MaxHealth += maxHealthChange;
            Status().Health += healthChange;
        }

        public bool CanApply()
        {
            return Status().Health > 0 && (maxHealthChange != 0 || 
                (healthChange > 0 && Status().Health < Status().MaxHealth) || 
                healthChange < 0);
        }
    }
}

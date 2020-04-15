namespace Modifiers.Generic
{
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
            PlayerStatus.MaxHealth += maxHealthChange;
            PlayerStatus.Health += healthChange;
        }

        public bool CanApply()
        {
            return PlayerStatus.Health > 0 && (maxHealthChange != 0 || 
                (healthChange > 0 && PlayerStatus.Health < PlayerStatus.MaxHealth) || 
                healthChange < 0);
        }
    }
}

using Battle;

namespace Modifiers.Ability
{
    // Saps health when a tech hits
    public class LifeDrainModifier : Modifier, IRollResultModifier
    {
        private int healthGain;

        public LifeDrainModifier(int healthGain)
        {
            priority = 5;
            this.healthGain = healthGain;
        }

        public RollResult ApplyRollResultMod(RollResult initial)
        {
            if (initial.GetRollDamage(actor.Opponent()) > 0 && initial.PlayerTech != null)
            {
                initial.AddHeal(actor, healthGain);
                BattleController.AddModMessage(actor, "Life Drain!");
            }
            return initial;
        }
    }
}

using Battle;

namespace Modifiers.Generic
{
    // Reduces roll damage taken
    public class RollDamageReductionModifier : Modifier, IRollResultModifier
    {
        protected int damageReduction = 1;

        public RollDamageReductionModifier(int damageReduction)
        {
            this.damageReduction = damageReduction;
            priority = 3;
        }

        public virtual RollResult ApplyRollResultMod(RollResult initial)
        {
            int rollDamage = initial.GetRollDamage(actor);
            if (rollDamage > 1)
            {
                rollDamage -= damageReduction;
                if (rollDamage < 1)
                {
                    rollDamage = 1;
                }
                initial.SetRollDamage(actor, rollDamage);
            }
            return initial;
        }
    }
}

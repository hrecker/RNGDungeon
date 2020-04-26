using Battle;

namespace Modifiers.Ability
{
    // Reduces roll damage taken
    public class ThickSkinModifier : Modifier, IRollResultModifier
    {
        private int damageReduction = 1;

        public ThickSkinModifier()
        {
            priority = 2;
        }

        public RollResult ApplyRollResultMod(RollResult initial)
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

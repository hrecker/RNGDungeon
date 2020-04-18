using Battle;

namespace Modifiers.Generic
{
    // Prevents damage
    public class BlockingModifier : Modifier, IRollResultModifier
    {
        public BlockingModifier()
        {
            priority = 3;
        }

        public RollResult ApplyRollResultMod(RollResult initial)
        {
            initial.SetDamage(actor, 0);
            return initial;
        }
    }
}

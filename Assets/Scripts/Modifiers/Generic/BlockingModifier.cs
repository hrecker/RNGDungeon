using Battle;

namespace Modifiers.Generic
{
    public class BlockingModifier : Modifier, IRollResultModifier
    {
        public RollResult ApplyRollResultMod(RollResult initial)
        {
            initial.PlayerDamage = 0;
            return initial;
        }
    }
}

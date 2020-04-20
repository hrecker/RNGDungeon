using Battle;

namespace Modifiers.Generic
{
    // Prevents roll damage
    public class BlockingModifier : Modifier, IRollResultModifier
    {
        public BlockingModifier()
        {
            priority = 3;
            battleEffect = RollBoundedBattleEffect.BLOCK;
        }

        public RollResult ApplyRollResultMod(RollResult initial)
        {
            initial.SetRollDamage(actor, 0);
            return initial;
        }
    }
}

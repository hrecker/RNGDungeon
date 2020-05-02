using Battle;
using Modifiers.Generic;

namespace Modifiers.Tech
{
    // Buffs max roll but deals 1 damage to the user
    public class RiskyKickModifier : RollBuffModifier, IRollResultModifier
    {
        public RiskyKickModifier(int maxRollBuff) : base(0, maxRollBuff) { }

        public RollResult ApplyRollResultMod(RollResult initial)
        {
            initial.AddNonRollDamage(actor, 1);
            return initial;
        }
    }
}

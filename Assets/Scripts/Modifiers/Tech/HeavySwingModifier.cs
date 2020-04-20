using System;
using Battle;

namespace Modifiers.Tech
{
    // Buff on the first roll, followed by two rolls of debuff
    public class HeavySwingModifier : Modifier, IRollGenerationModifier
    {
        private int rollCount;
        private const int rollBuff = 2;
        private const int rollDebuff = 1;
        private const int debuffRolls = 2;

        public Tuple<int, int> ApplyRollGenerationMod(int initialMinRoll, int initialMaxRoll)
        {
            if (rollCount == 0)
            {
                BattleController.AddModMessage(actor, "Heavy Swing!");
                initialMinRoll += rollBuff;
                initialMaxRoll += rollBuff;
                // Add debuff effect sprite for next two rolls
                battleEffect = RollBoundedBattleEffect.DEBUFF;
            }
            else if (rollCount <= debuffRolls)
            {
                initialMinRoll -= rollDebuff;
                initialMaxRoll -= rollDebuff;
            }
            rollCount++;
            return new Tuple<int, int>(initialMinRoll, initialMaxRoll);
        }
    }
}

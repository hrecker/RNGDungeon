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

        public RollGeneration ApplyRollGenerationMod(RollGeneration currentRollGen)
        {
            if (rollCount == 0)
            {
                BattleController.AddModMessage(actor, "Heavy Swing!");
                currentRollGen.MinRoll += rollBuff;
                currentRollGen.MaxRoll += rollBuff;
                // Add debuff effect sprite for next two rolls
                battleEffect = RollBoundedBattleEffect.DEBUFF;
            }
            else if (rollCount <= debuffRolls)
            {
                currentRollGen.MinRoll -= rollDebuff;
                currentRollGen.MaxRoll -= rollDebuff;
            }
            rollCount++;
            return currentRollGen;
        }
    }
}

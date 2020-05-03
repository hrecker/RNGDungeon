using Battle;
using Modifiers.Generic;

namespace Modifiers.Item
{
    // Buffs for first two rolls of each battle
    public class HeartyBreakfastModifier : RollBuffModifier, IPostBattleModifier
    {
        private int startingMinRollDiff;
        private int startingMaxRollDiff;

        public HeartyBreakfastModifier(int minRollDiff, int maxRollDiff) 
            : base(minRollDiff, maxRollDiff)
        {
            startingMinRollDiff = minRollDiff;
            startingMaxRollDiff = maxRollDiff;
            battleEffect = RollBoundedBattleEffect.BUFF;
        }

        // Reset for next battle
        public void ApplyPostBattleMod()
        {
            battleEffect = RollBoundedBattleEffect.BUFF;
            minRollDiff = startingMinRollDiff;
            maxRollDiff = startingMaxRollDiff;
        }

        public override RollGeneration ApplyRollGenerationMod(RollGeneration currentRollGen)
        {
            // Don't show as buff in inventory screen
            if (currentRollGen.CurrentRoll == -1)
            {
                return currentRollGen;
            }
            currentRollGen = base.ApplyRollGenerationMod(currentRollGen);
            if (currentRollGen.CurrentRoll == 2)
            {
                battleEffect = RollBoundedBattleEffect.NONE;
                minRollDiff = 0;
                maxRollDiff = 0;
            }
            return currentRollGen;
        }
    }
}

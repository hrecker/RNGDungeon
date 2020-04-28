using Battle;
using System;

namespace Modifiers.Ability
{
    // Ability that increasingly buffs roll when techs are used consecutively
    public class OverwhelmingSpeedModifier : Modifier, IRollGenerationModifier
    {
        private int minRollBuff;
        private int maxRollBuff;
        private int consecutiveTechCount;

        public OverwhelmingSpeedModifier(int minRollBuff, int maxRollBuff)
        {
            this.minRollBuff = minRollBuff;
            this.maxRollBuff = maxRollBuff;
        }

        public RollGeneration ApplyRollGenerationMod(RollGeneration currentRollGen)
        {
            if (currentRollGen.PlayerTech == null)
            {
                consecutiveTechCount = 0;
            }
            else
            {
                currentRollGen.MinRoll += (consecutiveTechCount * minRollBuff);
                currentRollGen.MaxRoll += (consecutiveTechCount * maxRollBuff);
                consecutiveTechCount++;
            }
            return currentRollGen;
        }
    }
}

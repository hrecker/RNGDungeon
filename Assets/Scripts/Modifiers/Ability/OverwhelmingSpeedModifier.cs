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

        public Tuple<int, int> ApplyRollGenerationMod(Data.Tech tech, int initialMinRoll, int initialMaxRoll)
        {
            if (tech == null)
            {
                consecutiveTechCount = 0;
            }
            else
            {
                initialMinRoll += (consecutiveTechCount * minRollBuff);
                initialMaxRoll += (consecutiveTechCount * maxRollBuff);
                consecutiveTechCount++;
            }
            return new Tuple<int, int>(initialMinRoll, initialMaxRoll);
        }
    }
}

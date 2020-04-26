using System;

namespace Modifiers.Generic
{
    // Apply a buff/debuff to min and/or max roll
    public class RollBuffModifier : Modifier, IRollGenerationModifier
    {
        private int minRollDiff;
        private int maxRollDiff;

        public RollBuffModifier(int minRollDiff, int maxRollDiff)
        {
            this.minRollDiff = minRollDiff;
            this.maxRollDiff = maxRollDiff;
        }

        public Tuple<int, int> ApplyRollGenerationMod(Data.Tech tech, int initialMinRoll, int initialMaxRoll)
        {
            initialMinRoll += minRollDiff;
            initialMaxRoll += maxRollDiff;
            return new Tuple<int, int>(initialMinRoll, initialMaxRoll);
        }
    }
}

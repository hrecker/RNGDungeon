using Battle;

namespace Modifiers.Generic
{
    // Apply a buff/debuff to min and/or max roll
    public class RollBuffModifier : Modifier, IRollGenerationModifier
    {
        protected int minRollDiff;
        protected int maxRollDiff;

        public RollBuffModifier(int minRollDiff, int maxRollDiff)
        {
            this.minRollDiff = minRollDiff;
            this.maxRollDiff = maxRollDiff;
        }

        public virtual RollGeneration ApplyRollGenerationMod(RollGeneration currentRollGen)
        {
            currentRollGen.MinRoll += minRollDiff;
            currentRollGen.MaxRoll += maxRollDiff;
            return currentRollGen;
        }
    }
}

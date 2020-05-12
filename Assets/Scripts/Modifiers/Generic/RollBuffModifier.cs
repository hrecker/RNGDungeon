using Battle;

namespace Modifiers.Generic
{
    // Apply a buff/debuff to min and/or max roll
    public class RollBuffModifier : Modifier, IRollGenerationModifier
    {
        protected int minRollDiff;
        protected int maxRollDiff;

        private string firstRollModMessage;
        private bool isFirstRoll;

        public RollBuffModifier(int minRollDiff, int maxRollDiff) : 
            this(minRollDiff, maxRollDiff, null) { }

        public RollBuffModifier(int minRollDiff, int maxRollDiff, string firstRollModMessage)
        {
            this.minRollDiff = minRollDiff;
            this.maxRollDiff = maxRollDiff;
            this.firstRollModMessage = firstRollModMessage;
            isFirstRoll = true;
        }

        public virtual RollGeneration ApplyRollGenerationMod(RollGeneration currentRollGen)
        {
            if (isFirstRoll)
            {
                BattleController.AddModMessage(actor, firstRollModMessage);
                isFirstRoll = false;
            }
            currentRollGen.MinRoll += minRollDiff;
            currentRollGen.MaxRoll += maxRollDiff;
            return currentRollGen;
        }

        public void SetMinRollDiff(int minRollDiff)
        {
            this.minRollDiff = minRollDiff;
        }

        public void SetMaxRollDiff(int maxRollDiff)
        {
            this.maxRollDiff = maxRollDiff;
        }
    }
}

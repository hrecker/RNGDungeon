namespace Modifiers.StatusEffect
{
    // Can extend statuses of this actor or the opponent
    public class StatusDurationModifier : Modifier, IStatusEffectModifier
    {
        private int statusDurationDiff;
        private int opponentStatusDurationDiff;

        public StatusDurationModifier(int statusDurationDiff, 
            int opponentStatusDurationDiff)
        {
            this.statusDurationDiff = statusDurationDiff;
            this.opponentStatusDurationDiff = opponentStatusDurationDiff;
        }

        public int GetOpponentStatusEffectDurationDiff()
        {
            return opponentStatusDurationDiff;
        }

        public int GetOpponentStatusEffectIntensityDiff()
        {
            return 0;
        }

        public int GetStatusEffectDurationDiff()
        {
            return statusDurationDiff;
        }

        public int GetStatusEffectIntensityDiff()
        {
            return 0;
        }
    }
}

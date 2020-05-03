namespace Modifiers.StatusEffect
{
    // Can increase intensity of statuses applying to this actor or the opponent
    public class StatusIntensityModifier : Modifier, IStatusEffectModifier
    {
        private int intensityDiff;
        private int opponentIntensityDiff;

        public StatusIntensityModifier(int intensityDiff, 
            int opponentIntensityDiff)
        {
            this.intensityDiff = intensityDiff;
            this.opponentIntensityDiff = opponentIntensityDiff;
        }

        public int GetOpponentStatusEffectDurationDiff()
        {
            return 0;
        }

        public int GetOpponentStatusEffectIntensityDiff()
        {
            return opponentIntensityDiff;
        }

        public int GetStatusEffectDurationDiff()
        {
            return 0;
        }

        public int GetStatusEffectIntensityDiff()
        {
            return intensityDiff;
        }
    }
}

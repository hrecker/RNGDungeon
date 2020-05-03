namespace Modifiers
{
    public interface IStatusEffectModifier
    {
        // Get diff for intensity of the status effect: 1 is default, higher numbers increase
        // (more poison damage, more recoil damage for rage, higher debuff for break)
        int GetStatusEffectIntensityDiff();

        // Same as above but applies to statuses that get put on the opponent
        int GetOpponentStatusEffectIntensityDiff();

        // Get a number of rolls to add/subtract from the duration of the status effect
        // (duration can't be reduced below 1)
        int GetStatusEffectDurationDiff();

        // Same as above but applies to statuses that get put on the opponent
        int GetOpponentStatusEffectDurationDiff();
    }
}

namespace Modifiers
{
    // Modifiers that have a one shot effect and then aren't used again
    public interface IOneTimeEffectModifier
    {
        void ApplyOneTimeEffectMod();

        // Check if this modifier can currently be used
        bool CanApply();
    }
}

using Battle;

namespace Modifiers
{
    // Modifiers affecting the results of the roll - how much damage is done
    public interface IRollResultModifier
    {
        RollResult ApplyRollResultMod(RollResult initial);
    }
}

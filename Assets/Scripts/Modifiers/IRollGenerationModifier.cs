using System;
using Battle;

namespace Modifiers
{
    // Modifiers that affect the min and max values used to generate a roll value
    public interface IRollGenerationModifier
    {
        RollGeneration ApplyRollGenerationMod(RollGeneration currentRollGen);
    }
}

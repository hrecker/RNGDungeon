using System;

// Modifiers that affect the min and max values used to generate a roll value
public interface IRollGenerationModifier
{
    Tuple<int, int> ApplyRollGenerationMod(int initialMinRoll, int initialMaxRoll);
}

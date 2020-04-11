using System;

public class RollBuffModifier : Modifier, IRollGenerationModifier
{
    private int minRollDiff;
    private int maxRollDiff;

    public RollBuffModifier(int minRollDiff, int maxRollDiff)
    {
        this.minRollDiff = minRollDiff;
        this.maxRollDiff = maxRollDiff;
    }

    public Tuple<int, int> ApplyRollGenerationMod(int initialMinRoll, int initialMaxRoll)
    {
        initialMinRoll += minRollDiff;
        initialMaxRoll += maxRollDiff;
        return new Tuple<int, int>(initialMinRoll, initialMaxRoll);
    }
}

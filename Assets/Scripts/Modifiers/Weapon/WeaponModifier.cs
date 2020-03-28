using System;

public class WeaponModifier : Modifier, IRollGenerationModifier
{
    private int minRollDiff;
    private int maxRollDiff;

    public WeaponModifier(int minRollDiff, int maxRollDiff)
    {
        this.minRollDiff = minRollDiff;
        this.maxRollDiff = maxRollDiff;
    }

    public Tuple<int, int> apply(int initialMinRoll, int initialMaxRoll, Stance currentStance)
    {
        initialMinRoll += minRollDiff;
        initialMaxRoll += maxRollDiff;
        return new Tuple<int, int>(initialMinRoll, initialMaxRoll);
    }
}

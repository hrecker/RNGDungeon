using System;

public class WeaponRollGenerationModifier : Modifier, IRollGenerationModifier
{
    private int minRollDiff;
    private int maxRollDiff;

    public WeaponRollGenerationModifier(int minRollDiff, int maxRollDiff)
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

    public override void DeregisterSelf()
    {
        PlayerStatus.Mods.DeregisterModifier((IRollGenerationModifier)this);
    }
}

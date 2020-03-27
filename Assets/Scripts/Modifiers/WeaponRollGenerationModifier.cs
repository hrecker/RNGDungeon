using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRollGenerationModifier : RollGenerationModifier
{
    private int minRollDiff;
    private int maxRollDiff;

    public WeaponRollGenerationModifier(int minRollDiff, int maxRollDiff)
    {
        this.minRollDiff = minRollDiff;
        this.maxRollDiff = maxRollDiff;
    }

    public override Tuple<int, int> apply(int initialMinRoll, int initialMaxRoll, Stance currentStance)
    {
        initialMinRoll += minRollDiff;
        initialMaxRoll += maxRollDiff;
        return new Tuple<int, int>(initialMinRoll, initialMaxRoll);
    }
}

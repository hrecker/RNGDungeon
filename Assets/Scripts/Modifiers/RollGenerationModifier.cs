using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Modifiers affects the min and max values used to generate a roll value
public abstract class RollGenerationModifier : Modifier
{
    public abstract Tuple<int, int> apply(
        int initialMinRoll, int initialMaxRoll, Stance currentStance);

    public override void DeregisterSelf()
    {
        PlayerStatus.Mods.DeregisterModifier(this);
    }
}

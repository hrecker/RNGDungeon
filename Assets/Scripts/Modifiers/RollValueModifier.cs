using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Modifiers affecting the values of the roll after it has been rolled
public abstract class RollValueModifier : Modifier
{
    public abstract Tuple<int, int> apply(int playerRoll, int enemyRoll);

    public override void DeregisterSelf()
    {
        PlayerStatus.Mods.DeregisterModifier(this);
    }
}

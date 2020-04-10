using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingModifier : Modifier, IRollResultModifier
{
    public RollResult apply(RollResult initial)
    {
        initial.PlayerDamage = 0;
        return initial;
    }
}

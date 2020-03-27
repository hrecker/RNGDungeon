using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalwartRollResultModifier : RollResultModifier
{
    public override RollResult apply(RollResult initial)
    {
        if (initial.PlayerDamage >= PlayerStatus.Health && RollTrigger())
        {
            initial.PlayerDamage--;
        }
        return initial;
    }
}

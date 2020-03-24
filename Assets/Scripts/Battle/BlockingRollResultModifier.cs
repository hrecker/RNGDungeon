using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingRollResultModifier : RollResultModifier
{
    public BlockingRollResultModifier(bool isPlayer) : base(isPlayer) { }

    public override RollResult apply(RollResult initial)
    {
        //TODO may cause issues with health gain related stuff
        if (isPlayer)
        {
            initial.PlayerDamage = 0;
        }
        else
        {
            initial.EnemyDamage = 0;
        }
        return initial;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilRollResultModifer : RollResultModifier
{
    public override RollResult apply(RollResult initial)
    {
        int damageReceived = initial.PlayerDamage;
        int recoilDamage = damageReceived > 0 ? 1 : 0;
        initial.EnemyDamage += recoilDamage;
        return initial;
    }
}

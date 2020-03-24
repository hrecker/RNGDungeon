using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilRollResultModifer : RollResultModifier
{
    public RecoilRollResultModifer(bool isPlayer) : base(isPlayer) { }

    public override RollResult apply(RollResult initial)
    {
        int damageReceived = initial.PlayerDamage;
        if (!isPlayer)
        {
            damageReceived = initial.EnemyDamage;
        }

        int recoilDamage = damageReceived > 0 ? 1 : 0;

        if (isPlayer)
        {
            initial.EnemyDamage += recoilDamage;
        }
        else
        {
            initial.PlayerDamage += recoilDamage;
        }
        return initial;
    }
}

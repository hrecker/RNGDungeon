using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBattleController : EnemyBattleController
{
    public int regenRate;
    private bool regenRoll;

    public override RollResult ApplyRollResultMods(RollResult initial)
    {
        // Slimes regen every other roll
        if (regenRoll)
        {
            initial.EnemyDamage -= regenRate;
        }
        regenRoll = !regenRoll;
        return initial;
    }
}

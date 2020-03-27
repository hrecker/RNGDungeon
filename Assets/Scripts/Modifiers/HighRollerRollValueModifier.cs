using System;
using UnityEngine;

public class HighRollerRollValueModifier : RollValueModifier
{
    public override Tuple<int, int> apply(int playerRoll, int enemyRoll)
    {
        if (RollTrigger())
        {
            playerRoll *= 2;
        }
        return new Tuple<int, int>(playerRoll, enemyRoll);
    }
}

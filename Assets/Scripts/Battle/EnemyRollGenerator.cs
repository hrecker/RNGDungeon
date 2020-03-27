using System;
using UnityEngine;

public class EnemyRollGenerator : RollGenerator
{
    public override int generateInitialRoll()
    {
        return generateBasicRoll(minRoll, maxRoll);
    }
}

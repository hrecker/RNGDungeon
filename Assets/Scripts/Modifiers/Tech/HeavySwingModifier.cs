using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavySwingModifier : Modifier, IRollGenerationModifier
{
    private int rollCount;
    private const int rollBuff = 2; // Buff min and max by two during heavy swing
    private const int rollDebuff = 1; // Debuff min and max by one after
    private const int debuffRolls = 2; // Debuff lasts two turns

    public Tuple<int, int> ApplyRollGenerationMod(int initialMinRoll, int initialMaxRoll)
    {
        // Heavy swing hits hard on first turn, then debuffs the next two turns
        if (rollCount == 0)
        {
            BattleController.AddPlayerModMessage("Heavy Swing!");
            initialMinRoll += rollBuff;
            initialMaxRoll += rollBuff;
        }
        else if (rollCount <= debuffRolls)
        {
            initialMinRoll -= rollDebuff;
            initialMaxRoll -= rollDebuff;
        }
        rollCount++;
        return new Tuple<int, int>(initialMinRoll, initialMaxRoll);
    }
}

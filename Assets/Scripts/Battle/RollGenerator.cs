using System;
using UnityEngine;

public abstract class RollGenerator : MonoBehaviour
{
    public int minRoll;
    public int maxRoll;

    // Generate an initial roll value
    public abstract int generateInitialRoll();

    // Apply modifiers to the intial roll of (player, enemy)
    public abstract Tuple<int, int> applyPostRollModifiers(Tuple<int, int> playerEnemyRolls);

    protected int generateBasicRoll(int min, int max)
    {
        return UnityEngine.Random.Range(min, max + 1);
    }
}

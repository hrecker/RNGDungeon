using System;
using UnityEngine;

public abstract class RollGenerator : MonoBehaviour
{
    public int minRoll;
    public int maxRoll;

    // Generate an initial roll value
    public abstract int GenerateInitialRoll();

    protected int GenerateBasicRoll(int min, int max)
    {
        return UnityEngine.Random.Range(min, max + 1);
    }
}

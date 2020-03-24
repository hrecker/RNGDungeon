using System;

[Serializable]
public class Level
{
    public int floor;
    public int width;
    public int height;
    public float encounterRate;
    public float[] enemyEncounterRates;
    public string[] enemies;
}

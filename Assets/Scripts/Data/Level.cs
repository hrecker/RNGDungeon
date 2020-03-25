using System;

[Serializable]
public class Level
{
    public int floor;
    public int width;
    public int height;
    public int floorItems;
    public float enemyItemDropRate;
    public float encounterRate;
    public float[] enemyEncounterRates;
    public string[] enemies;
}

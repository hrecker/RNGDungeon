using System;

[Serializable]
public class Level
{
    public int floor;
    public int roomWidth;
    public int roomHeight;
    public int numRooms;
    public int floorItems;
    public float enemyItemDropRate;
    public float encounterRate;
    public float[] enemyEncounterRates;
    public string[] enemies;
}

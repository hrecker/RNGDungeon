using System.Collections.Generic;
using UnityEngine;

public class CurrentLevel
{
    private static TileType[,] tiles;
    private static Vector3 playerStartingPosition;
    private static float encounterRate = 0.1f;

    public static string currentEnemyName = "Bat";
    private static List<string> levelEnemies = 
        new List<string>() { "Slime", "Bat" };
    private static List<float> enemyEncounterChances = 
        new List<float>() { 0.5f, 0.5f };

    public static void generateLevel(int width, int height)
    {
        tiles = new TileType[width, height];
        //TODO actual generation logic
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    tiles[x, y] = TileType.WALL;
                }
                else
                {
                    tiles[x, y] = TileType.FLOOR;
                }
            }
        }
        playerStartingPosition = new Vector3(1.5f, 1.5f, 0);
    }

    public static Vector3 GetPlayerStartingPosition()
    {
        return playerStartingPosition;
    }

    public static TileType[,] getTiles()
    {
        return tiles;
    }

    // Check if the player is allowed to move to the target position
    public static bool CheckMovement(Vector2 targetPosition)
    {
        return tiles[(int) targetPosition.x, (int) targetPosition.y] != TileType.WALL;
    }

    public static MoveResult Move(Vector2 targetPosition)
    {
        if (Random.value < encounterRate)
        {
            SelectEnemy();
            return MoveResult.BATTLE;
        }
        else
        {
            return MoveResult.NOTHING;
        }
    }

    private static void SelectEnemy()
    {
        float randomVal = Random.value;
        float totalEncounterChance = 0;
        currentEnemyName = "";
        for (int i = 0; i < enemyEncounterChances.Count; i++)
        {
            totalEncounterChance += enemyEncounterChances[i];
            if (randomVal <= totalEncounterChance)
            {
                currentEnemyName = levelEnemies[i];
                break;
            }
        }

        if (currentEnemyName == "")
        {
            currentEnemyName = levelEnemies[levelEnemies.Count - 1];
        }
    }
}

public enum TileType
{
    FLOOR,
    WALL,
    STAIRS
}

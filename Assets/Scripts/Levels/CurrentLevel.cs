using System.Collections.Generic;
using UnityEngine;

public class CurrentLevel
{
    private static TileType[,] tiles;
    private static Vector3 playerStartingPosition;
    private static TilemapPainter tilemapPainter;

    public static string currentEnemyName = "Bat";
    private static Level activeLevel;

    public static void SetTilemapPainter(TilemapPainter painter)
    {
        tilemapPainter = painter;
        if (tiles != null)
        {
            tilemapPainter.PaintLevel(tiles);
        }
    }

    public static void InitLevel(Level level)
    {
        activeLevel = level;
        tiles = new TileType[level.width, level.height];
        //TODO actual generation logic
        for (int x = 0; x < level.width; x++)
        {
            for (int y = 0; y < level.height; y++)
            {
                if (x == 0 || y == 0 || x == level.width - 1 || y == level.height - 1)
                {
                    tiles[x, y] = TileType.WALL;
                }
                else if (x == level.width - 2 && y == level.height - 2)
                {
                    tiles[x, y] = TileType.STAIRS;
                }
                else
                {
                    tiles[x, y] = TileType.FLOOR;
                }
            }
        }
        playerStartingPosition = new Vector3(1.5f, 1.5f, 0);
        if (tilemapPainter != null)
        {
            tilemapPainter.PaintLevel(tiles);
        }
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
        if (tiles[(int) targetPosition.x, (int) targetPosition.y] == TileType.STAIRS)
        {
            //TODO load next level
            Level nextLevel = Cache.GetLevel(activeLevel.floor + 1);
            if (nextLevel != null)
            {
                InitLevel(nextLevel);
                return MoveResult.STAIRSDOWN;
            }
        }

        if (Random.value < activeLevel.encounterRate)
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
        for (int i = 0; i < activeLevel.enemyEncounterRates.Length; i++)
        {
            totalEncounterChance += activeLevel.enemyEncounterRates[i];
            if (randomVal <= totalEncounterChance)
            {
                currentEnemyName = activeLevel.enemies[i];
                break;
            }
        }

        if (currentEnemyName == "")
        {
            currentEnemyName = activeLevel.enemies[0];
        }
    }
}

public enum TileType
{
    FLOOR,
    WALL,
    STAIRS
}

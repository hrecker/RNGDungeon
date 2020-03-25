using System.Collections.Generic;
using UnityEngine;

public class CurrentLevel
{
    private static TileType[,] tiles;
    private static Dictionary<Vector2Int, Item> floorItems;
    private static Vector3 playerStartingPosition;
    private static TilemapPainter tilemapPainter;

    public static string currentEnemyName = "Bat";
    private static Level activeLevel;
    public static string[] drops = new string[] { "HealthPotion", "BlockingPotion" };
    private static int dropsReceivedOnActiveLevel;
    private static int maxDropsPerFloor = 5;

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
        dropsReceivedOnActiveLevel = 0;
        floorItems = new Dictionary<Vector2Int, Item>();
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

        // Generate item locations
        int itemsGenerated = 0;
        while (itemsGenerated < activeLevel.floorItems)
        {
            int randomX = Random.Range(1, activeLevel.width - 2);
            int randomY = Random.Range(1, activeLevel.height - 2);
            if (tiles[randomX, randomY] == TileType.FLOOR && 
                (randomX != 1 || randomY != 1))
            {
                tiles[randomX, randomY] = TileType.ITEM;
                itemsGenerated++;
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
        int tileX = (int)targetPosition.x;
        int tileY = (int)targetPosition.y;
        if (tiles[tileX, tileY] == TileType.STAIRS)
        {
            Level nextLevel = Cache.GetLevel(activeLevel.floor + 1);
            if (nextLevel != null)
            {
                InitLevel(nextLevel);
                return MoveResult.STAIRSDOWN;
            }
        }

        if (tiles[tileX, tileY] == TileType.ITEM)
        {
            tiles[tileX, tileY] = TileType.FLOOR;
            if (tilemapPainter != null)
            {
                tilemapPainter.PaintLevel(tiles);
            }
            return MoveResult.ITEMPICKUP;
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

    public static string GetEnemyItemDrop()
    {
        float dropRate = (maxDropsPerFloor - dropsReceivedOnActiveLevel) / 
            (float)maxDropsPerFloor * activeLevel.enemyItemDropRate;
        if (Random.value < dropRate)
        {
            dropsReceivedOnActiveLevel++;
            return drops[Random.Range(0, drops.Length)];
        }
        return null;
    }
}

public enum TileType
{
    FLOOR,
    WALL,
    STAIRS,
    ITEM
}

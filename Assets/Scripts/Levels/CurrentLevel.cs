using System.Collections.Generic;
using UnityEngine;

public class CurrentLevel
{
    private static Tile[,] tiles;
    private static Dictionary<Vector2Int, Item> floorItems;
    private static Vector3 playerStartingPosition;
    private static TilemapPainter tilemapPainter;
    private static LevelSpriteController spriteController;

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

    public static void SetSpriteController(LevelSpriteController controller)
    {
        spriteController = controller;
        if (tiles != null)
        {
            spriteController.DrawSprites(tiles);
        }
    }

    public static void InitLevel(Level level)
    {
        dropsReceivedOnActiveLevel = 0;
        floorItems = new Dictionary<Vector2Int, Item>();
        activeLevel = level;

        LevelGenerator generator = new LevelGenerator(activeLevel);
        generator.GenerateLevel();
        tiles = generator.GetTiles();
        playerStartingPosition = generator.GetPlayerStartingPosition();

        if (tilemapPainter != null)
        {
            tilemapPainter.PaintLevel(tiles);
        }
        if (spriteController != null)
        {
            spriteController.DrawSprites(tiles);
        }
    }

    public static Vector3 GetPlayerStartingPosition()
    {
        return playerStartingPosition;
    }

    // Check if the player is allowed to move to the target position
    public static bool CheckMovement(Vector2 targetPosition)
    {
        return tiles[(int) targetPosition.x, (int) targetPosition.y].tileType != TileType.WALL;
    }

    public static MoveResult Move(Vector2 targetPosition)
    {
        int tileX = (int)targetPosition.x;
        int tileY = (int)targetPosition.y;
        if (tiles[tileX, tileY].tileType == TileType.STAIRS)
        {
            Level nextLevel = Cache.GetLevel(activeLevel.floor + 1);
            if (nextLevel != null)
            {
                InitLevel(nextLevel);
                // Remove any keys the player has
                //TODO reward for taking keys through to next level
                PlayerStatus.KeyCount = 0;
                return MoveResult.STAIRSDOWN;
            }
            else
            {
                // Initialize boss fight
                // reset tech cooldowns
                foreach (Tech tech in PlayerStatus.EnabledTechs)
                {
                    tech.ResetCooldown();
                }
                // set next enemy
                currentEnemyName = "Boss";
                return MoveResult.BATTLE;
            }
        }

        switch (tiles[tileX, tileY].tileContents)
        {
            case TileContents.ITEM:
                tiles[tileX, tileY].tileContents = TileContents.NONE;
                spriteController.RemoveItem(new Vector2Int(tileX, tileY));
                return MoveResult.ITEMPICKUP;
            case TileContents.LOCKED_CHEST:
                if (PlayerStatus.KeyCount > 0)
                {
                    PlayerStatus.KeyCount--;
                    tiles[tileX, tileY].tileContents = TileContents.UNLOCKED_CHEST;
                    spriteController.UnlockChest(new Vector2Int(tileX, tileY));
                    return MoveResult.CHESTOPEN;
                }
                return MoveResult.NOTHING;
            case TileContents.UNLOCKED_CHEST:
                return MoveResult.NOTHING;
                //TODO collectors

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
        // Collectors always drop keys
        if (currentEnemyName == "Collector")
        {
            return "Key";
        }

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
    NONE,
    FLOOR,
    WALL,
    STAIRS
}

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
    private static int dropsReceivedOnActiveLevel;
    private static int maxDropsPerFloor = 5;

    public static int GetCurrentFloorNumber()
    {
        if (activeLevel != null)
        {
            return activeLevel.floor;
        }
        return 1;
    }

    public static void SetTilemapPainter(TilemapPainter painter)
    {
        tilemapPainter = painter;
        if (tiles != null)
        {
            tilemapPainter.PaintLevel(tiles, GetCurrentFloorNumber());
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
        tiles[(int)playerStartingPosition.x, (int)playerStartingPosition.y].playerInTile = true;

        if (tilemapPainter != null)
        {
            tilemapPainter.PaintLevel(tiles, GetCurrentFloorNumber());
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

    // Check if an enemy is allowed to move to the target position
    public static bool CheckEnemyMovement(Vector2 targetPosition)
    {
        Tile targetTile = tiles[(int)targetPosition.x, (int)targetPosition.y];
        return targetTile.tileType == TileType.FLOOR && 
            targetTile.tileContents == TileContents.NONE && !targetTile.playerInTile;
    }

    public static MoveResult Move(Vector2 originalPosition, Vector2 targetPosition)
    {
        int tileX = (int)targetPosition.x;
        int tileY = (int)targetPosition.y;
        tiles[tileX, tileY].playerInTile = true;
        tiles[(int)originalPosition.x, (int)originalPosition.y].playerInTile = false;
        if (tiles[tileX, tileY].tileType == TileType.STAIRS)
        {
            Level nextLevel = Cache.GetLevel(activeLevel.floor + 1);
            if (nextLevel != null)
            {
                InitLevel(nextLevel);
                // TODO may want to remove keys player has with some reward
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
            case TileContents.COLLECTOR:
                currentEnemyName = "Collector";
                tiles[tileX, tileY].tileContents = TileContents.NONE;
                spriteController.RemoveCollector(new Vector2Int(tileX, tileY));
                return MoveResult.BATTLE;
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

    public static void MoveCollector(Vector2 originalPosition, Vector2 targetPosition)
    {
        tiles[(int)originalPosition.x, (int)originalPosition.y].tileContents = TileContents.NONE;
        tiles[(int)targetPosition.x, (int)targetPosition.y].tileContents = TileContents.COLLECTOR;
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

    public static Item GetEnemyItemDrop()
    {
        // Collectors always drop keys
        if (currentEnemyName == "Collector")
        {
            return Cache.GetItem("Key");
        }

        float dropRate = (maxDropsPerFloor - dropsReceivedOnActiveLevel) / 
            (float)maxDropsPerFloor * activeLevel.enemyItemDropRate;
        if (Random.value < dropRate)
        {
            dropsReceivedOnActiveLevel++;
            Dictionary<Rarity, float> rarityChances = new Dictionary<Rarity, float>()
            {
                { Rarity.COMMON, 0.8f },
                { Rarity.UNCOMMON, 0.15f },
                { Rarity.RARE, 0.05f }
            };
            return Cache.GetRandomItem(rarityChances);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CustomTile : Tile
{
    private TileType type;

    public Sprite floor;
    public Sprite stairs;
    public Sprite itemFloor;
    public Sprite[] wallSprites;

    public void Init(TileType type)
    {
        this.type = type;
        //TODO maybe make this less hard coded at some point
        Object[] wallSpriteObjects = Resources.LoadAll("Floor1Tilemap");
        wallSprites = new Sprite[16];
        for (int i = 0; i < 16; i++)
        {
            wallSprites[i] = (Sprite)wallSpriteObjects[i + 1];
        }
        floor = (Sprite)wallSpriteObjects[17];
        stairs = (Sprite)wallSpriteObjects[18];
        itemFloor = (Sprite)wallSpriteObjects[19];
    }

    public TileType GetTileType()
    {
        return type;
    }

    public override void RefreshTile(Vector3Int location, ITilemap tilemap)
    {
        if (type != TileType.WALL)
        {
            tilemap.RefreshTile(location);
        }

        for (int yd = -1; yd <= 1; yd++)
        {
            for (int xd = -1; xd <= 1; xd++)
            {
                Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);
                if (HasWallTile(tilemap, position))
                {
                    tilemap.RefreshTile(position);
                }
            }
        }
    }

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        if (type == TileType.NONE)
        {
            tileData.sprite = null;
            return;
        }
        if (type == TileType.FLOOR)
        {
            tileData.sprite = floor;
            return;
        }
        if (type == TileType.STAIRS)
        {
            tileData.sprite = stairs;
            return;
        }
        if (type == TileType.ITEM)
        {
            tileData.sprite = itemFloor;
            return;
        }

        int mask = HasWallTile(tilemap, location + new Vector3Int(0, 1, 0)) ? 1 : 0;
        mask += HasWallTile(tilemap, location + new Vector3Int(1, 0, 0)) ? 2 : 0;
        mask += HasWallTile(tilemap, location + new Vector3Int(0, -1, 0)) ? 4 : 0;
        mask += HasWallTile(tilemap, location + new Vector3Int(-1, 0, 0)) ? 8 : 0;
        if (mask >= 0 && mask < wallSprites.Length)
        {
            tileData.sprite = wallSprites[mask];
        }
        else
        {
            Debug.LogWarning("Not enough sprites in CustomTile instance");
        }
    }

    private bool HasWallTile(ITilemap tilemap, Vector3Int position)
    {
        CustomTile tile = (CustomTile) tilemap.GetTile(position);
        if (tile != null)
        {
            return tile.GetTileType() == TileType.WALL || 
                tile.GetTileType() == TileType.NONE;
        }
        else
        {
            return true;
        }
    }
}

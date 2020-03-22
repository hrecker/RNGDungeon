using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapPainter : MonoBehaviour
{
    //TODO these probably could go in some config file somewhere
    public int width, height;
    public Tilemap tileMap;

    void Awake()
    {
        //TODO won't want to do this on awake, only when new level is needed
        CurrentLevel.generateLevel(width, height);
        paintLevel(CurrentLevel.getTiles());
    }

    private void paintLevel(TileType[,] tiles)
    {
        tileMap.ClearAllTiles();
        int xLength = tiles.GetLength(0);
        int yLength = tiles.GetLength(1);
        for (int x = 0; x < xLength; x++)
        {
            for (int y = 0; y < yLength; y++)
            {
                CustomTile tile = ScriptableObject.CreateInstance<CustomTile>();
                tile.Init(tiles[x, y]);
                tileMap.SetTile(new Vector3Int(x - (xLength / 2), y - (yLength / 2), 0), tile);
            }
        }
    }
}

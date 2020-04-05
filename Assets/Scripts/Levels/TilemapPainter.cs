using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapPainter : MonoBehaviour
{
    public Tilemap tileMap;

    void Start()
    {
        CurrentLevel.SetTilemapPainter(this);
    }

    public void PaintLevel(Tile[,] tiles)
    {
        tileMap.ClearAllTiles();
        int xLength = tiles.GetLength(0);
        int yLength = tiles.GetLength(1);
        for (int x = 0; x < xLength; x++)
        {
            for (int y = 0; y < yLength; y++)
            {
                CustomTile tile = ScriptableObject.CreateInstance<CustomTile>();
                tile.Init(tiles[x, y].tileType);
                tileMap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }
}

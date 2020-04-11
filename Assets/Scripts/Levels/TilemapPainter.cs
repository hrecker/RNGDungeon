using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels
{
    public class TilemapPainter : MonoBehaviour
    {
        public Tilemap tileMap;
        public Color[] levelColors;

        void Start()
        {
            CurrentLevel.SetTilemapPainter(this);
        }

        public void PaintLevel(Tile[,] tiles, int floor)
        {
            tileMap.ClearAllTiles();
            SetTilemapColor(floor);
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

        private void SetTilemapColor(int floor)
        {
            int colorIndex = floor > levelColors.Length ? levelColors.Length - 1 : floor - 1;
            tileMap.color = levelColors[colorIndex];
        }
    }
}

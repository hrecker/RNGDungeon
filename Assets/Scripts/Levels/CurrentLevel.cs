public class CurrentLevel
{
    private static TileType[,] tiles;

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
    }

    public static TileType[,] getTiles()
    {
        return tiles;
    }
}

public enum TileType
{
    FLOOR,
    WALL,
    STAIRS
}

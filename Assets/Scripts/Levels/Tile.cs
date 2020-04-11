namespace Levels
{
    // Represents a tile in the game map
    public class Tile
    {
        public TileType tileType;
        public TileContents tileContents;
        public bool playerInTile;
    }

    public enum TileContents
    {
        NONE,
        ITEM,
        COLLECTOR,
        LOCKED_CHEST,
        UNLOCKED_CHEST
    }
}

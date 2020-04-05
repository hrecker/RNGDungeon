// Represents a tile in the game map
public class Tile
{
    public TileType tileType;
    public TileContents tileContents;
}

public enum TileContents
{
    NONE,
    PLAYER,
    ITEM,
    COLLECTOR,
    LOCKED_CHEST,
    UNLOCKED_CHEST
}

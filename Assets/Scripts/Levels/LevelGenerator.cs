using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator
{
    private Level level;
    private TileType[,] tiles;
    private Vector3 playerStartingPosition;
    private float roomChance;
    private bool generated;
    private List<Vector2Int> takenRoomCoords;

    public LevelGenerator(Level level)
    {
        if (level.numRooms < Math.Max((level.floorItems + 2), 5))
        {
            throw new Exception("Invalid room count: level must have enough " +
                "rooms for all floor items, and a minimum of 5 rooms");
        }
        if (level.roomWidth < 3 || level.roomHeight < 3)
        {
            throw new Exception("Invalid room width or height: must be at least 3");
        }

        this.level = level;
        roomChance = 0.5f;
        takenRoomCoords = new List<Vector2Int>();
    }

    public TileType[,] GetTiles()
    {
        return tiles;
    }

    public Vector3 GetPlayerStartingPosition()
    {
        return playerStartingPosition;
    }

    public void GenerateLevel()
    {
        if (generated)
        {
            return;
        }

        int maxTileWidth = level.roomWidth * (int) Math.Ceiling(level.numRooms / 2.0f + 2);
        int maxTileHeight = level.roomHeight * (int)Math.Ceiling(level.numRooms / 2.0f + 2);
        tiles = new TileType[maxTileWidth, maxTileHeight];

        // Create entrance room
        playerStartingPosition = new Vector3(maxTileWidth / 2 + 0.5f, maxTileHeight / 2 + 0.5f, 0);
        Room entrance = GenerateRoom((int)playerStartingPosition.x - (level.roomWidth / 2),
            (int)playerStartingPosition.y - (level.roomHeight / 2), 
            Vector2Int.zero,  RoomType.ENTRANCE);
        List<Room> allRooms = new List<Room> { entrance };
        List<Room> leafRooms = new List<Room> { entrance };
        int roomsGenerated = 1;

        // Generate rooms
        while (roomsGenerated < level.numRooms)
        {
            List<Room> newLeafs = new List<Room>();
            foreach (Room leaf in leafRooms)
            {
                List<Room> generated = GenerateAdjacentRooms(leaf, level.numRooms - roomsGenerated);
                allRooms.AddRange(generated);
                roomsGenerated += generated.Count;

                if (leaf.northRoom != null)
                {
                    newLeafs.Add(leaf.northRoom);
                }
                if (leaf.eastRoom != null)
                {
                    newLeafs.Add(leaf.eastRoom);
                }
                if (leaf.southRoom != null)
                {
                    newLeafs.Add(leaf.southRoom);
                }
                if (leaf.westRoom != null)
                {
                    newLeafs.Add(leaf.westRoom);
                }
            }

            if (newLeafs.Count > 0)
            {
                leafRooms = newLeafs;
            }
            else
            {
                leafRooms.Clear();
                leafRooms.Add(entrance);
                roomChance += 0.1f;
            }
        }

        // Set item and exit rooms
        allRooms.Remove(entrance);
        for (int i = 0; i < level.floorItems; i++)
        {
            // set items
            Room itemRoom = allRooms[UnityEngine.Random.Range(0, allRooms.Count)];
            Vector2Int itemPosition = GetRandomRoomTile(itemRoom);
            tiles[itemPosition.x, itemPosition.y] = TileType.ITEM;
            allRooms.Remove(itemRoom);
        }
        //Set exit
        Room exitRoom = allRooms[UnityEngine.Random.Range(0, allRooms.Count)];
        Vector2Int exitPosition = GetRandomRoomTile(exitRoom);
        tiles[exitPosition.x, exitPosition.y] = TileType.STAIRS;
        allRooms.Remove(exitRoom);

        generated = true;
    }

    private Room GenerateRoom(int minFloorX, int minFloorY, Vector2Int roomCoords)
    {
        return GenerateRoom(minFloorX, minFloorY, roomCoords, RoomType.DEFAULT);
    }

    private Room GenerateRoom(int minFloorX, int minFloorY, Vector2Int roomCoords, RoomType roomType)
    {
        Room newRoom = new Room()
        {
            minFloorX = minFloorX,
            minFloorY = minFloorY,
            roomCoords = roomCoords,
            roomType = roomType
        };
        takenRoomCoords.Add(roomCoords);
        UpdateTiles(newRoom);
        return newRoom;
    }

    private void UpdateTiles(Room room)
    { 
        for (int x = -1; x < level.roomWidth + 1; x++)
        {
            for (int y = -1; y < level.roomHeight + 1; y++)
            {
                int tileX = room.minFloorX + x;
                int tileY = room.minFloorY + y;
                if ((x == -1 || y == -1 || x == level.roomWidth || y == level.roomHeight) && 
                    tiles[tileX, tileY] != TileType.FLOOR)
                {
                    tiles[tileX, tileY] = TileType.WALL;
                }
                else
                {
                    tiles[tileX, tileY] = TileType.FLOOR;
                }
            }
        }
    }

    private List<Room> GenerateAdjacentRooms(Room baseRoom, int maxToGenerate)
    {
        List<Room> newRooms = new List<Room>();
        if (IsNorthSpaceFree(baseRoom) && UnityEngine.Random.value < roomChance &&
            newRooms.Count < maxToGenerate)
        {
            // Generate north room
            baseRoom.northRoom = GenerateRoom(baseRoom.minFloorX,
                baseRoom.minFloorY + level.roomHeight + 1, 
                baseRoom.roomCoords + Vector2Int.up);
            // Add hallway floor tile
            tiles[baseRoom.northRoom.minFloorX + (level.roomWidth / 2), 
                baseRoom.northRoom.minFloorY - 1] = TileType.FLOOR;
            newRooms.Add(baseRoom.northRoom);
        }
        if (IsEastSpaceFree(baseRoom) && UnityEngine.Random.value < roomChance &&
            newRooms.Count < maxToGenerate)
        {
            // Generate east room
            baseRoom.eastRoom = GenerateRoom(baseRoom.minFloorX + level.roomWidth + 1,
                baseRoom.minFloorY,
                baseRoom.roomCoords + Vector2Int.right);
            // Add hallway floor tile
            tiles[baseRoom.eastRoom.minFloorX - 1,
                baseRoom.eastRoom.minFloorY + (level.roomHeight / 2)] = TileType.FLOOR;
            newRooms.Add(baseRoom.eastRoom);
        }
        if (IsSouthSpaceFree(baseRoom) && UnityEngine.Random.value < roomChance &&
            newRooms.Count < maxToGenerate)
        {
            // Generate south room
            baseRoom.southRoom = GenerateRoom(baseRoom.minFloorX,
                baseRoom.minFloorY - level.roomHeight - 1,
                baseRoom.roomCoords + Vector2Int.down);
            // Add hallway floor tile
            tiles[baseRoom.southRoom.minFloorX + (level.roomWidth / 2),
                baseRoom.minFloorY - 1] = TileType.FLOOR;
            newRooms.Add(baseRoom.southRoom);
        }
        if (IsWestSpaceFree(baseRoom) && UnityEngine.Random.value < roomChance &&
            newRooms.Count < maxToGenerate)
        {
            // Generate west room
            baseRoom.westRoom = GenerateRoom(baseRoom.minFloorX - level.roomWidth - 1,
                baseRoom.minFloorY,
                baseRoom.roomCoords + Vector2Int.left);
            // Add hallway floor tile
            tiles[baseRoom.minFloorX - 1,
                baseRoom.westRoom.minFloorY + (level.roomHeight / 2)] = TileType.FLOOR;
            newRooms.Add(baseRoom.westRoom);
        }
        return newRooms;
    }

    private bool IsNorthSpaceFree(Room room)
    {
        return room.northRoom == null &&
            !takenRoomCoords.Contains(room.roomCoords + Vector2Int.up) &&
            (room.minFloorY + (2 * level.roomHeight) + 1) < tiles.GetLength(1);
    }

    private bool IsEastSpaceFree(Room room)
    {
        return room.eastRoom == null &&
            !takenRoomCoords.Contains(room.roomCoords + Vector2Int.right) &&
            (room.minFloorX + (2 * level.roomWidth) + 1) < tiles.GetLength(0);
    }

    private bool IsSouthSpaceFree(Room room)
    {
        return room.southRoom == null &&
            !takenRoomCoords.Contains(room.roomCoords + Vector2Int.down) &&
            (room.minFloorY - level.roomHeight - 2) >= 0;
    }

    private bool IsWestSpaceFree(Room room)
    {
        return room.westRoom == null &&
            !takenRoomCoords.Contains(room.roomCoords + Vector2Int.left) &&
            (room.minFloorX - level.roomWidth - 2) >= 0;
    }

    private Vector2Int GetRandomRoomTile(Room room)
    {
        int itemX = room.minFloorX + UnityEngine.Random.Range(0, level.roomWidth);
        int itemY = room.minFloorY + UnityEngine.Random.Range(0, level.roomHeight);
        return new Vector2Int(itemX, itemY);
    }
}

class Room
{
    public int minFloorX, minFloorY;
    public Vector2Int roomCoords; // Coordinates in terms of rooms; entrance is (0, 0).
    public RoomType roomType;
    public Room northRoom, eastRoom, southRoom, westRoom;
}

enum RoomType
{
    DEFAULT,
    ITEM,
    ENTRANCE,
    EXIT
}

using UnityEngine;
using System.Collections.Generic;

namespace FallingSloth.ProceduralMazeGenerator
{
    public class Dungeon : MonoBehaviour
    {
        [Range(7, 200)]
        public int width = 7, height = 7;

        [Range(3, 20)]
        public int minRoomSize = 3, maxRoomSize = 12;

        [Range(1, 100)]
        public int minRoomCount = 1, maxRoomCount = 20;

        //public bool allowRoomOverlap = false;

        public Tile tilePrefab;

        public Tile[,] tiles;

        [HideInInspector]
        public List<DungeonRoom> rooms;

        public Tile this[int x, int y]
        {
            get { return tiles[x, y]; }
            set { tiles[x, y] = value; }
        }

        public void GenerateDungeon()
        {
            if (tiles != null)
            {
                for (int x = 0; x < tiles.GetLength(0); x++)
                    for (int y = 0; y < tiles.GetLength(1); y++)
                        Destroy(tiles[x, y].gameObject);
            }

            tiles = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tiles[x, y] = Instantiate(tilePrefab, this.transform);
                    tiles[x, y].x = x;
                    tiles[x, y].y = y;
                    tiles[x, y].parentArray = tiles;
                }
            }

            rooms = new List<DungeonRoom>();
            GenerateRooms();

            GenerateCorridors();
        }

        void GenerateRooms()
        {
            if (minRoomSize > maxRoomSize)
            {
                int t = minRoomSize;
                minRoomSize = maxRoomSize;
                maxRoomSize = t;
            }

            DungeonRoom temp;
            for (int r = 0; r < Random.Range(minRoomCount, maxRoomCount + 1); r++)
            {
                temp = DungeonRoom.GenerateNewRoom(minRoomSize, maxRoomSize, width, height);
                for (int x = temp.left; x <= temp.right; x++)
                {
                    for (int y = temp.bottom; y <= temp.top; y++)
                    {
                        if (tiles[x, y].availability != TileAvailability.Room)
                            tiles[x, y].availability = TileAvailability.Room;
                    }
                }
                rooms.Add(temp);
            }

            foreach (DungeonRoom room in rooms)
                SetupRoomTiles(room);
        }

        void SetupRoomTiles(DungeonRoom room)
        {
            for (int x = room.left; x <= room.right; x++)
            {
                for (int y = room.bottom; y <= room.top; y++)
                {
                    if (x > room.left) tiles[x, y].corridors |= Directions.West;
                    if (x == room.left && x > 0 && tiles[x - 1, y].availability == TileAvailability.Room) tiles[x, y].corridors |= Directions.West;

                    if (x < room.right) tiles[x, y].corridors |= Directions.East;
                    if (x == room.right && x < this.width-1 && tiles[x + 1, y].availability == TileAvailability.Room) tiles[x, y].corridors |= Directions.East;

                    if (y > room.bottom) tiles[x, y].corridors |= Directions.South;
                    if (y == room.bottom && y > 0 && tiles[x, y - 1].availability == TileAvailability.Room) tiles[x, y].corridors |= Directions.South;

                    if (y < room.top) tiles[x, y].corridors |= Directions.North;
                    if (y == room.top && y < this.height-1 && tiles[x, y + 1].availability == TileAvailability.Room) tiles[x, y].corridors |= Directions.North;
                }
            }
        }

        void GenerateCorridors()
        {
            int x = Random.Range(0, width), y = Random.Range(0, height);
            while (tiles[x,y].availability != TileAvailability.Empty)
            {
                x = Random.Range(0, width);
                y = Random.Range(0, height);
            }
            Debug.Log("Starting corridor generation at (" + x + ", " + y + ")");
            RecursiveCorridorGenerator(x, y);
        }

        void RecursiveCorridorGenerator(int x, int y)
        {
            tiles[x, y].availability = TileAvailability.Maze;

            List<Directions> directions = new List<Directions>();
            if (y > 0) directions.Add(Directions.South);
            if (y < height - 1) directions.Add(Directions.North);
            if (x > 0) directions.Add(Directions.West);
            if (x < width - 1) directions.Add(Directions.East);
            directions = directions.Shuffle();

            foreach (Directions direction in directions)
            {
                int xOffset = 0, yOffset = 0;
                switch (direction)
                {
                    case Directions.North:
                        yOffset = 1;
                        break;
                    case Directions.East:
                        xOffset = 1;
                        break;
                    case Directions.South:
                        yOffset = -1;
                        break;
                    case Directions.West:
                        xOffset = -1;
                        break;
                }
                if (tiles[x + xOffset, y + yOffset].availability == TileAvailability.Empty)
                {
                    tiles[x, y][direction] = true;
                    tiles[x + xOffset, y + yOffset][Utility.OppositeDirection(direction)] = true;
                    RecursiveCorridorGenerator(x + xOffset, y + yOffset);
                }
            }
        }
    }
}
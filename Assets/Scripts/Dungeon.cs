using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Collections;

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

        [Range(1, 2000)]
        public int maxRoomPlaceAttempts = 1;

        [Range(0.01f, 1f)]
        public float delayTime = 0.05f;

        public bool showWorking = false;

        public Tile tilePrefab;

        public char mazeChar = '*', roomChar = '#';

        public Tile[,] tiles;

        public TMPro.TextMeshProUGUI timeText;

        public float lastDungeonTime { get; protected set; }

        List<Vector2Int> deadEnds;

        [HideInInspector]
        public List<DungeonRoom> rooms;

        public delegate void DungeonCompleteCallbackDelegate();
        public DungeonCompleteCallbackDelegate dungeonCompleteCallback;

        public bool isRunning { get; protected set; }

        public Tile this[int x, int y]
        {
            get { return tiles[x, y]; }
            set { tiles[x, y] = value; }
        }

        float startTime = 0;

        public void OutputCurrentDungeon()
        {
            using (StreamWriter output = new StreamWriter(Application.persistentDataPath + "/dungeon output.txt", true))
            {
                output.WriteLine();
                output.WriteLine();
                output.WriteLine(this.ToString());
            }
        }

        public void GenerateDungeon()
        {
            if (!isRunning)
            {
                isRunning = true;
                StartCoroutine(GenerateDungeonAsync());
            }
        }

        IEnumerator GenerateDungeonAsync()
        {
            yield return null;

            startTime = Time.realtimeSinceStartup;

            if (maxRoomPlaceAttempts < minRoomCount)
                maxRoomPlaceAttempts = minRoomCount;

            #region Clear existing tiles
            if (tiles != null)
            {
                for (int x = 0; x < tiles.GetLength(0); x++)
                    for (int y = 0; y < tiles.GetLength(1); y++)
                        Destroy(tiles[x, y].gameObject);
            }
            #endregion

            #region Generate new tiles with default values
            tiles = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tiles[x, y] = Instantiate(tilePrefab, this.transform);
                    tiles[x, y].gameObject.name = string.Format("Tile({0}, {1})", x, y);
                    tiles[x, y].x = x;
                    tiles[x, y].y = y;
                    tiles[x, y].parentArray = tiles;
                }
            }
            #endregion

            #region Generate dungeon rooms
            rooms = new List<DungeonRoom>();
            
            // If min and max are swapped, swap them back
            if (minRoomSize > maxRoomSize)
            {
                int t = minRoomSize;
                minRoomSize = maxRoomSize;
                maxRoomSize = t;
            }

            #region Create random amount of rooms
            DungeonRoom temp;
            int targetRoomCount = Random.Range(minRoomCount, maxRoomCount + 1);
            for (int r = 0; r < maxRoomPlaceAttempts; r++)
            {
                temp = DungeonRoom.GenerateNewRoom(minRoomSize, maxRoomSize, width, height);
                if (rooms.Count > 0)
                {
                    bool overlap = false;
                    for (int i = 0; i < rooms.Count; i++)
                    {
                        overlap |= DungeonRoom.OverlapCheck(rooms[i], temp);
                    }
                    if (overlap)
                        continue;
                }

                // Mark room tiles as such
                for (int x = temp.left; x <= temp.right; x++)
                    for (int y = temp.bottom; y <= temp.top; y++)
                        tiles[x, y].availability = TileAvailability.Room;

                rooms.Add(temp);
                if (rooms.Count >= targetRoomCount)
                    break;
            }
            #endregion
            
            foreach (DungeonRoom room in rooms)
                yield return StartCoroutine(SetupRoomTiles(room));
            #endregion

            #region Generate corridors using recursive backtracker, making sure to fill any islands
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (tiles[x, y].availability == TileAvailability.Empty)
                    {
                        yield return StartCoroutine(RecursiveCorridorGenerator(x, y));
                    }
                }
            }
            #endregion

            #region Mark all dead ends
            deadEnds = new List<Vector2Int>();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    switch (tiles[x,y].corridors)
                    {
                        case Directions.North:
                        case Directions.East:
                        case Directions.South:
                        case Directions.West:
                            deadEnds.Add(new Vector2Int(x, y));
                            break;
                    }
                }
            }
            #endregion

            #region Use regresion to eliminate some of the dead ends
            #endregion

            #region Make entrances into rooms
            #endregion

            lastDungeonTime = Time.realtimeSinceStartup - startTime;
            timeText.text = string.Format("Dungeon generated in {0:F3} seconds.", lastDungeonTime);

            dungeonCompleteCallback?.Invoke();

            isRunning = false;
        }

        IEnumerator SetupRoomTiles(DungeonRoom room)
        {
            Color roomColour = Color.HSVToRGB(Random.value, 1f, 1f);

            for (int y = room.bottom; y <= room.top; y++)
            {
                for (int x = room.left; x <= room.right; x++)
                {
                    tiles[x, y].renderer.color = roomColour;

                    if (x > room.left) tiles[x, y].corridors |= Directions.West;
                    if (x == room.left && x > 0 && tiles[x - 1, y].availability == TileAvailability.Room) tiles[x, y].corridors |= Directions.West;

                    if (x < room.right) tiles[x, y].corridors |= Directions.East;
                    if (x == room.right && x < this.width-1 && tiles[x + 1, y].availability == TileAvailability.Room) tiles[x, y].corridors |= Directions.East;

                    if (y > room.bottom) tiles[x, y].corridors |= Directions.South;
                    if (y == room.bottom && y > 0 && tiles[x, y - 1].availability == TileAvailability.Room) tiles[x, y].corridors |= Directions.South;

                    if (y < room.top) tiles[x, y].corridors |= Directions.North;
                    if (y == room.top && y < this.height-1 && tiles[x, y + 1].availability == TileAvailability.Room) tiles[x, y].corridors |= Directions.North;

                    if (showWorking)
                        yield return new WaitForSeconds(delayTime);
                }
                if (showWorking)
                    yield return new WaitForSeconds(delayTime);
            }
        }

        IEnumerator RecursiveCorridorGenerator(int x, int y)
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
                    tiles[x + xOffset, y + yOffset][direction.Opposite()] = true;

                    if (showWorking)
                        yield return new WaitForSeconds(delayTime);

                    yield return StartCoroutine(RecursiveCorridorGenerator(x + xOffset, y + yOffset));
                }
            }
        }

        public override string ToString()
        {
            if (tiles == null) return "null";

            StringBuilder sb = new StringBuilder();

            char v;
            for (int y = height - 1; y >= 0; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    switch (tiles[x,y].availability)
                    {
                        default:
                        case TileAvailability.Empty:
                            v = '☐';
                            break;
                        case TileAvailability.Maze:
                            v = mazeChar;
                            break;
                        case TileAvailability.Room:
                            v = roomChar;
                            break;
                    }
                    sb.Append(v);
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
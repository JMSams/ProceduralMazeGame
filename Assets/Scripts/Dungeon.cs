using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
                SetupRoomTiles(room);
            #endregion

            #region Mark main rooms
            List<DungeonRoom> mainRooms = new List<DungeonRoom>();
            Vector2Int meanSize = rooms.MeanSize();
            meanSize.x = Mathf.RoundToInt(meanSize.x * 1.25f);
            meanSize.y = Mathf.RoundToInt(meanSize.y * 1.25f);
            rooms.ForEach((room) =>
            {
                if (room.width >= meanSize.x
                    && room.height >= meanSize.y)
                {
                    mainRooms.Add(room);
                }
            });
            #endregion

            #region Link main rooms

            #endregion

            lastDungeonTime = Time.realtimeSinceStartup - startTime;
            timeText.text = string.Format("Dungeon generated in {0:F3} seconds.", lastDungeonTime);

            dungeonCompleteCallback?.Invoke();
        }

        void SetupRoomTiles(DungeonRoom room)
        {
            Color roomColour = Color.HSVToRGB(Random.value, 1f, 1f);

            for (int x = room.left; x <= room.right; x++)
            {
                for (int y = room.bottom; y <= room.top; y++)
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
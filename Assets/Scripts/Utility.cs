using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FallingSloth.ProceduralMazeGenerator
{
    public static class Utility
    {
        public static List<T> Shuffle<T>(this IList<T> list)
        {
            return list.OrderBy(i => Random.value).ToList();
        }

        public static Directions Opposite(this Directions direction)
        {
            switch (direction)
            {
                case Directions.North:
                    return Directions.South;
                case Directions.South:
                    return Directions.North;
                case Directions.East:
                    return Directions.West;
                case Directions.West:
                    return Directions.East;
                default:
                    return Directions.None;
            }
        }

        public static Vector2Int MeanSize(this List<DungeonRoom> rooms)
        {
            int totalX = 0, totalY = 0;
            rooms.ForEach((room) => { totalX += room.width; totalY += room.height; });
            return new Vector2Int(totalX/rooms.Count, totalY/rooms.Count);
        }
    }

    [System.Flags]
    public enum Directions : byte
    {
        None = 0,
        North = 1,
        East = 2,
        South = 4,
        West = 8
    }
    
    public enum TileAvailability : byte
    {
        Empty,
        Maze,
        Room
    }
}

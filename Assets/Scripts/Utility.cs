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

        public static Directions OppositeDirection(Directions direction)
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
                    throw new System.ArgumentOutOfRangeException();
            }
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

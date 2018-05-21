using UnityEngine;
using System.Collections;
using System.Text;

namespace FallingSloth.ProceduralMazeGenerator
{
    public class DungeonRoom
    {
        /// <summary>
        /// The x co-ordinate of the bottom-left corner of the room.
        /// </summary>
        public int x;

        /// <summary>
        /// The y co-ordinate of the bottom-left corner of the room.
        /// </summary>
        public int y;

        /// <summary>
        /// The width of the room.
        /// </summary>
        public int width;

        /// <summary>
        /// The height of the room.
        /// </summary>
        public int height;

        public int left { get { return x; } }
        public int right { get { return x + width; } }
        public int top { get { return y + height; } }
        public int bottom { get { return y; } }

        protected DungeonRoom(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public static bool OverlapCheck(DungeonRoom d1, DungeonRoom d2)
        {
            return (((d1.x + d1.width) < d2.x) || (d1.x > (d2.x + d2.width)))
                && (((d1.y + d1.height) < d2.y) || (d1.y > (d2.y + d2.height)));
        }

        public static DungeonRoom GenerateNewRoom(int minSize, int maxSize, int gridSizeX, int gridSizeY)
        {
            int sizeX = Random.Range(minSize, maxSize);
            int sizeY = Random.Range(minSize, maxSize);

            return new DungeonRoom(Random.Range(0, gridSizeX - sizeX),
                                   Random.Range(0, gridSizeY - sizeY),
                                   sizeX, sizeY);
        }

        public override string ToString()
        {
            return string.Format("{{{0}, {1}, {2}, {3}}}", x, y, width, height);
        }
    }
}
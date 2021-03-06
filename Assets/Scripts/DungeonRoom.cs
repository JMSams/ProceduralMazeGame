﻿using UnityEngine;
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
        public Vector2 center { get { return new Vector2(x + (width / 2f), y + (height / 2f)); } }

        protected DungeonRoom(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public static bool OverlapCheck(DungeonRoom d1, DungeonRoom d2)
        {
            return (((d1.right >= d2.left) && (d1.left <= d2.right))
                && ((d1.top >= d2.bottom) && (d1.bottom <= d2.top)));
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
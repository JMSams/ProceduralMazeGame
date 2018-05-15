using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FallingSloth.ProceduralMazeGenerator
{
    public class Maze : SingletonBehaviour<Maze>
    {
        [Range(3, 100)]
        public int width = 7, height = 7;

        #region Sprites
        public Sprite NorthSprite;
        public Sprite EastSprite;
        public Sprite SouthSprite;
        public Sprite WestSprite;
        public Sprite NorthSouthSprite;
        public Sprite EastWestSprite;
        public Sprite NorthEastSprite;
        public Sprite EastSouthSprite;
        public Sprite SouthWestSprite;
        public Sprite NorthWestSprite;
        public Sprite NorthEastSouthSprite;
        public Sprite NorthSouthWestSprite;
        public Sprite NorthEastWestSprite;
        public Sprite EastSouthWestSprite;
        public Sprite NorthEastSouthWestSprite;
        #endregion

        public Tile[,] mazeTiles;

        [HideInInspector]
        public List<Vector2Int> deadEnds;

        public void GenerateMaze()
        {
            if (mazeTiles != null)
            {
                for (int x = 0; x < mazeTiles.GetLength(0); x++)
                    for (int y = 0; y < mazeTiles.GetLength(1); y++)
                        Destroy(mazeTiles[x, y].gameObject);
            }
            deadEnds = new List<Vector2Int>();

            Tile[,] maze = new Tile[width, height];

            GameObject temp;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    temp = new GameObject("Tile[" + x + "," + y + "]");
                    temp.AddComponent<SpriteRenderer>();
                    maze[x, y] = temp.AddComponent<Tile>();
                    maze[x, y].transform.parent = this.transform;
                    maze[x, y].x = x;
                    maze[x, y].y = y;
                }
            }

            maze = GenerateMaze(Random.Range(0, width),
                                Random.Range(0, height),
                                maze);

            mazeTiles = maze;
        }
        Tile[,] GenerateMaze(int x, int y, Tile[,] maze)
        {
            maze[x, y].visited = true;

            List<Directions> directions = new List<Directions>();
            if (y > 0)          directions.Add(Directions.South);
            if (y < height - 1) directions.Add(Directions.North);
            if (x > 0)          directions.Add(Directions.West);
            if (x < width - 1)  directions.Add(Directions.East);
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
                if (!maze[x + xOffset, y + yOffset].visited)
                {
                    maze[x, y][direction] = true;
                    maze[x + xOffset, y + yOffset][Utility.OppositeDirection(direction)] = true;
                    maze = GenerateMaze(x + xOffset, y + yOffset, maze);
                }
            }

            byte corridors = 0;
            if (maze[x, y][Directions.North])
                corridors += (byte)Directions.North;
            if (maze[x, y][Directions.East])
                corridors += (byte)Directions.East;
            if (maze[x, y][Directions.South])
                corridors += (byte)Directions.South;
            if (maze[x, y][Directions.West])
                corridors += (byte)Directions.West;

            #region Sprite Selection
            switch (corridors)
            {
                case 1:
                    maze[x, y].renderer.sprite = NorthSprite;
                    deadEnds.Add(new Vector2Int(x, y));
                    break;
                case 2:
                    maze[x, y].renderer.sprite = EastSprite;
                    deadEnds.Add(new Vector2Int(x, y));
                    break;
                case 3:
                    maze[x, y].renderer.sprite = NorthEastSprite;
                    break;
                case 4:
                    maze[x, y].renderer.sprite = SouthSprite;
                    deadEnds.Add(new Vector2Int(x, y));
                    break;
                case 5:
                    maze[x, y].renderer.sprite = NorthSouthSprite;
                    break;
                case 6:
                    maze[x, y].renderer.sprite = EastSouthSprite;
                    break;
                case 7:
                    maze[x, y].renderer.sprite = NorthEastSouthSprite;
                    break;
                case 8:
                    maze[x, y].renderer.sprite = WestSprite;
                    deadEnds.Add(new Vector2Int(x, y));
                    break;
                case 9:
                    maze[x, y].renderer.sprite = NorthWestSprite;
                    break;
                case 10:
                    maze[x, y].renderer.sprite = EastWestSprite;
                    break;
                case 11:
                    maze[x, y].renderer.sprite = NorthEastWestSprite;
                    break;
                case 12:
                    maze[x, y].renderer.sprite = SouthWestSprite;
                    break;
                case 13:
                    maze[x, y].renderer.sprite = NorthSouthSprite;
                    break;
                case 14:
                    maze[x, y].renderer.sprite = EastSouthWestSprite;
                    break;
                case 15:
                    maze[x, y].renderer.sprite = NorthEastSouthWestSprite;
                    break;
            }
            #endregion

            return maze;
        }
    }
}
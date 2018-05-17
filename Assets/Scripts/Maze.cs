using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FallingSloth.ProceduralMazeGenerator
{
    public class Maze : SingletonBehaviour<Maze>
    {
        public bool showWorking = false;

        public bool autoRegenerate = false;

        [Range(3, 200)]
        public int width = 7, height = 7;

        int tilesToGo = -1;

        #region Sprites
        public Sprite BlankSprite;
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

        public Tile this[int x, int y]
        {
            get { return mazeTiles[x, y]; }
            set { mazeTiles[x, y] = value; }
        }

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

            mazeTiles = new Tile[width, height];
            tilesToGo = width * height;

            GameObject temp;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    temp = new GameObject("Tile[" + x + "," + y + "]");
                    temp.AddComponent<SpriteRenderer>();
                    mazeTiles[x, y] = temp.AddComponent<Tile>();
                    mazeTiles[x, y].transform.parent = this.transform;
                    mazeTiles[x, y].x = x;
                    mazeTiles[x, y].y = y;
                    mazeTiles[x, y].renderer.color = Color.red;
                }
            }

            if (showWorking)
                StartCoroutine(GenerateMazeWithDelay(Random.Range(0, width),
                                                     Random.Range(0, height)));
            else
                GenerateMazeImediate(Random.Range(0, width),
                                     Random.Range(0, height));
        }

        void GenerateMazeImediate(int x, int y)
        {
            mazeTiles[x, y].visited = true;

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
                if (!mazeTiles[x + xOffset, y + yOffset].visited)
                {
                    mazeTiles[x, y][direction] = true;
                    mazeTiles[x + xOffset, y + yOffset][Utility.OppositeDirection(direction)] = true;
                    GenerateMazeImediate(x + xOffset, y + yOffset);
                }
            }

            mazeTiles[x, y].renderer.color = Color.white;

            tilesToGo--;
            if (tilesToGo <= 0)
                StartCoroutine(AutoRegenerate());
        }

        IEnumerator GenerateMazeWithDelay(int x, int y)
        {
            yield return new WaitForSeconds(0.025f);

            mazeTiles[x, y].visited = true;

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
                if (!mazeTiles[x + xOffset, y + yOffset].visited)
                {
                    mazeTiles[x, y][direction] = true;
                    mazeTiles[x + xOffset, y + yOffset][Utility.OppositeDirection(direction)] = true;
                    yield return StartCoroutine(GenerateMazeWithDelay(x + xOffset, y + yOffset));
                }
            }

            mazeTiles[x, y].renderer.color = Color.white;

            tilesToGo--;
            if (tilesToGo <= 0)
                StartCoroutine(AutoRegenerate());
            else
                yield return new WaitForSeconds(0.025f);
        }

        IEnumerator AutoRegenerate()
        {
            yield return new WaitForSeconds(5f);

            GenerateMaze();
        }
    }
}
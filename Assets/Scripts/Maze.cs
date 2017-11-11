using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FallingSloth.ProceduralMazeGame
{
    public class Maze : MonoBehaviour
    {
        Tile[,] tiles;

        List<Tile> deadEnds;

        int startX, startY, endX, endY;

        [Range(3, 64)]
        public int gridSizeX, gridSizeY;

        public Tile tilePrefab;

        public List<Sprite> tileSprites;

        bool stepForward = false;
        bool useStepping = true;

        int checkedCount = 0;
        int totalCount;

        void Start()
        {
            GenerateMaze();
        }

        void GenerateMaze()
        {
            tiles = new Tile[gridSizeX, gridSizeY];
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    tiles[x, y] = Instantiate(tilePrefab, this.transform);
                    tiles[x, y].gameObject.name = "Tile (" + x + "," + y + ")";
                    tiles[x, y].x = x;
                    tiles[x, y].y = y;
                }
            }

            startX = Random.Range(0, gridSizeX);
            startY = Random.Range(0, gridSizeY);

            deadEnds = new List<Tile>();
            checkedCount = 0;
            totalCount = gridSizeX * gridSizeY;

            StartCoroutine(GenerateMazeAndAddSprites());
        }

        public void StepForward() { stepForward = true; }
        public void StopStepping() { useStepping = false; }
        public void StopAndRestart()
        {
            StopAllCoroutines();
            
            stepForward = false;
            useStepping = true;
            GenerateMaze();
        }

        IEnumerator RecursiveBacktracker(int x, int y)
        {
            while (useStepping && !stepForward)
            {
                yield return null;
            }
            stepForward = false;

            tiles[x, y].visited = true;

            tiles[x, y].renderer.color = Color.red;

            List<Tile> toVisit = new List<Tile>();

            if (x > 0 && !tiles[x - 1, y].visited)
            {
                tiles[x, y].westCorridor = true;
                tiles[x - 1, y].eastCorridor = true;
                toVisit.Add(tiles[x - 1, y]);
            }
            if (x < gridSizeX - 1 && !tiles[x + 1, y].visited)
            {
                tiles[x, y].eastCorridor = true;
                tiles[x + 1, y].westCorridor = true;
                toVisit.Add(tiles[x + 1, y]);
            }

            if (y > 0 && !tiles[x, y - 1].visited)
            {
                tiles[x, y].southCorridor = true;
                tiles[x, y - 1].northCorridor = true;
                toVisit.Add(tiles[x, y - 1]);
            }
            if (y < gridSizeY - 1 && !tiles[x, y + 1].visited)
            {
                tiles[x, y].northCorridor = true;
                tiles[x, y + 1].southCorridor = true;
                toVisit.Add(tiles[x, y + 1]);
            }

            if (toVisit.Count > 0)
            {
                toVisit.Shuffle();

                foreach (Tile tile in toVisit)
                {
                    if (tiles[tile.x, tile.y].visited)
                        continue;
                    else
                        yield return StartCoroutine(RecursiveBacktracker(tile.x, tile.y));
                }
            }
            else
            {
                deadEnds.Add(tiles[x, y]);
            }

            tiles[x, y].renderer.color = Color.white;
            checkedCount++;
        }

        IEnumerator GenerateMazeAndAddSprites()
        {
            yield return StartCoroutine(RecursiveBacktracker(startX, startY));

            #region Set sprites
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    int s = -1;

                    #region Sprite selection
                    if (tiles[x, y].northCorridor
                        && tiles[x, y].eastCorridor
                        && tiles[x, y].southCorridor
                        && tiles[x, y].westCorridor)
                        s = 14;
                    else if (tiles[x, y].eastCorridor
                        && tiles[x, y].southCorridor
                        && tiles[x, y].westCorridor)
                        s = 13;
                    else if (tiles[x, y].northCorridor
                        && tiles[x, y].southCorridor
                        && tiles[x, y].westCorridor)
                        s = 12;
                    else if (tiles[x, y].northCorridor
                        && tiles[x, y].eastCorridor
                        && tiles[x, y].westCorridor)
                        s = 11;
                    else if (tiles[x, y].northCorridor
                        && tiles[x, y].eastCorridor
                        && tiles[x, y].southCorridor)
                        s = 10;
                    else if (tiles[x, y].southCorridor
                        && tiles[x, y].westCorridor)
                        s = 9;
                    else if (tiles[x, y].eastCorridor
                        && tiles[x, y].westCorridor)
                        s = 8;
                    else if (tiles[x, y].eastCorridor
                        && tiles[x, y].southCorridor)
                        s = 7;
                    else if (tiles[x, y].northCorridor
                        && tiles[x, y].westCorridor)
                        s = 6;
                    else if (tiles[x, y].northCorridor
                        && tiles[x, y].southCorridor)
                        s = 5;
                    else if (tiles[x, y].northCorridor
                        && tiles[x, y].eastCorridor)
                        s = 4;
                    else if (tiles[x, y].westCorridor)
                        s = 3;
                    else if (tiles[x, y].southCorridor)
                        s = 2;
                    else if (tiles[x, y].eastCorridor)
                        s = 1;
                    else if (tiles[x, y].northCorridor)
                        s = 0;
                    
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendLine("Tile (" + x + "," + y + ")");
                    sb.AppendLine("North corridor: " + tiles[x, y].northCorridor);
                    sb.AppendLine("East corridor: " + tiles[x, y].eastCorridor);
                    sb.AppendLine("South corridor: " + tiles[x, y].southCorridor);
                    sb.AppendLine("West corridor: " + tiles[x, y].westCorridor);
                    sb.AppendLine("Sprite index: " + s);
                    Debug.Log(sb);
                    #endregion

                    tiles[x, y].renderer.sprite = tileSprites[s];
                }
            }
            #endregion
        }
    }
}
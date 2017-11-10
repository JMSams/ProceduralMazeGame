using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FallingSloth.ProceduralMazeGame
{
    public class Maze : MonoBehaviour
    {
        Tile[,] cells;
        Vector2 startPos;
        Vector2 endPos;
        List<Vector2> deadEnds;

        public Vector2 gridSize;
        public Tile tilePrefab;

        public List<Sprite> tileSprites;

        void Start()
        {
            GenerateMaze();
        }

        void GenerateMaze()
        {
            cells = new Tile[(int)gridSize.x, (int)gridSize.y];
            deadEnds = new List<Vector2>();
            startPos = new Vector2(Random.Range(0, gridSize.x - 1), Random.Range(0, gridSize.y - 1));

            RecursiveBacktracker((int)startPos.x, (int)startPos.y);

            if (deadEnds.Count == 0)
                throw new System.Exception("Somehow, there are no dead ends!?");
            else
            {
                while (true)
                {
                    int i = Random.Range(0, deadEnds.Count);
                    if (cells[(int)deadEnds[i].x, (int)deadEnds[i].y].isStart)
                    {
                        endPos = deadEnds[Random.Range(0, deadEnds.Count)];
                        cells[(int)endPos.x, (int)endPos.y].isEnd = true;
                        break;
                    }
                }
            }
        }

        IEnumerator RecursiveBacktracker(int x, int y)
        {
            // To make a recursive call:
            // yield return StartCoroutine(RecursiveBacktracker(x, y));
        }
    }
}
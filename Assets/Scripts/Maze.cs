using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace FallingSloth.ProceduralMazeGenerator
{
    public partial class Maze : MonoBehaviour
    {
        Tile[,] tiles;

        List<Tile> deadEnds;

        int startX, startY, endX, endY;

        public Color startColour = Color.red;
        public Color endColour = Color.green;
        public Color normalColour = Color.white;
        public Color inProgressColour = Color.magenta;
        public Color defaultColour = Color.grey;

        [Range(3, 64)]
        public int gridSizeX, gridSizeY;

        public Tile tilePrefab;

        public List<Sprite> tileSprites;

        public float delay = 0.5f;

        bool running = false;

        public Text delayText;

        public Algorithms algorithm = Algorithms.RecursiveBacktracking;

        void Start()
        {
            transform.position = new Vector3(-gridSizeX / 2f + .5f, -gridSizeY / 2f - .4f);
            PickAlgorithm();
        }

        void PickAlgorithm()
        {
            switch (algorithm)
            {
                case Algorithms.RecursiveBacktracking:
                    StartCoroutine(GenerateWithRecursiveBacktracker());
                    break;
                case Algorithms.Eller:
                    StartCoroutine(GenerateWithEller());
                    break;
            }
        }

        void ClearGrid()
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Destroy(tiles[x, y].gameObject);
                }
            }
        }
        
        public void Stop()
        {
            StopAllCoroutines();
            running = false;
        }

        public void Restart()
        {
            if (running) { Stop(); }

            PickAlgorithm();
        }

        public void ChangeDelay(float newDelay)
        {
            delay = newDelay;
            delayText.text = string.Format("Delay: {0:0.00}", newDelay);
        }

        void SetSprite(int x, int y)
        {
            int s = -1;
            
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

            tiles[x, y].renderer.sprite = tileSprites[s];
        }
    }
}
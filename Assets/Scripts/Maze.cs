using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace FallingSloth.ProceduralMazeGame
{
    public class Maze : MonoBehaviour
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

        void Start()
        {
            StartCoroutine(GenerateMaze());
        }

        IEnumerator GenerateMaze()
        {
            running = true;

            if (tiles != null) ClearGrid();

            tiles = new Tile[gridSizeX, gridSizeY];
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    tiles[x, y] = Instantiate(tilePrefab, this.transform);
                    tiles[x, y].gameObject.name = "Tile (" + x + "," + y + ")";
                    tiles[x, y].x = x;
                    tiles[x, y].y = y;
                    tiles[x, y].renderer.color = defaultColour;
                }
            }

            startX = Random.Range(0, gridSizeX);
            startY = Random.Range(0, gridSizeY);
            tiles[startX, startY].isStart = true;

            deadEnds = new List<Tile>();

            yield return StartCoroutine(RecursiveBacktracker(startX, startY));

            running = false;
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

            StartCoroutine(GenerateMaze());
        }

        public void ChangeDelay(float newDelay)
        {
            delay = newDelay;
            delayText.text = string.Format("Delay: {0:0.00%}", newDelay);
        }

        IEnumerator RecursiveBacktracker(int x, int y)
        {
            if (delay > 0f)
                yield return new WaitForSeconds(delay / 2f);

            tiles[x, y].visited = true;

            tiles[x, y].renderer.color = inProgressColour;

            Dictionary<char, Tile> toVisit = new Dictionary<char, Tile>();

            if (x > 0 && !tiles[x - 1, y].visited)
                toVisit.Add('w', tiles[x - 1, y]);

            if (x < gridSizeX - 1 && !tiles[x + 1, y].visited)
                toVisit.Add('e', tiles[x + 1, y]);

            if (y > 0 && !tiles[x, y - 1].visited)
                toVisit.Add('s', tiles[x, y - 1]);
            
            if (y < gridSizeY - 1 && !tiles[x, y + 1].visited)
                toVisit.Add('n', tiles[x, y + 1]);

            if (toVisit.Count > 0)
            {
                toVisit = toVisit.OrderBy(i => Random.value).ToDictionary(item => item.Key, item => item.Value);

                foreach (KeyValuePair<char, Tile> tile in toVisit)
                {
                    if (tiles[tile.Value.x, tile.Value.y].visited) { continue; }
                    else
                    {
                        switch (tile.Key)
                        {
                            case 'n':
                                tiles[x, y].northCorridor = true;
                                tiles[x, y + 1].southCorridor = true;
                                break;
                            case 'e':
                                tiles[x, y].eastCorridor = true;
                                tiles[x + 1, y].westCorridor = true;
                                break;
                            case 's':
                                tiles[x, y].southCorridor = true;
                                tiles[x, y - 1].northCorridor = true;
                                break;
                            case 'w':
                                tiles[x, y].westCorridor = true;
                                tiles[x - 1, y].eastCorridor = true;
                                break;
                            default:
                                throw new System.Exception("Something has gone terribly wrong: toVisit has invalid key.");
                        }
                        yield return StartCoroutine(RecursiveBacktracker(tile.Value.x, tile.Value.y));
                    }
                }
            }
            else
            {
                deadEnds.Add(tiles[x, y]);
            }

            tiles[x, y].renderer.color = (tiles[x,y].isStart) ? startColour : normalColour;
            SetSprite(x, y);

            if (delay > 0f)
                yield return new WaitForSeconds(delay/2f);
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
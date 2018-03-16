using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FallingSloth.ProceduralMazeGenerator
{
	public partial class Maze
    {
        IEnumerator GenerateWithRecursiveBacktracker()
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

        IEnumerator RecursiveBacktracker(int x, int y, float cullPercent = 0f)
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
                        SetSprite(x, y);
                        yield return StartCoroutine(RecursiveBacktracker(tile.Value.x, tile.Value.y));
                    }
                }
            }
            else
            {
                deadEnds.Add(tiles[x, y]);
            }

            tiles[x, y].renderer.color = normalColour;
            SetSprite(x, y);

            if (delay > 0f)
                yield return new WaitForSeconds(delay / 2f);
        }
    }
}

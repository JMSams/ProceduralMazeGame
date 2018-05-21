using UnityEngine;
using System.Collections.Generic;

namespace FallingSloth.ProceduralMazeGenerator
{
    public class Tile : MonoBehaviour
    {
        public Color roomColour = Color.red;
        public Color mazeColour = Color.blue;

        public List<Sprite> sprites;

        [HideInInspector]
        public Tile[,] parentArray;

        [HideInInspector]
        new public SpriteRenderer renderer;

        TileAvailability _availability = TileAvailability.Empty;
        public TileAvailability availability
        {
            get { return _availability; }
            set { _availability = value; SetSprite(); }
        }

        Directions _corridors;
        public Directions corridors
        {
            get { return _corridors; }
            set { _corridors = value; SetSprite(); }
        }

        public bool this[Directions direction]
        {
            get
            {
                return (corridors & direction) == direction;
            }
            set
            {
                if (value)
                    corridors |= direction;
                else
                    corridors = corridors & ~direction;
            }
        }

        [HideInInspector]
        public bool isStart;

        [HideInInspector]
        public bool isEnd;

        private int _x;
        public int x
        {
            get { return _x; }
            set
            {
                _x = value;
                transform.localPosition = new Vector3(value, y);
            }
        }
        
        private int _y;
        public int y
        {
            get { return _y; }
            set
            {
                _y = value;
                transform.localPosition = new Vector3(x, value);
            }
        }

        void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();
        }

        void SetSprite()
        {
            switch (corridors)
            {
                case Directions.None:
                    renderer.sprite = sprites[27];
                    break;
                case Directions.North:
                    renderer.sprite = sprites[19];
                    break;
                case Directions.East:
                    renderer.sprite = sprites[24];
                    break;
                case Directions.North | Directions.East:
                    if (availability == TileAvailability.Maze) renderer.sprite = sprites[16];
                    else if (availability == TileAvailability.Room) renderer.sprite = sprites[20];
                    break;
                case Directions.South:
                    renderer.sprite = sprites[3];
                    break;
                case Directions.North | Directions.South:
                    renderer.sprite = sprites[11];
                    break;
                case Directions.East | Directions.South:
                    if (availability == TileAvailability.Maze) renderer.sprite = sprites[0];
                    else if (availability == TileAvailability.Room) renderer.sprite = sprites[4];
                    break;
                case Directions.North | Directions.East | Directions.South:
                    if (availability == TileAvailability.Maze) renderer.sprite = sprites[8];
                    else if (availability == TileAvailability.Room) renderer.sprite = sprites[12];
                    break;
                case Directions.West:
                    renderer.sprite = sprites[26];
                    break;
                case Directions.North | Directions.West:
                    if (availability == TileAvailability.Maze) renderer.sprite = sprites[18];
                    else if (availability == TileAvailability.Room) renderer.sprite = sprites[22];
                    break;
                case Directions.East | Directions.West:
                    renderer.sprite = sprites[25];
                    break;
                case Directions.North | Directions.East | Directions.West:
                    if (availability == TileAvailability.Maze) renderer.sprite = sprites[17];
                    else if (availability == TileAvailability.Room) renderer.sprite = sprites[21];
                    break;
                case Directions.South | Directions.West:
                    if (availability == TileAvailability.Maze) renderer.sprite = sprites[2];
                    else if (availability == TileAvailability.Room) renderer.sprite = sprites[6];
                    break;
                case Directions.North | Directions.South | Directions.West:
                    if (availability == TileAvailability.Maze) renderer.sprite = sprites[10];
                    else if (availability == TileAvailability.Room) renderer.sprite = sprites[14];
                    break;
                case Directions.East | Directions.South | Directions.West:
                    if (availability == TileAvailability.Maze) renderer.sprite = sprites[1];
                    else if (availability == TileAvailability.Room) renderer.sprite = sprites[5];
                    break;
                case Directions.North | Directions.East | Directions.South | Directions.West:
                    if (availability == TileAvailability.Maze) renderer.sprite = sprites[9];
                    else if (availability == TileAvailability.Room)
                    {
                        if (y > 0 && parentArray[x, y - 1].availability == TileAvailability.Maze)
                            renderer.sprite = sprites[7];
                        else if (y < parentArray.GetLength(1) - 1 && parentArray[x, y + 1].availability == TileAvailability.Maze)
                            renderer.sprite = sprites[15];
                        else if (x > 0 && parentArray[x - 1, y].availability == TileAvailability.Maze)
                            renderer.sprite = sprites[29];
                        else if (x < parentArray.GetLength(0) - 1 && parentArray[x + 1, y].availability == TileAvailability.Maze)
                            renderer.sprite = sprites[28];
                        else
                            renderer.sprite = sprites[13];
                    }
                    break;
            }

            switch (availability)
            {
                case TileAvailability.Empty:
                    renderer.color = Color.white;
                    break;
                case TileAvailability.Maze:
                    renderer.color = mazeColour;
                    break;
                case TileAvailability.Room:
                    renderer.color = roomColour;
                    break;
            }
        }
    }
}
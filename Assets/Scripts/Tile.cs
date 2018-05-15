using UnityEngine;
using System.Collections;

namespace FallingSloth.ProceduralMazeGenerator
{
    public class Tile : MonoBehaviour
    {
        [HideInInspector]
        new public SpriteRenderer renderer;

        [HideInInspector]
        public bool visited;

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
                    renderer.sprite = Maze.Instance.BlankSprite;
                    break;
                case Directions.North:
                    renderer.sprite = Maze.Instance.NorthSprite;
                    break;
                case Directions.East:
                    renderer.sprite = Maze.Instance.EastSprite;
                    break;
                case Directions.North | Directions.East:
                    renderer.sprite = Maze.Instance.NorthEastSprite;
                    break;
                case Directions.South:
                    renderer.sprite = Maze.Instance.SouthSprite;
                    break;
                case Directions.North | Directions.South:
                    renderer.sprite = Maze.Instance.NorthSouthSprite;
                    break;
                case Directions.East | Directions.South:
                    renderer.sprite = Maze.Instance.EastSouthSprite;
                    break;
                case Directions.North | Directions.East | Directions.South:
                    renderer.sprite = Maze.Instance.NorthEastSouthSprite;
                    break;
                case Directions.West:
                    renderer.sprite = Maze.Instance.WestSprite;
                    break;
                case Directions.North | Directions.West:
                    renderer.sprite = Maze.Instance.NorthWestSprite;
                    break;
                case Directions.East | Directions.West:
                    renderer.sprite = Maze.Instance.EastWestSprite;
                    break;
                case Directions.North | Directions.East | Directions.West:
                    renderer.sprite = Maze.Instance.NorthEastWestSprite;
                    break;
                case Directions.South | Directions.West:
                    renderer.sprite = Maze.Instance.SouthWestSprite;
                    break;
                case Directions.North | Directions.South | Directions.West:
                    renderer.sprite = Maze.Instance.NorthSouthWestSprite;
                    break;
                case Directions.East | Directions.South | Directions.West:
                    renderer.sprite = Maze.Instance.EastSouthWestSprite;
                    break;
                case Directions.North | Directions.East | Directions.South | Directions.West:
                    renderer.sprite = Maze.Instance.NorthEastSouthWestSprite;
                    break;
            }
        }
    }
}
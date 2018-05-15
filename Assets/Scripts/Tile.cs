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

        [HideInInspector]
        public bool northCorridor;

        [HideInInspector]
        public bool eastCorridor;

        [HideInInspector]
        public bool southCorridor;

        [HideInInspector]
        public bool westCorridor;

        public bool this[Directions direction]
        {
            get
            {
                switch (direction)
                {
                    case Directions.North:
                        return northCorridor;
                    case Directions.East:
                        return eastCorridor;
                    case Directions.South:
                        return southCorridor;
                    case Directions.West:
                        return westCorridor;
                    default:
                        throw new System.ArgumentOutOfRangeException();
                }
            }
            set
            {
                switch (direction)
                {
                    case Directions.North:
                        northCorridor = value;
                        break;
                    case Directions.East:
                        eastCorridor = value;
                        break;
                    case Directions.South:
                        southCorridor = value;
                        break;
                    case Directions.West:
                        westCorridor = value;
                        break;
                    default:
                        throw new System.ArgumentOutOfRangeException();
                }
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
    }
}
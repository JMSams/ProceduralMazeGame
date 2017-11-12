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
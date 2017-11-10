using UnityEngine;
using System.Collections;

namespace FallingSloth.ProceduralMazeGame
{
    public class Tile : MonoBehaviour
    {
        new SpriteRenderer renderer;

        [HideInInspector]
        public bool northCorridor, eastCorridor, southCorridor, westCorridor, isStart, isEnd;

        void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();
        }
    }
}
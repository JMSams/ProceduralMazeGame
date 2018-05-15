using UnityEngine;
using System.Collections;

namespace FallingSloth.ProceduralMazeGenerator
{
    public class MazeNode
    {
        public int x;
        public int y;

        public MazeNode northConnection;
        public MazeNode eastConnection;
        public MazeNode southConnection;
        public MazeNode westConection;


    }
}
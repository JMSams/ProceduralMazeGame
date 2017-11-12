using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FallingSloth.ProceduralMazeGenerator
{
    public partial class Maze
    {
        IEnumerator GenerateWithEller()
        {
            // TODO: Setup for algorithm

            yield return StartCoroutine(Eller());
        }
        
        IEnumerator Eller()
        {
            throw new System.NotImplementedException();
        }
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace FallingSloth.ProceduralMazeGenerator
{
    public partial class Maze : MonoBehaviour
    {
        void CullDeadEnds()
        {
            switch (cullMode)
            {
                case CullModes.Braid:
                    StartCoroutine(CullByBraid());
                    break;
                case CullModes.Remove:
                    StartCoroutine(CullByRemove());
                    break;
            }
        }

        IEnumerator CullByBraid()
        {
            yield return null;
        }

        IEnumerator CullByRemove()
        {
            yield return null;
        }
    }
}

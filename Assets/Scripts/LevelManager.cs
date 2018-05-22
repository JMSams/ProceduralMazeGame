using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FallingSloth.ProceduralMazeGenerator
{
    public class LevelManager : MonoBehaviour
    {
        public Dungeon dungeon;
        public SceneFader fader;

        void Start()
        {
            fader.FadeOutCompleteDelegate += this.FadeOutComplete;

            dungeon.dungeonCompleteCallback += this.OnDungeonComplete;
            dungeon.GenerateDungeon();
        }

        internal void FadeOutComplete()
        {
            dungeon.GenerateDungeon();
        }

        internal void OnDungeonComplete()
        {
            fader.FadeIn();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FallingSloth.ProceduralMazeGenerator
{
    public class LevelManager : MonoBehaviour
    {
        public Dungeon dungeon;
        public SceneFader fader;

        void Awake()
        {
            dungeon.dungeonCompleteCallback += this.OnDungeonComplete;
            dungeon.GenerateDungeon();
        }

        internal void OnDungeonComplete()
        {
            fader.FadeIn();
        }
    }
}
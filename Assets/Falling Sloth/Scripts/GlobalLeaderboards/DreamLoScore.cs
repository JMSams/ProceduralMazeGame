using UnityEngine;
using System.Collections;

namespace FallingSloth
{
    public class DreamLoScore
    {
        public string name;
        public int score;

        public DreamLoScore(string name, int score)
        {
            this.name = name;
            this.score = score;
        }

        public static DreamLoScore FromString(string scoreData)
        {
            string[] data = scoreData.Split('|');

            return new DreamLoScore(data[0], int.Parse(data[1]));
        }
    }
}
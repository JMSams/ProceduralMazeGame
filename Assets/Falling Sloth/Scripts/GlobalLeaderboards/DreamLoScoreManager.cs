using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FallingSloth
{
    public class DreamLoScoreManager : SingletonBehaviour<DreamLoScoreManager>
    {
        public DreamLoData keyAsset;

        public delegate void AddScoreCallbackDelegate(WebCallResult result);
        public delegate void GetScoresCallbackDelegate(WebCallResult result, List<DreamLoScore> scores);

        public AddScoreCallbackDelegate AddScoreCallback;
        public GetScoresCallbackDelegate GetScoresCallback;

        public void AddScore(string name, int score)
        {
            StartCoroutine(_AddScoreRoutine(name, score));
        }

        IEnumerator _AddScoreRoutine(string name, int score)
        {
            string url = string.Format("http://dreamlo.com/lb/{0}/pipe-get/{1}", keyAsset.publicKey, name);

            WWW www = new WWW(url);

            yield return www;

            if (www.text != "")
            {
                DreamLoScore previousScore = DreamLoScore.FromString(www.text);
                if (previousScore.score >= score)
                {
                    AddScoreCallback(WebCallResult.Error_ScoreNotNew);
                    yield break;
                }
            }

            url = string.Format("http://dreamlo.com/lb/{0}/add/{1}/{2}", keyAsset.privateKey, name, score);

            www = new WWW(url);

            yield return www;

            WebCallResult result = (www.text == "OK") ? WebCallResult.Success : WebCallResult.Error;
            AddScoreCallback(result);
        }

        public void GetScores(int limit = 50)
        {
            StartCoroutine(_GetScoresRoutine(limit));
        }

        IEnumerator _GetScoresRoutine(int limit)
        {
            string url = string.Format("http://dreamlo.com/lb/{0}/pipe/0/{1}", keyAsset.publicKey, limit);

            WWW www = new WWW(url);

            yield return www;

            if (www.text == "")
            {
                GetScoresCallback(WebCallResult.Error_NoScores, new List<DreamLoScore>());
            }
            else
            {
                List<DreamLoScore> scores = new List<DreamLoScore>();

                string[] scoreData = www.text.Split('\n');

                for (int i = 0; i < scoreData.Length; i++)
                {
                    scores.Add(DreamLoScore.FromString(scoreData[i]));
                }

                GetScoresCallback(WebCallResult.Success, scores);
            }
        }

        public enum WebCallResult
        {
            Success,
            Error,
            Error_ScoreNotNew,
            Error_NoScores
        }
    }
}
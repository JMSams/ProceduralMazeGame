using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FallingSloth.ImageEffects;

namespace FallingSloth.ImageEffects
{
    [AddComponentMenu("Falling Sloth/Image Effects/Scene Transition Controller")]
    public class SceneTransitionController : MonoBehaviour
    {
        ImageEffectScript imageEffectScript;

        public float animationSpeed = 1.0f;

        public List<TransitionEffect> transitions;

        void Start()
        {
            imageEffectScript = GetComponent<ImageEffectScript>();
        }

        public void SelectEffect(string name)
        {
            foreach (TransitionEffect effect in transitions)
            {
                if (effect.name == name)
                {
                    imageEffectScript.mat.SetTexture("_TransitionGradient", effect.texture);
                    return;
                }
            }

            throw new System.ArgumentOutOfRangeException();
        }

        public void RunEffect()
        {
            StartCoroutine(_RunEffect());
        }
        IEnumerator _RunEffect()
        {
            float t = 0f;
            while (t < 1f)
            {
                imageEffectScript.mat.SetFloat("_Cutoff", t);

                t += Time.deltaTime * animationSpeed;
                yield return null;
            }
            imageEffectScript.mat.SetFloat("_Cutoff", 1f);
            yield return new WaitForSeconds(1f);
            while (t > 0)
            {
                imageEffectScript.mat.SetFloat("_Cutoff", t);

                t -= Time.deltaTime * animationSpeed;
                yield return null;
            }
            imageEffectScript.mat.SetFloat("_Cutoff", 0f);
        }

        public void SetSpeed(float speed)
        {
            animationSpeed = speed;
        }
    }

    [System.Serializable]
    public struct TransitionEffect
    {
        public string name;
        public Texture2D texture;
    }
}
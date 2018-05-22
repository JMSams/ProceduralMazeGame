using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FallingSloth.ProceduralMazeGenerator
{
    public class SceneFader : MonoBehaviour
    {
        public bool autoFadeIn = true;

        public delegate void FadeCompleteDelegate();
        public FadeCompleteDelegate FadeInCompleteDelegate;
        public FadeCompleteDelegate FadeOutCompleteDelegate;

        Animator animator;
        
        int levelIndex = -1;

        void Awake()
        {
            animator = GetComponent<Animator>();
            if (autoFadeIn)
                animator.SetTrigger("FadeIn");
        }

        public void LoadLevel(int levelIndex)
        {
            this.levelIndex = levelIndex;
            animator.SetTrigger("FadeOut");
        }

        public void FadeIn()
        {
            animator.SetTrigger("FadeIn");
        }

        public void FadeOutComplete()
        {
            FadeOutCompleteDelegate();

            if (levelIndex > -1)
                UnityEngine.SceneManagement.SceneManager.LoadScene(levelIndex);
        }
    }
}
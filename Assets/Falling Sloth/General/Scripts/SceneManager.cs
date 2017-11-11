using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FallingSloth
{
    /// <summary>
    /// Wrapper class that allows UI elements to change scenes.
    /// </summary>
    public class SceneManager : SingletonBehaviour<SceneManager>
    {
        /// <summary>
        /// Loads the scene with the given sceneName.
        /// </summary>
        /// <param name="sceneName">The name of the scene to load.</param>
        public void LoadScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// Loads the scene with the given sceneIndex.
        /// </summary>
        /// <param name="sceneIndex">The index of the scene to load.</param>
        public void LoadScene(int sceneIndex)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
        }

        /// <summary>
        /// Loads the scene with the given sceneName asynchronously.
        /// </summary>
        /// <param name="sceneName">The name of the scene to load.</param>
        public void LoadSceneAsync(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        }

        /// <summary>
        /// Loads the scene with the given sceneIndex asynchronously.
        /// </summary>
        /// <param name="sceneIndex">The index of the scene to load.</param>
        public void LoadSceneAsync(int sceneIndex)
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex);
        }
    }
}
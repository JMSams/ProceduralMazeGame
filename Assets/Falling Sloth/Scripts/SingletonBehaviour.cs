using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FallingSloth
{
    /// <summary>
    /// A MonoBehaviour that follows a singleton pattern.  Children should be of the form Child : SingletonBehaviour&lt;Child>
    /// </summary>
    /// <typeparam name="T">The type of the child of this class.</typeparam>
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        /// <summary>
        /// The static instance of this class.
        /// </summary>
        public static T Instance { get; private set; }

        /// <summary>
        /// Whether this instance should persist across scene loads.
        /// </summary>
        public bool persistent = true;

        /// <summary>
        /// If there is already an instance, then this instance is destroyed.  Otherwise, this instance is assigned to the static instance.
        /// </summary>
        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                DestroyImmediate(gameObject);
                return;
            }
            else
            {
                Instance = this as T;

                if (persistent)
                    DontDestroyOnLoad(gameObject);
            }
        }
    }
}
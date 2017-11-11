using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FallingSloth
{
    /// <summary>
    /// A pool of objects that gets instantiated once and then re-uses those objects.  Optionally grows when an object is requested but there is not one available.
    /// </summary>
    public class GameObjectPool
    {
        /// <summary>
        /// The prefab to use.
        /// </summary>
        GameObject prefab;

        /// <summary>
        /// Whether the pool will grow or not.
        /// </summary>
        bool growWhenEmpty = true;

        /// <summary>
        /// The pool of objects.
        /// </summary>
        List<GameObject> objects;

        /// <summary>
        /// Creates a new pool of objects.
        /// </summary>
        /// <param name="prefab">The prefab of type T to use.</param>
        /// <param name="size">The number of objects to put in the pool.</param>
        /// <param name="growWhenEmpty">Whether or not the pool should grow.  Defaults to true.</param>
        public GameObjectPool(GameObject prefab, int size, bool growWhenEmpty = true)
        {
            this.prefab = prefab;
            this.growWhenEmpty = growWhenEmpty;
            
            objects = new List<GameObject>();
            for (int i = 0; i < size; i++)
            {
                AddNewObject();
            }
        }

        /// <summary>
        /// Adds a new object to the pool.
        /// </summary>
        void AddNewObject()
        {
            GameObject temp = GameObject.Instantiate(prefab);
            temp.SetActive(false);
            objects.Add(temp);
        }

        /// <summary>
        /// Returns an object from the pool.  If there is no object available and <see cref="growWhenEmpty"/> is true, then a new object is added and returned.  If <see cref="growWhenEmpty"/> is false, an exception is thrown.
        /// </summary>
        /// <returns>An object from the pool.  Note that the object with not be active.</returns>
        public GameObject GetObject()
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i].activeSelf)
                {
                    return objects[i];
                }
            }

            if (growWhenEmpty)
            {
                AddNewObject();
                return objects[objects.Count - 1];
            }
            else
            {
                throw new System.Exception("Not enough objects in pool!");
            }
        }
    }
}
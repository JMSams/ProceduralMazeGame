using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FallingSloth
{
    /// <summary>
    /// A pool of objects that gets instantiated once and then re-uses those objects.  Optionally grows when an object is requested but there is not one available.
    /// </summary>
    /// <typeparam name="T">The type of object to pool.  Must inherit from MonoBehaviour.</typeparam>
    public class Pool<T> where T : MonoBehaviour
    {
        /// <summary>
        /// The prefab to use.
        /// </summary>
        T prefab;

        /// <summary>
        /// Whether the pool will grow or not.
        /// </summary>
        bool growWhenEmpty = true;

        /// <summary>
        /// The pool of objects.
        /// </summary>
        List<T> objects;

        /// <summary>
        /// Creates a new pool of objects.
        /// </summary>
        /// <param name="prefab">The prefab of type T to use.</param>
        /// <param name="size">The number of objects to put in the pool.</param>
        /// <param name="growWhenEmpty">Whether or not the pool should grow.  Defaults to true.</param>
        public Pool(T prefab, int size, bool growWhenEmpty = true)
        {
            this.prefab = prefab;
            this.growWhenEmpty = growWhenEmpty;
            
            objects = new List<T>();
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
            T temp = GameObject.Instantiate(prefab);
            temp.gameObject.SetActive(false);
            objects.Add(temp);
        }

        /// <summary>
        /// Returns an object from the pool.  If there is no object available and <see cref="growWhenEmpty"/> is true, then a new object is added and returned.  If <see cref="growWhenEmpty"/> is false, an exception is thrown.
        /// </summary>
        /// <returns>An object from the pool.  Note that the object with not be active.</returns>
        public T GetObject()
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (!objects[i].gameObject.activeSelf)
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

        public T[] GetActiveObjects()
        {
            List<T> activeObjects = new List<T>();

            objects.ForEach(obj => { if (obj.gameObject.activeSelf) { activeObjects.Add(obj); } });

            return activeObjects.ToArray();
        }

        public void DestroyPool()
        {
            objects.ForEach(obj => GameObject.Destroy(obj.gameObject));
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component, IPoolable
{
    private T prefab;
    private List<T> pool = new List<T>();

    public ObjectPool(T prefab, int initialSize)
    {
        this.prefab = prefab;

        // Pre-instantiate objects and add them to the pool
        for (int i = 0; i < initialSize; i++)
        {
            AddObjectToPool();
        }
    }

    public T GetObjectFromPool()
    {
        // Find an inactive object in the pool
        foreach (var obj in pool)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                obj.InitializePoolable(); // Reset the object's state
                return obj;
            }
        }

        // If no inactive objects are found, create a new one (fallback)
        T newObj = AddObjectToPool();
        newObj.InitializePoolable(); // Reset the object's state
        return newObj;
    }

    private T AddObjectToPool()
    {
        T newObj = Object.Instantiate(prefab);
        newObj.gameObject.SetActive(false);
        pool.Add(newObj);
        return newObj;
    }
}
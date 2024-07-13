using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private readonly T prefab;
    private readonly Stack<T> pool;
    private readonly Transform parent;

    public ObjectPool(T prefab, int initialSize)
    {
        this.prefab = prefab;
        pool = new Stack<T>(initialSize);
        parent = new GameObject(prefab.name + " Pool").transform;

        for (int i = 0; i < initialSize; i++)
        {
            T obj = GameObject.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            pool.Push(obj);
        }
    }

    public T Get()
    {
        if (pool.Count > 0)
        {
            T obj = pool.Pop();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            T obj = GameObject.Instantiate(prefab, parent);
            return obj;
        }
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Push(obj);
    }
} 
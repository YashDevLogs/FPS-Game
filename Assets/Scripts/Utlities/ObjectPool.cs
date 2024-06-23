using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private T prefab;
    private Queue<T> objects = new Queue<T>();
    private Transform parentTransform;

    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parentTransform = parent;
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }
    }

    private T CreateNewObject()
    {
        T newObject = GameObject.Instantiate(prefab, parentTransform);
        newObject.gameObject.SetActive(false);
        objects.Enqueue(newObject);
        return newObject;
    }

    public T Get()
    {
        if (objects.Count == 0)
        {
            CreateNewObject();
        }

        T obj = objects.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        objects.Enqueue(obj);
    }
}

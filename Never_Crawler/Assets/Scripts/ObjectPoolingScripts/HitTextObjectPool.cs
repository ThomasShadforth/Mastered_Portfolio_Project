using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTextObjectPool : MonoBehaviour
{
    public int hitTextNum;
    [SerializeField] GameObject _hitTextPrefab;

    public static HitTextObjectPool instance { get; private set; }
    Queue<GameObject> _availableObjects = new Queue<GameObject>();

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        GrowPool();
    }

    void GrowPool()
    {
        for(int i = 0; i < hitTextNum; i++)
        {
            var instanceToAdd = Instantiate(_hitTextPrefab);
            instanceToAdd.transform.SetParent(transform);
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instanceToAdd)
    {
        instanceToAdd.SetActive(false);
        _availableObjects.Enqueue(instanceToAdd);
    }

    public GameObject GetFromPool()
    {
        if(_availableObjects.Count == 0)
        {
            GrowPool();
        }

        var poolInstance = _availableObjects.Dequeue();
        poolInstance.SetActive(true);

        return poolInstance;
    }
}

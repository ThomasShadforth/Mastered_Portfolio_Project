using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpTextObjectPool : MonoBehaviour
{
    public int expTextNum;
    [SerializeField] GameObject _expTextPrefab;

    Queue<GameObject> _availableObjects = new Queue<GameObject>();

    public static ExpTextObjectPool instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    void GrowPool()
    {
        for(int i = 0; i < expTextNum; i++)
        {
            var instanceToAdd = Instantiate(_expTextPrefab);
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

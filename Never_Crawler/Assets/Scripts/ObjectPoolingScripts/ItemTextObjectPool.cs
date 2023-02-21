using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTextObjectPool : MonoBehaviour
{
    public int itemTextNum;
    [SerializeField] GameObject _itemTextPrefab;

    Queue<GameObject> _availableObjects = new Queue<GameObject>();

    public static ItemTextObjectPool instance;

    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
        GrowPool();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GrowPool()
    {
        for(int i = 0; i < itemTextNum; i++)
        {
            var instanceToAdd = Instantiate(_itemTextPrefab);
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

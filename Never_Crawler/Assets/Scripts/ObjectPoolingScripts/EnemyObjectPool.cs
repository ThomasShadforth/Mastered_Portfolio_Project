using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectPool : MonoBehaviour
{
    Queue<GameObject>[] _availableObjects;

    public static EnemyObjectPool instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        int layerCount = FindObjectsOfType<RecursiveDungeon>().Length;

        if(layerCount != 0)
        {
            Debug.Log($"Number of Maze Layers: {layerCount}");
            _availableObjects = new Queue<GameObject>[layerCount];
            for(int i = 0; i < _availableObjects.Length; i++)
            {
                _availableObjects[i] = new Queue<GameObject>();
            }
        }
        else
        {
            _availableObjects = new Queue<GameObject>[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToPool(GameObject instanceToAdd, int layerIndex)
    {
        instanceToAdd.SetActive(false);
        _availableObjects[layerIndex].Enqueue(instanceToAdd);
    }

    public GameObject[] GetArrayFromPool(int layerIndex)
    {
        if (_availableObjects[layerIndex].Count != 0)
        {
            List<GameObject> objects = new List<GameObject>();

            foreach (var objectInstance in _availableObjects[layerIndex])
            {
                objects.Add(objectInstance);
            }

            _availableObjects[layerIndex].Clear();

            Debug.Log(objects.Count);

            return objects.ToArray();
        }

        return null;
    }

    public GameObject GetFromPool(int layerIndex)
    {
        if(_availableObjects[layerIndex].Count == 0)
        {
            return null;
        }

        var poolInstance = _availableObjects[layerIndex].Dequeue();
        poolInstance.SetActive(true);

        return poolInstance;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolSpawner : MonoBehaviour
{
    public GameObject[] objectPoolsToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < objectPoolsToSpawn.Length; i++)
        {
            GameObject objectPool = Instantiate(objectPoolsToSpawn[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

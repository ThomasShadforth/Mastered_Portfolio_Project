using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    public GameObject prefabToPlace;
    public float spawnProbability;
    public MazeGen.PieceType pieceToPlaceOn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnObject(int x, int z, float height, float rot, MazeGen parent)
    {
        float spawnChance = Random.Range(1f, 100f);
        if(spawnChance <= spawnProbability)
        {
            GameObject spawnedObj = Instantiate(prefabToPlace, new Vector3(x, height, z), Quaternion.Euler(0, rot, 0));
            spawnedObj.transform.SetParent(parent.transform);
        }
    }

    public bool CheckSpawnChance()
    {
        float spawnChance = Random.Range(1f, 100f);
        return spawnChance <= spawnProbability;
    }
}

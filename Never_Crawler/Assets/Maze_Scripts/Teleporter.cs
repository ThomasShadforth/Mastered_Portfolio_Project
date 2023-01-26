using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject teleporterPrefab;
    public int startMaze;
    public int endMaze;


    public void Add(MazeGen fromMaze, MazeGen toMaze)
    {
        int fIndex = Random.Range(0, fromMaze.locations.Count);
        int tIndex = Random.Range(0, toMaze.locations.Count);

        Vector3 fromPos = fromMaze.piecePlaces[fromMaze.locations[fIndex].x, fromMaze.locations[fIndex].z].model.transform.position;

        GameObject fromTeleport = Instantiate(teleporterPrefab, fromPos, Quaternion.identity);

        Vector3 toPos = toMaze.piecePlaces[toMaze.locations[tIndex].x, toMaze.locations[tIndex].z].model.transform.position;

        fromTeleport.GetComponent<Teleport>().destination = toPos;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

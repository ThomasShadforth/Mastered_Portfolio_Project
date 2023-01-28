using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMazeManager : MazeManager
{
    public int[] roomNumbers;
    public int[] minRoomSizes;
    public int[] maxRoomSizes;

    public GameObject stairwell;


    // Start is called before the first frame update
    void Start()
    {
        int level = 0;

        foreach (MazeGen maze in mazes)
        {
            maze.xSize = xSize;
            maze.zSize = zSize;

            maze.roomNumber = roomNumbers[level];
            maze.minRoomsize = minRoomSizes[level];
            maze.maxRoomSize = maxRoomSizes[level];

            maze.level = level++;

            maze.Build();

        }

        xSize += 6;
        zSize += 6;

        for (int mazeIndex = 0; mazeIndex < mazes.Length - 1; mazeIndex++)
        {
            if(PlaceStairs(mazeIndex, 0, MazeGen.PieceType.Dead_Left, MazeGen.PieceType.Dead_Right, stairwell)) continue;
            if(PlaceStairs(mazeIndex, 180, MazeGen.PieceType.Dead_Right, MazeGen.PieceType.Dead_Left, stairwell)) continue;
            if(PlaceStairs(mazeIndex, 90, MazeGen.PieceType.Dead_End, MazeGen.PieceType.Dead_Upside_Down, stairwell)) continue;
            PlaceStairs(mazeIndex, -90, MazeGen.PieceType.Dead_Upside_Down, MazeGen.PieceType.Dead_End, stairwell);
        }

        for(int mazeIndex = 0; mazeIndex < mazes.Length - 1; mazeIndex++)
        {
            mazes[mazeIndex + 1].gameObject.transform.Translate(mazes[mazeIndex + 1].xOffset * mazes[mazeIndex + 1].scale, 0, mazes[mazeIndex + 1].zOffset * mazes[mazeIndex + 1].scale);
        }

        Teleporter[] teleporters = GetComponents<Teleporter>();

        if(teleporters.Length != 0)
        {
            foreach(Teleporter tp in teleporters)
            {
                tp.Add(mazes[tp.startMaze], mazes[tp.endMaze]);
            }
        }
    }

    bool PlaceStairs(int mazeIndex, float rot, MazeGen.PieceType bottomType, MazeGen.PieceType topType, GameObject stairsPrefab)
    {
        List<MapLocation> startingLocations = new List<MapLocation>();
        List<MapLocation> endingLocations = new List<MapLocation>();

        for(int i = 0; i < zSize; i++)
        {
            for(int j = 0; j < xSize; j++)
            {
                if(mazes[mazeIndex].piecePlaces[j, i].piece == bottomType)
                {
                    startingLocations.Add(new MapLocation(j, i));
                }

                if(mazes[mazeIndex + 1].piecePlaces[j, i].piece == topType)
                {
                    endingLocations.Add(new MapLocation(j, i));
                }
            }
        }

        if (startingLocations.Count == 0 || endingLocations.Count == 0) return false;

        MapLocation bottomOfStairs = startingLocations[UnityEngine.Random.Range(0, startingLocations.Count)];
        MapLocation topOfStairs = endingLocations[UnityEngine.Random.Range(0, endingLocations.Count)];

        mazes[mazeIndex + 1].xOffset = bottomOfStairs.x - topOfStairs.x + mazes[mazeIndex].xOffset;
        mazes[mazeIndex + 1].zOffset = bottomOfStairs.z - topOfStairs.z + mazes[mazeIndex].zOffset;

        Vector3 bottomPos = new Vector3(bottomOfStairs.x * mazes[mazeIndex].scale, mazes[mazeIndex].scale * mazes[mazeIndex].level * mazes[mazeIndex].heightModifier, bottomOfStairs.z * mazes[mazeIndex].scale);

        mazes[mazeIndex].exitPoint = new MapLocation(bottomOfStairs.x, bottomOfStairs.z);

        Vector3 topPos = new Vector3(topOfStairs.x * mazes[mazeIndex + 1].scale, mazes[mazeIndex + 1].scale * mazes[mazeIndex + 1].level * mazes[mazeIndex + 1].heightModifier, topOfStairs.z * mazes[mazeIndex].scale);

        mazes[mazeIndex + 1].entryPoint = new MapLocation(topOfStairs.x, topOfStairs.z);

        Destroy(mazes[mazeIndex].piecePlaces[bottomOfStairs.x, bottomOfStairs.z].model);
        Destroy(mazes[mazeIndex + 1].piecePlaces[topOfStairs.x, topOfStairs.z].model);

        GameObject stairs = Instantiate(stairwell, bottomPos, Quaternion.identity);
        stairs.transform.Rotate(0, rot, 0);
        mazes[mazeIndex].piecePlaces[bottomOfStairs.x, bottomOfStairs.z].model = stairs;
        mazes[mazeIndex].piecePlaces[bottomOfStairs.x, bottomOfStairs.z].piece = MazeGen.PieceType.Stairwell;
        mazes[mazeIndex].piecePlaces[bottomOfStairs.x, bottomOfStairs.z].model.GetComponent<MapLoc>().x = bottomOfStairs.x;
        mazes[mazeIndex].piecePlaces[bottomOfStairs.x, bottomOfStairs.z].model.GetComponent<MapLoc>().z = bottomOfStairs.z;

        mazes[mazeIndex + 1].piecePlaces[topOfStairs.x, topOfStairs.z].model = null;
        mazes[mazeIndex + 1].piecePlaces[topOfStairs.x, topOfStairs.z].piece = MazeGen.PieceType.Stairwell;
        

        stairs.transform.SetParent(mazes[mazeIndex].gameObject.transform);

        return true;
    }

    IEnumerator GenerateMazeCo(int level)
    {
        yield return null;
        
    }
}

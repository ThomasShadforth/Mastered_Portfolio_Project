using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WilsonsMazeGen : MazeGen
{
    

    List<MapLocation> notUsed = new List<MapLocation>();

    public override void Generate()
    {
        int x = Random.Range(2, xSize - 1);
        int z = Random.Range(2, zSize - 1);

        map[x, z] = 2;

        while (GetAvailableCells() > 1)
        {
            RandomWalk();
        }
    }

    int CountMazeNeighbours(int x, int z)
    {
        int count = 0;

        for(int i = 0; i < directions.Count; i++)
        {
            int nx = x + directions[i].x;
            int nz = z + directions[i].z;

            if (map[nx, nz] == 2) count++;
        }

        return count;
    }

    int GetAvailableCells()
    {
        notUsed.Clear();
        for(int z = 1; z < zSize - 1; z++)
        {
            for(int x = 1; x < xSize - 1; x++)
            {
                if(CountMazeNeighbours(x,z) == 0)
                {
                    notUsed.Add(new MapLocation(x, z));
                }
            }
        }

        return notUsed.Count;
    }

    void RandomWalk()
    {
        List<MapLocation> walkedPath = new List<MapLocation>();

        bool done = false;
        //Checks whether or not a valid path through the maze has been found (Based on whether or not it connects to the initial tile, which has a value of 2
        bool validPath = false;

        //Crawl across the map
        //Current x and z values
        int rndStart = Random.Range(0, notUsed.Count);
        int cx = notUsed[rndStart].x;
        int cz = notUsed[rndStart].z;

        

        walkedPath.Add(new MapLocation(cx, cz));

        int countLoop = 0;

        while (!done && countLoop < 5000 && !validPath)
        {
            map[cx, cz] = 0;

            if (CountMazeNeighbours(cx, cz) > 1)
            {
                break;
            }

            int rndDir = Random.Range(0, directions.Count);

            int nx = cx + directions[rndDir].x;
            int nz = cz + directions[rndDir].z;

            if (CountNeighbours(nx, nz) < 2)
            {
                cx = nx;
                cz = nz;
                walkedPath.Add(new MapLocation(cx, cz));
            }

            validPath = CountMazeNeighbours(cx, cz) == 1;
            countLoop++;
            done |= cx <= 0 || cx >= xSize - 1 || cz <= 0 || cz >= zSize - 1;
        }


        if (validPath)
        {
            map[cx, cz] = 0;
            Debug.Log("PATH FOUND");

            foreach(MapLocation location in walkedPath)
            {
                map[location.x, location.z] = 2;
            }

            walkedPath.Clear();
        }
        else
        {
            foreach(MapLocation location in walkedPath)
            {
                map[location.x, location.z] = 1;
            }

            walkedPath.Clear();
        }
        //As is, this method will result in rooms being created instead of corridors
        //Create methods to check the neighbours for the position, check how many neighbours there are. If there's more than one, do not remove that square.
        //This can be done either at the start of a new loop, or at the end of a current loop (End allows the potential to set a new direction)
    }

    
}

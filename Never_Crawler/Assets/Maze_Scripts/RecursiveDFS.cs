using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursiveDFS : MazeGen
{

    string results;
    public override void Generate()
    {
        //Instead of directly running this version of generate, create and call
        //An overloaded version
        Generate(Random.Range(1, xSize), Random.Range(1, zSize));

        
    }

    void Generate(int x, int z)
    {
        if(CountNeighbours(x, z) >= 2)
        {
            return;
        }

        map[x, z] = 0;

        directions.ShuffleList();

        Generate(x + directions[0].x, z + directions[0].z);
        Generate(x + directions[1].x, z + directions[1].z);
        Generate(x + directions[2].x, z + directions[2].z);
        Generate(x + directions[3].x, z + directions[3].z);
    }

    
}

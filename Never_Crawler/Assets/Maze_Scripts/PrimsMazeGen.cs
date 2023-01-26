using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimsMazeGen : MazeGen
{
    public override void Generate()
    {
        //Choose a random starting point within the maze
        int x = Random.Range(1, xSize - 1);
        int z = Random.Range(1, zSize - 1);

        //Set that wall to 0 to remove it
        map[x, z] = 0;

        //Create a new list of the surrounding walls, add them to the list
        List<MapLocation> walls = new List<MapLocation>();
        walls.Add(new MapLocation(x + 1, z));
        walls.Add(new MapLocation(x - 1, z));
        walls.Add(new MapLocation(x, z + 1));
        walls.Add(new MapLocation(x, z - 1));

        int countLoops = 0;

        while(walls.Count > 0 && countLoops < 5000)
        {
            //Randomly selected one of the walls next to the empty space.
            int rWall = Random.Range(0, walls.Count);
            //Update x and z to that wall's co-ordinates
            x = walls[rWall].x;
            z = walls[rWall].z;

            //Remove this wall from the list (remembers that it has been checked)
            walls.RemoveAt(rWall);

            //If there is only one empty space surrounding this wall (On the first pass there will be only 1)
            if(CountNeighbours(x, z) == 1)
            {
                //Set wall to 0 to remove it
                map[x, z] = 0;
                //Add the new walls to the list (Do not clear, remembers all other walls to carve out the rest of the maze
                walls.Add(new MapLocation(x + 1, z));
                walls.Add(new MapLocation(x - 1, z));
                walls.Add(new MapLocation(x, z + 1));
                walls.Add(new MapLocation(x, z - 1));
            }

            countLoops++;
        }
    }

}

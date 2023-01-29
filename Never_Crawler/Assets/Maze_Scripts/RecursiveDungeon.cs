using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursiveDungeon : MazeGen
{
    public override void Generate()
    {
        this.transform.position = Vector3.zero;
        //Instead of directly running this version of generate, create and call
        //An overloaded version
        Generate(/*Random.Range(1, xSize)*/5 , 5/*Random.Range(1, zSize)*/);


    }

    public override void AddRooms(int roomNumber, int minRoomsize, int maxRoomSize)
    {
        for (int i = 0; i < roomNumber; i++)
        {
            //Debug.Log(gameObject.name + $" room number {i + 1} ");
            int startX = Random.Range(3, xSize - 3);
            int startZ = Random.Range(3, zSize - 3);
            int roomWidth = Random.Range(minRoomsize, maxRoomSize);

            int roomDepth = Random.Range(minRoomsize, maxRoomSize);

            for (int x = startX; x < xSize - 3 && x < startX + roomWidth; x++)
            {
                for (int z = startZ; z < zSize - 3 && z < startZ + roomDepth; z++)
                {
                    map[x, z] = 0;
                }
            }
        }
    }

    public override void SpawnBoss()
    {
        

        bool _spawnFound = false;

        int xIndex;
        int zIndex;

        Vector3 posToSpawn = Vector3.zero;
        string debugName = "";

        do
        {
            xIndex = Random.Range(0, xSize);
            zIndex = Random.Range(0, zSize);

            if(map[xIndex, zIndex] == 0 && ((CountNeighbours(xIndex, zIndex) > 1 && CountDiagNeighbours(xIndex, zIndex) >= 1) || (CountNeighbours(xIndex, zIndex) >= 1 && CountDiagNeighbours(xIndex, zIndex) > 1)))
            {
                if (piecePlaces[xIndex, zIndex].piece == PieceType.Room)
                {
                    posToSpawn = piecePlaces[xIndex, zIndex].model.transform.position;
                    debugName = piecePlaces[xIndex, zIndex].model.name;
                    _spawnFound = true;
                }
            }
        } while (!_spawnFound);

        BossData data = GetComponent<BossData>();

        GameObject bossbattle = Instantiate(data.bossBattle);
        BossBattle bossConfig = bossbattle.GetComponent<BossBattle>();

        if(bossConfig != null)
        {
            

            bossConfig.SetBossPhases(data.phase1Sstate, data.phase2Sstate, data.phase3Sstate);

            GameObject bossEnemy = Instantiate(data.bossEnemy, posToSpawn, Quaternion.identity);
            GameObject bossTrigger = Instantiate(data.bossTrigger, posToSpawn, Quaternion.identity);

            ColliderTrigger trigger = bossTrigger.GetComponent<ColliderTrigger>();
            AIThinker bossThinker = bossEnemy.GetComponent<AIThinker>();

            if(bossThinker != null)
            {
                bossConfig.SetBossAI(bossThinker);
            }

            if(trigger != null)
            {
                bossConfig.SetBossTrigger(trigger);
            }

            bossEnemy.transform.SetParent(transform);
            bossTrigger.transform.SetParent(transform);

        }


        //Debug.Log("Boss will be spawned at position: " + posToSpawn + " on: " + debugName);
    }

    void Generate(int x, int z)
    {
        //If the number of neighbours counted at this given position is at least two, then return
        if (CountNeighbours(x, z) >= 2)
        {
            return;
        }

        //Set the cell at this position in the matrix to 0

        map[x, z] = 0;

        //Take the list of cardinal direction (Up, down, left and right) and shuffle it so that each subsequent direction is randomised
        directions.ShuffleList();

        //Call this method recursively, adding each of the entries in the list to the x and z parameters 
        Generate(x + directions[0].x, z + directions[0].z);
        Generate(x + directions[1].x, z + directions[1].z);
        Generate(x + directions[2].x, z + directions[2].z);
        Generate(x + directions[3].x, z + directions[3].z);

        //This method is designed to be "Recursive", where it keeps calling itself until all instances of it has failed
        //In each instance of this method, it will call itself four times, continuously branching off.
        //The origin of each branched path must finish all of it's method calls down the line before moving on to the next (e.g. To move from the first call of generate to the second one, all of the calls made along the branches belonging to the first method call must fail (return)
        //Then the second call can handle all of it's method calls, etc.
        //This is repeatedly done until the origin (Where generate is first called) handles the four method calls
    }
}

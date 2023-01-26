using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : MazeGen
{
    

    public override void Generate()
    {

        for(int i = 0; i < 3; i++)
        {
            //Debug.Log(i);
            CrawlVertical();
            CrawlHorizontal();
        }
    }

    void CrawlVertical()
    {
        bool done = false;
        int x = Random.Range(1, xSize - 1);
        int z = 1;

        while (!done)
        {
            map[x, z] = 0;
            if (Random.Range(0, 100) < 50)
            {
                x += Random.Range(-1, 2);
            }
            else
            {
                z += Random.Range(0, 2);
            }
            done |= (x < 1 || x >= xSize - 1 || z < 1 || z >= zSize - 1);
        }
    }

    void CrawlHorizontal()
    {
        bool done = false;
        int x = 1;
        int z = Random.Range(1, zSize - 1);

        //Debug.Log(z);

        while (!done)
        {
            //Debug.Log(z);
            map[x, z] = 0;


            if (Random.Range(0, 100) < 50)
            {
                x += Random.Range(0, 2);
            }
            else
            {
                z += Random.Range(-1, 2);
            }
            done |= (x < 1 || x >= xSize - 1 || z < 1 || z >= zSize - 1);
        }

        Debug.Log(done);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    public MazeGen[] mazes;
    

    public int xSize;
    public int zSize;

    void Start()
    {
        int level = 0;

        foreach(MazeGen maze in mazes)
        {
            maze.xSize = xSize;
            maze.zSize = zSize;

            maze.level = level++;

            maze.Build();
        }

        for (int mazeIndex = 0; mazeIndex < mazes.Length - 1; mazeIndex++)
        {
            for (int i = 0; i < zSize; i++)
            {
                for (int j = 0; j < xSize; j++)
                {
                    //Check if the piece above and below in the current position are the same type
                    if (mazes[mazeIndex].piecePlaces[j, i].piece == mazes[mazeIndex + 1].piecePlaces[j, i].piece)
                    {
                        if (mazes[mazeIndex].piecePlaces[j, i].piece == MazeGen.PieceType.Vert_Straight)
                        {
                            Vector3 upManholePos = new Vector3(j * mazes[mazeIndex].scale, mazes[mazeIndex].scale * mazes[mazeIndex].level * mazes[mazeIndex].heightModifier, i * mazes[0].scale);
                            //Instantiate the manhole

                            Vector3 downManholePos = new Vector3(j * mazes[mazeIndex + 1].scale, mazes[mazeIndex + 1].scale * mazes[mazeIndex + 1].level * mazes[mazeIndex].heightModifier, i * mazes[mazeIndex + 1].scale);
                            //Instantiate the manhole

                            //Destroy the original model and replace with the manhole
                            //destroy(mazes[0].piecePlaces[j, i].prefab)
                            //destroy(mazes[1].piecePlaces[j, i].prefab)

                            //mazes[0].piecePlaces[j, i].prefab = Instantiate(.....) for upper
                            //mazes[1].piecePlaces[j, i].prefab = Instantiate(.....) for lower

                            mazes[mazeIndex].piecePlaces[j, i].piece = MazeGen.PieceType.Manhole;
                        }

                        else if (mazes[mazeIndex].piecePlaces[j, i].piece == MazeGen.PieceType.Dead_End)
                        {
                            Vector3 upManholePos = new Vector3(j * mazes[mazeIndex].scale, mazes[mazeIndex].scale * mazes[mazeIndex].level * mazes[mazeIndex].heightModifier, i * mazes[mazeIndex].scale);
                            //Instantiate the manhole

                            Vector3 downManholePos = new Vector3(j * mazes[mazeIndex + 1].scale, mazes[mazeIndex + 1].scale * mazes[mazeIndex + 1].level * mazes[mazeIndex].heightModifier, i * mazes[mazeIndex + 1].scale);
                            //Instantiate the manhole

                            //Destroy the original model and replace with the manhole
                            //destroy(mazes[0].piecePlaces[j, i].prefab)
                            //destroy(mazes[1].piecePlaces[j, i].prefab)

                            //mazes[0].piecePlaces[j, i].prefab = Instantiate(.....) for upper
                            //mazes[1].piecePlaces[j, i].prefab = Instantiate(.....) for lower
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}

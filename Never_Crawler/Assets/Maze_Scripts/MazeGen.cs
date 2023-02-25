using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using UnityEngine.Experimental.AI;

public class MapLocation
{
    public int x;
    public int z;

    

    public MapLocation(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public Vector2 ToVector()
    {
        return new Vector2(x, z);
    }

    public static MapLocation operator +(MapLocation a, MapLocation b)
       => new MapLocation(a.x + b.x, a.z + b.z);

    public override bool Equals(object obj)
    {
        if (obj == null || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            return x == ((MapLocation)obj).x && z == ((MapLocation)obj).z;
        }


    }

    public override int GetHashCode()
    {
        return 0;
    }
}

public class MazeGen : MonoBehaviour
{
    //Store a list of the different directions
    //Will be randomly added to the cx and cz for directions
    public List<MapLocation> directions = new List<MapLocation>()
    {
        new MapLocation(1, 0),
        new MapLocation(-1, 0),
        new MapLocation(0, 1),
        new MapLocation(0, -1)
    };

    public List<MapLocation> pillarLocations = new List<MapLocation>();

    public int mazeLayer { get; private set; }

    public int xSize;
    public int zSize;

    public int roomNumber;
    public int minRoomsize;
    public int maxRoomSize;

    public byte[,] map;

    public int scale = 6;

    public float xOffset = 0;
    public float zOffset = 0;

    public MapLocation entryPoint;
    public MapLocation exitPoint;

    [System.Serializable]
    public struct Module
    {
        public GameObject prefab;
        public Vector3 rotation;
    }

    public Module verStraight;
    public Module horStraight;
    public Module crossroads;
    public Module rightUpCorn;
    public Module rightDownCorn;
    public Module leftUpCorn;
    public Module leftDownCorn;
    public Module tJunct;
    public Module tUpsideDown;
    public Module tRight;
    public Module tLeft;
    public Module deadEnd;
    public Module deadUpsideDown;
    public Module deadRight;
    public Module deadLeft;
    public Module wallTop;
    public Module wallBottom;
    public Module wallLeft;
    public Module wallRight;
    public Module floor;
    public Module ceiling;

    public Module pillar;
    public Module doorTop;
    public Module doorBottom;
    public Module doorLeft;
    public Module doorRight;


    [SerializeField] Transform dungeonParent;
    public GameObject player;
    public GameObject enemyPrefab;
    [SerializeField] NavMeshSurface _navMesh;

    public bool isFirst;
    public int level = 0;
    public float heightModifier = 1.5f;

    bool isInactive = true;

    public enum PieceType
    {
        Hor_Straight,
        Vert_Straight,
        Right_Up_Corn,
        Left_Up_Corn,
        Right_Down_Corn,
        Left_Down_Corn,
        T_Junct,
        TUpsideDown,
        TToLeft,
        TToRight,
        Dead_End,
        Dead_Upside_Down,
        Dead_Right,
        Dead_Left,
        Wall,
        Crossroad,
        Room,
        Manhole,
        Stairwell
    }

    public struct Pieces
    {
        public PieceType piece;
        public GameObject model;

        public Pieces(PieceType pt, GameObject m)
        {
            piece = pt;
            model = m;
        }
    }

    public Pieces[,] piecePlaces;
    public List<MapLocation> locations = new List<MapLocation>();

    public void SetLayerIndex(int index)
    {
        mazeLayer = index;
    }

    // Start is called before the first frame update
    public void Build()
    {
        InitializeMap();
        Generate();
        AddRooms(roomNumber, minRoomsize, maxRoomSize);

        byte[,] oldmap = map;
        int oldXSize = xSize;
        int oldZSize = zSize;

        xSize += 6;
        zSize += 6;

        map = new byte[xSize, zSize];
        InitializeMap();

        for(int i = 0; i < oldZSize; i++)
        {
            for(int j = 0; j < oldXSize; j++)
            {
                map[j + 3, i + 3] = oldmap[j, i];
            }
        }

        int xPos;
        int zPos;

        FindPathAStar aStar = GetComponent<FindPathAStar>();

        if(aStar != null)
        {
            aStar.Build();
            if(aStar.startNode.location.x < aStar.goalNode.location.x)
            {
                //Start node is on the left side
                xPos = aStar.startNode.location.x;
                zPos = aStar.startNode.location.z;

                while(xPos > 1)
                {
                    map[xPos, zPos] = 0;
                    xPos--;
                }

                xPos = aStar.goalNode.location.x;
                zPos = aStar.goalNode.location.z;

                while(xPos < xSize - 2)
                {
                    map[xPos, zPos] = 0;
                    xPos++;
                }
            }
            else
            {
                xPos = aStar.startNode.location.x;
                zPos = aStar.startNode.location.z;

                while(xPos < xSize - 2)
                {
                    map[xPos, zPos] = 0;
                    xPos++;
                }

                xPos = aStar.goalNode.location.x;
                zPos = aStar.goalNode.location.z;

                while (xPos > 1)
                {
                    map[xPos, zPos] = 0;
                    xPos--;
                }

            }
        }
        else
        {
            //Upper vert corridor (From the top of the map trim to the dungeon/maze
            xPos = Random.Range(5, xSize - 5);
            zPos = zSize - 2;

            while(map[xPos, zPos] != 0 && zPos > 1)
            {
                map[xPos, zPos] = 0;
                zPos--;
            }

            //lower vert - bottom of trim to maze/dungeon
            xPos = Random.Range(5, xSize - 5);
            zPos = 1;

            while (map[xPos, zPos] != 0 && zPos < zSize - 2)
            {
                map[xPos, zPos] = 0;
                zPos++;
            }

            //Left Hor
            xPos = 1;
            zPos = Random.Range(5, zSize - 5);

            while (map[xPos, zPos] != 0 && xPos < xSize - 2)
            {
                map[xPos, zPos] = 0;
                xPos++;
            }

            //Right Hor

            xPos = xSize - 2;
            zPos = Random.Range(5, zSize - 5);

            while (map[xPos, zPos] != 0 && xPos > 1)
            {
                map[xPos, zPos] = 0;
                xPos--;
            }

        }

        DrawMap();

        

        if (player != null)
        {
            PlacePlayer();
        }

        for (int i = 0; i < zSize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                GameObject objectModel = piecePlaces[j, i].model;

                if (objectModel != null)
                {
                    piecePlaces[j, i].model.GetComponent<MapLoc>().x = j;
                    piecePlaces[j, i].model.GetComponent<MapLoc>().z = i;
                }

                if (map[j, i] != 1)
                {
                    locations.Add(new MapLocation(j, i));
                }
            }
        }

        StartCoroutine(BuildNavMesh());

        //Handle the placement of enemies (Will test spawning a single enemy for now)
        EnemyPlacement enemyPlace = GetComponent<EnemyPlacement>();

        //If the component exists, begin enemy placement
        if (enemyPlace != null)
        {
            //Checks whether a position has been found
            bool foundPlace = false;
            //Stores the position an enemy will be spawned in
            Vector3 posToPlace = Vector3.zero;
            //Stores a temporary list of the positions where enemies have been spawned (Used in order to prevent enemies being spawned on top of one another
            List<Vector3> enemyPositions = new List<Vector3>();
            //Stores a temporary list of AIThinkers. Will get the scripts attached to the AI that are spawned
            List<AIThinker> enemyAI = new List<AIThinker>();

            //Cycle through how many enemies are spawned via this component
            for (int i = 0; i < enemyPlace.GetEnemyNumber(); i++)
            {

                foundPlace = false;
                //Do while - execute this loop, then check the condition
                do
                {
                    //Get a random x and z index for the generated maze pieces
                    int xIndex = Random.Range(0, xSize);
                    int zIndex = Random.Range(0, zSize);

                    //If a model exists there (isn't initially null or hasn't been deleted due to being replaced by a staircase) and the map has it's value set to 0
                    if(piecePlaces[xIndex, zIndex].model != null && map[xIndex, zIndex] == 0)
                    {
                        //On the initial pass, spawn the enemy, record the position.
                        if (enemyPositions.Count == 0)
                        {
                            foundPlace = true;
                            posToPlace = piecePlaces[xIndex, zIndex].model.transform.position;
                            enemyPositions.Add(posToPlace);
                        }
                        else
                        {
                            //Get the position in which the enemy may be spawned
                            posToPlace = piecePlaces[xIndex, zIndex].model.transform.position;

                            //Initialise a temp bool. Used to flag whether or not the spot is actually empty
                            bool spotEmpty = true;

                            //For each of the positions, check if the position matches any recorded spawn positions
                            foreach(Vector3 position in enemyPositions)
                            {
                                if(position == posToPlace)
                                {
                                    spotEmpty = false;
                                }
                            }

                            //If the spotEmpty variable remains true, then record the position, set foundPlace to true
                            if (spotEmpty)
                            {
                                foundPlace = true;
                                enemyPositions.Add(posToPlace);
                            }
                        }
                    }
                    //Exit the while loop if foundPlace is true
                } while (!foundPlace);

                //Spawn the enemy in the recorded position
                //GameObject enemyToSpawn = Instantiate(enemyPrefab, posToPlace, Quaternion.identity);
                GameObject enemyToSpawn = Instantiate(enemyPlace.GetEnemyToSpawn(), posToPlace, Quaternion.identity);
                enemyToSpawn.transform.SetParent(transform);
                enemyAI.Add(enemyToSpawn.GetComponent<AIThinker>());
            }

            List<GameObject> patrolPositions = new List<GameObject>();
            Vector3 patrolPos = Vector3.zero;

            for (int i = 0; i < enemyPlace.patrolPointNum; i++)
            {
                foundPlace = false;

                do
                {
                    int xIndex = Random.Range(0, xSize);
                    int zIndex = Random.Range(0, zSize);

                    if(piecePlaces[xIndex, zIndex].model != null && map[xIndex, zIndex] == 0)
                    {
                        patrolPos = piecePlaces[xIndex, zIndex].model.transform.position;
                        foundPlace = true;
                    }
                } while (!foundPlace);

                GameObject patrolPoint = Instantiate(enemyPlace.patrolPoint, patrolPos, Quaternion.identity);
                patrolPoint.transform.SetParent(transform);
                patrolPositions.Add(patrolPoint);
                
            }

            if (patrolPositions.Count != 0)
            {
                foreach (AIThinker ai in enemyAI)
                {
                    ai.SetLayerIndex(mazeLayer);
                    ai.patrolPoints = new Transform[2];
                    List<GameObject> patrolPointsTemp = patrolPositions;

                    int firstPatrolIndex = Random.Range(0, patrolPointsTemp.Count);
                    ai.patrolPoints[0] = patrolPointsTemp[firstPatrolIndex].transform;

                    patrolPointsTemp.RemoveAt(firstPatrolIndex);

                    int secondPatrolIndex = Random.Range(0, patrolPointsTemp.Count);
                    ai.patrolPoints[1] = patrolPointsTemp[secondPatrolIndex].transform;
                    ai.active = true;
                }
            }

            

        }

        BossData _bossData = GetComponent<BossData>();

        if (_bossData != null)
        {
            SpawnBoss();
        }

        if (!isFirst)
        {
            
            EnableDisableMeshes(false);
        }
        else
        {
            isInactive = false;
        }

        
    }

    public virtual void SpawnBoss()
    {

    }
    

    public void InitializeMap()
    {
        map = new byte[xSize, zSize];
        piecePlaces = new Pieces[xSize, zSize];

        for (int i = 0; i < zSize; i++)
        {
            for(int j = 0; j < xSize; j++)
            {
                map[j, i] = 1;
            }
        }
    }

    public virtual void Generate()
    {
        for (int i = 0; i < zSize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                if(Random.Range(0, 101) < 50)
                {
                    map[j, i] = 0;
                }
            }
        }
    }

    public virtual void AddRooms(int roomNumber, int minRoomsize, int maxRoomSize)
    {
        
    }

    public virtual void PlacePlayer()
    {
        for(int i = 0; i < zSize; i++)
        {
            for(int j = 0; j < xSize; j++)
            {
                if(map[j, i] == 0)
                {
                    player.transform.position = new Vector3(j * scale, 0, i * scale);
                    return;
                }
            }
        }
    }

    public void EnableDisableMeshes(bool enabled)
    {
        for (int i = 0; i < zSize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                if (piecePlaces[j, i].model != null)
                {
                    Renderer baseObjectRenderer = piecePlaces[j, i].model.GetComponent<Renderer>();

                    if (baseObjectRenderer != null)
                    {
                        piecePlaces[j, i].model.GetComponent<Renderer>().enabled = enabled;
                    }

                    Renderer[] renderers = piecePlaces[j, i].model.gameObject.GetComponentsInChildren<Renderer>();

                    if (renderers.Length != 0)
                    {
                        //Debug.Log(renderers.Length);
                        for (int k = 0; k < renderers.Length; k++)
                        {
                            renderers[k].enabled = enabled;
                        }
                    }
                }


            }
        }
    }

    public void DrawMap()
    {
        float height = level * scale * heightModifier;

        for (int i = 0; i < zSize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                if (map[j, i] == 1)
                {
                    /*
                    Vector3 pos = new Vector3(j * scale, 0, i * scale);
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.localScale *= scale;
                    wall.transform.position = pos;*/

                    piecePlaces[j, i] = new Pieces(PieceType.Wall, null);
                }
                else if (Search2D(j, i, new int[] { 5, 1, 5, 0, 0, 1, 5, 1, 5 }))
                {
                    //end piece -|
                    Vector3 pos = new Vector3(j * scale, height, i * scale);
                    GameObject end = Instantiate(deadRight.prefab);
                    end.transform.position = pos;
                    Vector3 rot = deadRight.rotation;
                    end.transform.eulerAngles = rot;
                    end.transform.SetParent(dungeonParent);
                    piecePlaces[j, i] = new Pieces(PieceType.Dead_Right, end);
                }
                else if (Search2D(j, i, new int[] { 5, 1, 5, 1, 0, 0, 5, 1, 5 }))
                {
                    //end piece |-
                    Vector3 pos = new Vector3(j * scale, height, i * scale);
                    GameObject end = Instantiate(deadLeft.prefab);
                    end.transform.position = pos;
                    Vector3 rot = deadLeft.rotation;
                    //rot.y = -90;
                    end.transform.eulerAngles = rot;
                    end.transform.SetParent(dungeonParent);
                    piecePlaces[j, i] = new Pieces(PieceType.Dead_Left, end);
                }
                else if (Search2D(j, i, new int[] { 5, 1, 5, 1, 0, 1, 5, 0, 5 }))
                {
                    //vert end piece T
                    Vector3 pos = new Vector3(j * scale, height, i * scale);
                    GameObject end = Instantiate(deadEnd.prefab);
                    end.transform.localEulerAngles = deadEnd.rotation;
                    end.transform.position = pos;
                    end.transform.SetParent(dungeonParent);
                    piecePlaces[j, i] = new Pieces(PieceType.Dead_End, end);
                }
                else if (Search2D(j, i, new int[] { 5, 0, 5, 1, 0, 1, 5, 1, 5 }))
                {
                    //vert end piece upside down T
                    Vector3 pos = new Vector3(j * scale, height, i * scale);
                    GameObject end = Instantiate(deadUpsideDown.prefab);
                    end.transform.position = pos;
                    end.transform.SetParent(dungeonParent);
                    Vector3 rot = deadUpsideDown.rotation;
                    
                    end.transform.eulerAngles = rot;
                    piecePlaces[j, i] = new Pieces(PieceType.Dead_Upside_Down, end);
                }
                else if (Search2D(j, i, new int[] { 5, 0, 5, 1, 0, 1, 5, 0, 5 }))
                {
                    //Vert straight piece
                    Vector3 pos = new Vector3(j * scale, height, i * scale);
                    Vector3 rot = verStraight.rotation;
                    GameObject straight = Instantiate(verStraight.prefab, pos, Quaternion.Euler(0, rot.y, 0));
                    straight.transform.SetParent(dungeonParent);
                    piecePlaces[j, i] = new Pieces(PieceType.Vert_Straight, straight);
                }
                else if (Search2D(j, i, new int[] { 5, 1, 5, 0, 0, 0, 5, 1, 5 }))
                {
                    //Horizontal Straight piece
                    Vector3 pos = new Vector3(j * scale, height, i * scale);
                    Vector3 rot = horStraight.rotation;
                    GameObject straight = Instantiate(horStraight.prefab, pos, Quaternion.Euler(0, rot.y, 0));
                    straight.transform.SetParent(dungeonParent);
                    piecePlaces[j, i] = new Pieces(PieceType.Hor_Straight, straight);
                }
                else if (Search2D(j, i, new int[] { 1, 0, 1, 0, 0, 0, 1, 0, 1 }))
                {
                    Vector3 pos = new Vector3(j * scale, height, i * scale);
                    //Instantiate crossroads piece
                    GameObject crossroad = Instantiate(crossroads.prefab, pos, Quaternion.identity);
                    crossroad.transform.SetParent(dungeonParent);
                    piecePlaces[j, i] = new Pieces(PieceType.Crossroad, crossroad);
                }
                //Corners
                else if (Search2D(j, i, new int[] { 5,1,5,0,0,1,1,0,5 }))
                {
                    //upper right
                    Vector3 pos = new Vector3(j * scale, height, i * scale);
                    Vector3 rot = rightUpCorn.rotation;
                    GameObject corner = Instantiate(rightUpCorn.prefab, pos, Quaternion.Euler(0, rot.y,0));
                    corner.transform.SetParent(dungeonParent);
                    piecePlaces[j, i] = new Pieces(PieceType.Right_Up_Corn, corner);

                }
                else if (Search2D(j, i, new int[] { 5, 1, 5, 1, 0, 0, 5, 0, 1 }))
                {
                    //upper left
                    Vector3 pos = new Vector3(j * scale, height, i * scale);
                    Vector3 rot = leftUpCorn.rotation;
                    GameObject corner = Instantiate(leftUpCorn.prefab, pos, Quaternion.Euler(0, rot.y, 0));
                    corner.transform.SetParent(dungeonParent);
                    piecePlaces[j, i] = new Pieces(PieceType.Left_Up_Corn, corner);
                }
                else if (Search2D(j, i, new int[] { 5, 0, 1, 1, 0, 0, 5, 1, 5 }))
                {
                    //lower left
                    Vector3 pos = new Vector3(j * scale, height, i * scale);
                    Vector3 rot = leftDownCorn.rotation;
                    //Formerly 0
                    GameObject corner = Instantiate(leftDownCorn.prefab, pos, Quaternion.Euler(0,rot.y,0));
                    corner.transform.SetParent(dungeonParent);
                    piecePlaces[j, i] = new Pieces(PieceType.Left_Down_Corn, corner);
                }
                else if (Search2D(j, i, new int[] { 1, 0, 5, 5, 0, 1, 5, 1, 5 }))
                {
                    //lower right
                    Vector3 pos = new Vector3(j * scale, height, i * scale);
                    Vector3 rot = rightDownCorn.rotation;
                    GameObject corner = Instantiate(rightDownCorn.prefab, pos, Quaternion.Euler(0, rot.y, 0));
                    corner.transform.SetParent(dungeonParent);
                    piecePlaces[j, i] = new Pieces(PieceType.Right_Down_Corn, corner);
                }
                //TJuncts
                else if (Search2D(j, i, new int[] { 1, 0, 1, 0, 0, 0, 5, 1, 5 }))
                {
                    //Upside Down T-Junction
                    Vector3 pos = new Vector3(j * scale, height, i * scale);
                    Vector3 rot = tUpsideDown.rotation;
                    GameObject tjunction = Instantiate(tUpsideDown.prefab, pos, Quaternion.Euler(0, rot.y, 0));
                    tjunction.transform.SetParent(dungeonParent);
                    piecePlaces[j, i] = new Pieces(PieceType.TUpsideDown, tjunction);
                }
                else if (Search2D(j, i, new int[] { 5, 1, 5, 0, 0, 0, 1, 0, 1 }))
                {
                    //Normal tjunct
                    Vector3 pos = new Vector3(j * scale, height, i * scale);
                    Vector3 rot = tJunct.rotation;
                    GameObject tjunction = Instantiate(tJunct.prefab, pos, Quaternion.Euler(0, rot.y, 0));
                    tjunction.transform.SetParent(dungeonParent);
                    piecePlaces[j, i] = new Pieces(PieceType.T_Junct, tjunction);
                }
                else if (Search2D(j, i, new int[] { 5, 0, 1, 1, 0, 0, 5, 0, 1 }))
                {
                    //TJunct |-
                    Vector3 pos = new Vector3(j * scale, height, i * scale);
                    Vector3 rot = tLeft.rotation;
                    GameObject tjunction = Instantiate(tLeft.prefab, pos, Quaternion.Euler(0, rot.y, 0));
                    tjunction.transform.SetParent(dungeonParent);
                    piecePlaces[j, i] = new Pieces(PieceType.TToLeft, tjunction);
                }
                else if (Search2D(j, i, new int[] { 1, 0, 5, 0, 0, 1, 1, 0, 5 }))
                {
                    //TJunct -|
                    Vector3 pos = new Vector3(j * scale, height, i * scale);
                    Vector3 rot = tRight.rotation;
                    GameObject tjunction = Instantiate(tRight.prefab, pos, Quaternion.Euler(0, rot.y, 0));
                    tjunction.transform.SetParent(dungeonParent);
                    piecePlaces[j, i] = new Pieces(PieceType.TToRight, tjunction);
                } 
                else if(map[j, i] == 0 && ((CountNeighbours(j, i) > 1 && CountDiagNeighbours(j, i) >= 1)) ||
                    (CountNeighbours(j, i) >= 1 && CountDiagNeighbours(j, i) > 1)){

                    GameObject floor = Instantiate(this.floor.prefab);
                    floor.transform.position = new Vector3(j * scale, height, i * scale);
                    floor.transform.SetParent(dungeonParent);
                    GameObject ceiling = Instantiate(this.ceiling.prefab);
                    ceiling.transform.position = new Vector3(j * scale, height, i * scale);
                    ceiling.transform.SetParent(dungeonParent);

                    piecePlaces[j, i] = new Pieces(PieceType.Room, floor);


                    GameObject pillarCorner;

                    LocateWalls(j, i);

                    if (top)
                    {
                        GameObject wall = Instantiate(wallTop.prefab);
                        Vector3 rot = wallTop.rotation;
                        wall.transform.eulerAngles = rot;
                        wall.transform.position = new Vector3(j * scale, height, i * scale);
                        wall.transform.SetParent(dungeonParent);

                        if (map[j + 1, i] == 0 && map[j + 1, i + 1] == 0 && !pillarLocations.Contains(new MapLocation(j, i)))
                        {
                            pillarCorner = Instantiate(pillar.prefab);
                            pillarCorner.transform.position = new Vector3(j * scale, height, i * scale);
                            pillarCorner.name = "Top Right";
                            pillarLocations.Add(new MapLocation(j, i));
                            pillarCorner.transform.localScale = new Vector3(1.01f, 1, 1.01f);
                            pillarCorner.transform.SetParent(dungeonParent);
                        }

                        if (map[j - 1, i] == 0 && map[j - 1, i + 1] == 0 && !pillarLocations.Contains(new MapLocation(j - 1, i)))
                        {
                            pillarCorner = Instantiate(pillar.prefab);
                            pillarCorner.transform.position = new Vector3((j - 1) * scale, height, i * scale);
                            pillarCorner.name = "Top Left";
                            pillarLocations.Add(new MapLocation(j - 1, i));
                            pillarCorner.transform.localScale = new Vector3(1.01f, 1, 1.01f);
                            pillarCorner.transform.SetParent(dungeonParent);
                        }


                    }

                    if (bottom)
                    {
                        GameObject wall = Instantiate(wallBottom.prefab, new Vector3(j * scale, height, i * scale), Quaternion.Euler(0, wallBottom.rotation.y, 0));
                        wall.transform.SetParent(dungeonParent);


                        if (map[j + 1, i] == 0 && map[j + 1, i - 1] == 0 && !pillarLocations.Contains(new MapLocation(j, i - 1)))
                        {
                            pillarCorner = Instantiate(pillar.prefab);
                            pillarCorner.transform.position = new Vector3(j * scale, height, (i - 1) * scale);
                            pillarCorner.name = "Bottom Right";
                            pillarLocations.Add(new MapLocation(j, i - 1));
                            pillarCorner.transform.localScale = new Vector3(1.01f, 1, 1.01f);
                            pillarCorner.transform.SetParent(dungeonParent);
                        }

                        if (map[j - 1, i - 1] == 0 && map[j - 1, i] == 0 && !pillarLocations.Contains(new MapLocation(j - 1, i - 1)))
                        {
                            pillarCorner = Instantiate(pillar.prefab);
                            pillarCorner.transform.position = new Vector3((j - 1) * scale, height, (i - 1)* scale);
                            pillarCorner.name = "Bottom Left";
                            pillarLocations.Add(new MapLocation(j - 1, i - 1));
                            pillarCorner.transform.localScale = new Vector3(1.01f, 1, 1.01f);
                            pillarCorner.transform.SetParent(dungeonParent);
                        }

                    }

                    if (left)
                    {
                        GameObject wall = Instantiate(wallLeft.prefab, new Vector3(j * scale, height, i * scale), Quaternion.Euler(0, wallLeft.rotation.y, 0));
                        wall.transform.SetParent(dungeonParent);

                        if (map[j - 1, i + 1] == 0 && map[j, i + 1] == 0 && !pillarLocations.Contains(new MapLocation(j - 1, i)))
                        {
                            pillarCorner = Instantiate(pillar.prefab);
                            pillarCorner.transform.position = new Vector3((j - 1) * scale, height, i * scale);
                            pillarCorner.name = "Left Top";
                            pillarLocations.Add(new MapLocation(j - 1, i));
                            pillarCorner.transform.localScale = new Vector3(1.01f, 1, 1.01f);
                            pillarCorner.transform.SetParent(dungeonParent);
                        }

                        if (map[j - 1, i - 1] == 0 && map[j, i - 1] == 0 && !pillarLocations.Contains(new MapLocation(j - 1, i - 1)))
                        {
                            pillarCorner = Instantiate(pillar.prefab);
                            pillarCorner.transform.position = new Vector3((j - 1) * scale, height, (i - 1) * scale);
                            pillarCorner.name = "Left Bottom";
                            pillarLocations.Add(new MapLocation(j - 1, i - 1));
                            pillarCorner.transform.localScale = new Vector3(1.01f, 1, 1.01f);
                            pillarCorner.transform.SetParent(dungeonParent);
                        }

                    }

                    if (right)
                    {
                        GameObject wall = Instantiate(wallRight.prefab, new Vector3(j * scale, height, i * scale), Quaternion.Euler(0, wallRight.rotation.y, 0));
                        wall.transform.SetParent(dungeonParent);

                        if (map[j + 1, i + 1] == 0 && map[j, i + 1] == 0 && !pillarLocations.Contains(new MapLocation(j , i)))
                        {
                            pillarCorner = Instantiate(pillar.prefab);
                            pillarCorner.transform.position = new Vector3(j * scale, height, i * scale);
                            pillarCorner.name = "Right Top";
                            pillarLocations.Add(new MapLocation(j, i));
                            pillarCorner.transform.localScale = new Vector3(1.01f, 1, 1.01f);
                            pillarCorner.transform.SetParent(dungeonParent);
                        }

                        if (map[j + 1, i - 1] == 0 && map[j, i - 1] == 0 && !pillarLocations.Contains(new MapLocation(j, i - 1)))
                        {
                            pillarCorner = Instantiate(pillar.prefab);
                            pillarCorner.transform.position = new Vector3(j * scale, height, (i - 1) * scale);
                            pillarCorner.name = "Right Bottom";
                            pillarLocations.Add(new MapLocation(j, i - 1));
                            pillarCorner.transform.localScale = new Vector3(1.01f, 1, 1.01f);
                            pillarCorner.transform.SetParent(dungeonParent);
                        }
                    }

                    
                }
                else
                {
                    
                    Vector3 pos = new Vector3(j * scale, 0, i * scale);
                    /*
                    GameObject roomBlock = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    roomBlock.transform.localScale *= scale;
                    roomBlock.transform.position = pos;*/
                    //GameObject roomFloor = Instantiate(floorPiece, pos, Quaternion.identity);
                }


            }
        }


        for(int i = 0; i < zSize; i++)
        {
            for(int j = 0; j < xSize; j++)
            {
                if (piecePlaces[j, i].piece != PieceType.Room) continue;

                GameObject doorway;
                LocateDoors(j, i);

                if (top)
                {
                    Vector3 rot = doorTop.rotation;

                    doorway = Instantiate(doorTop.prefab, new Vector3(j * scale, height, i * scale), Quaternion.Euler(rot));
                    doorway.name = "Top Exit";
                    doorway.transform.Translate(0, 0, 0.01f);
                    doorway.transform.SetParent(dungeonParent);
                }
                if (bottom)
                {
                    Vector3 rot = doorBottom.rotation;

                    doorway = Instantiate(doorBottom.prefab, new Vector3(j * scale, height, i * scale), Quaternion.Euler(rot));
                    doorway.name = "Bottom Exit";
                    doorway.transform.Translate(0, 0, 0.01f);
                    doorway.transform.SetParent(dungeonParent);
                }
                if (left)
                {
                    Vector3 rot = doorLeft.rotation;

                    doorway = Instantiate(doorLeft.prefab, new Vector3(j * scale, height, i * scale), Quaternion.Euler(rot));
                    doorway.name = "Left Exit";
                    doorway.transform.Translate(0, 0, 0.01f);
                    doorway.transform.SetParent(dungeonParent);
                }
                if (right)
                {
                    Vector3 rot = doorRight.rotation;

                    doorway = Instantiate(doorRight.prefab, new Vector3(j * scale, height, i * scale), Quaternion.Euler(rot));
                    doorway.name = "Right Exit";
                    doorway.transform.Translate(0, 0, 0.01f);
                    doorway.transform.SetParent(dungeonParent);
                }
            }
        }

        

        ObjectPlacement[] objPlacers = GetComponents<ObjectPlacement>();

        if(objPlacers.Length != 0)
        {
            foreach(ObjectPlacement objPlace in objPlacers)
            {
                for (int i = 0; i < zSize; i++)
                {
                    for (int j = 0; j < xSize; j++)
                    {
                        if (piecePlaces[j, i].piece != objPlace.pieceToPlaceOn) continue;

                        GameObject spawnedObject;
                        if (objPlace.CheckSpawnChance())
                        {
                            spawnedObject = Instantiate(objPlace.prefabToPlace, new Vector3(j * scale, height, i * scale), Quaternion.Euler(0, piecePlaces[j, i].model.transform.rotation.y, 0));
                            spawnedObject.transform.SetParent(dungeonParent);
                        }
                    }
                }
            }
        }

        

    }

    //Bools specifically for checking for walls
    bool top;
    bool bottom;
    bool left;
    bool right;

    public void LocateWalls(int x, int z)
    {
        top = false;
        bottom = false;
        left = false;
        right = false;

        //Check for being on the edge of the maze
        if (x <= 0 || x >= xSize - 1 || z <= 0 || z >= zSize - 1) return;

        //Checks if the spaces adjacent to a floor is empty. If it is, then set the directional check to true
        if (map[x, z + 1] == 1) top = true;
        if (map[x, z - 1] == 1) bottom = true;
        if (map[x + 1, z] == 1) right = true;
        if (map[x - 1, z] == 1) left = true;
    }

    public void LocateDoors(int x, int z)
    {
        top = false;
        bottom = false;
        left = false;
        right = false;

        //Check for being on the edge of the maze
        if (x <= 0 || x >= xSize - 1 || z <= 0 || z >= zSize - 1) return;

        //Checks if the spaces adjacent to a floor is empty. If it is, then set the directional check to true
        if (piecePlaces[x, z + 1].piece != PieceType.Room && piecePlaces[x, z + 1].piece != PieceType.Wall) top = true;
        if (piecePlaces[x, z - 1].piece != PieceType.Room && piecePlaces[x, z - 1].piece != PieceType.Wall) bottom = true;
        if (piecePlaces[x + 1, z].piece != PieceType.Room && piecePlaces[x + 1, z].piece != PieceType.Wall) right = true;
        if (piecePlaces[x - 1, z].piece != PieceType.Room && piecePlaces[x - 1, z].piece != PieceType.Wall) left = true;
    }

    bool Search2D(int c, int r, int[] pattern)
    {
        //Counts number of matches
        int count = 0;

        int pos = 0;

        for(int z = 1; z > -2; z--)
        {
            for(int x = -1; x < 2; x++)
            {
                if (pattern[pos] == map[c + x, r + z] || pattern[pos] == 5)
                {
                    count++;
                    pos++;
                }
            }
        }

        return count == 9;
    }

    public int CountNeighbours(int x, int z)
    {
        int count = 0;

        if(x <= 0 || x >= xSize - 1 || z <= 0 || z >= zSize - 1)
        {
            return 5;
        }
        if(map[x - 1, z] == 0)
        {
            count++;
        }

        if(map[x, z + 1] == 0)
        {
            count++;
        }

        if(map[x, z - 1] == 0)
        {
            count++;
        }

        if(map[x + 1, z] == 0)
        {
            count++;
        }

        return count;
    }

    public int CountDiagNeighbours(int x, int z)
    {
        int count = 0;

        if (x <= 0 || x >= xSize - 1 || z <= 0 || z >= zSize - 1) return 5;

        if (map[x - 1, z + 1] == 0) count++;
        if (map[x - 1, z - 1] == 0) count++;
        if (map[x + 1, z + 1] == 0) count++;
        if (map[x + 1, z - 1] == 0) count++;

        return count;
    }

    public int CountAllNeighbours(int x, int z)
    {
        return CountNeighbours(x, z) + CountDiagNeighbours(x, z);
    }

    //Experimental - Testing the implementation of nav-meshes into the maze environment

    IEnumerator BuildNavMesh()
    {
        yield return null;

        if(_navMesh != null)
        {
            
            _navMesh.BuildNavMesh();
        }

    }

    //Experimental - testing the idea of disabling/enabling mesh renderers depending on if the player is within the range of the maze layers
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            if (isInactive)
            {
                isInactive = false;
                EnableDisableMeshes(true);
                if (EnemyObjectPool.instance != null)
                {
                    GameObject[] aiToRespawn = EnemyObjectPool.instance.GetArrayFromPool(mazeLayer);


                    if (aiToRespawn != null)
                    {
                        foreach (var AI in aiToRespawn)
                        {
                            AI.SetActive(true);
                            AI.GetComponent<AIThinker>().healthSystem.Heal(AI.GetComponent<AIThinker>().enemyMaxHealth);
                        }
                    }

                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isInactive)
            {

                EnableDisableMeshes(false);
            }   
        }
        
    }
}

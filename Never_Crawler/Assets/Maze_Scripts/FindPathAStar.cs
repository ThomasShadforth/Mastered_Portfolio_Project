using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class PathMarker
{
    public MapLocation location;
    public float G;
    public float H;
    public float F;
    //public GameObject marker;
    public PathMarker parent;

    public PathMarker(MapLocation l, float g, float h, float f, PathMarker p)
    {
        location = l;
        G = g;
        H = h;
        F = f;
        
        parent = p;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            //Check whether or not the location of a pathmarker is the same as another
            return location.Equals(((PathMarker)obj).location);
        }
    }

    public override int GetHashCode()
    {
        return 0;
    }
}

public class FindPathAStar : MonoBehaviour
{
    public MazeGen maze;
    

    List<PathMarker> openList = new List<PathMarker>();
    List<PathMarker> closedList = new List<PathMarker>();

    public PathMarker goalNode;
     public PathMarker startNode;

    PathMarker lastPos;
    bool done;

    void BeginSearch()
    {
        done = false;

        List<MapLocation> locations = new List<MapLocation>();
        for (int i = 0; i < maze.zSize; i++)
        {
            for (int j = 0; j < maze.xSize; j++)
            {
                if (maze.map[j, i] != 1)
                {
                    locations.Add(new MapLocation(j, i));
                }
            }
        }

        locations.ShuffleList();

        Vector3 startLocation = new Vector3(locations[0].x * maze.scale, 0, locations[0].z * maze.scale);
        startNode = new PathMarker(new MapLocation(locations[0].x, locations[0].z), 0, 0, 0, null);

        Vector3 goalLocation = new Vector3(locations[1].x * maze.scale, 0, locations[1].z * maze.scale);
        goalNode = new PathMarker(new MapLocation(locations[1].x, locations[1].z), 0, 0, 0, null);

        openList.Clear();
        closedList.Clear();

        openList.Add(startNode);
        lastPos = startNode;
    }

    void BeginSearch(PathMarker start, PathMarker end)
    {
        done = false;

        maze.locations.ShuffleList();

        startNode = start;
        goalNode = end;

        
        openList.Clear();
        closedList.Clear();

        openList.Add(startNode);
        lastPos = startNode;
    }

    void Search(PathMarker thisNode)
    {
        if (thisNode.Equals(goalNode))
        {
            done = true;
            return;
        }

        foreach (MapLocation dir in maze.directions)
        {
            MapLocation neighbour = dir + thisNode.location;

            if (maze.map[neighbour.x, neighbour.z] == 1)
            {
                continue;
            }

            if (neighbour.x < 1 || neighbour.x >= maze.xSize || neighbour.z < 1 || neighbour.z >= maze.zSize) continue;

            if (IsClosed(neighbour)) continue;

            float g = Vector2.Distance(thisNode.location.ToVector(), neighbour.ToVector()) + thisNode.G;
            float h = Vector2.Distance(neighbour.ToVector(), goalNode.location.ToVector());
            float f = g + h;

           

            if (!UpdateMarker(neighbour, g, h, f, thisNode))
            {
                openList.Add(new PathMarker(neighbour, g, h, f, thisNode));
            }

        }

        openList = openList.OrderBy(p => p.F).ToList<PathMarker>();
        PathMarker pm = (PathMarker)openList.ElementAt(0);
        closedList.Add(pm);

        openList.RemoveAt(0);
        

        lastPos = pm;
    }

    bool UpdateMarker(MapLocation pos, float g, float h, float f, PathMarker prt)
    {
        foreach (PathMarker p in openList)
        {
            if (p.location.Equals(pos))
            {
                p.G = g;
                p.H = h;
                p.F = f;
                p.parent = prt;
                return true;
            }
        }

        return false;
    }

    bool IsClosed(MapLocation marker)
    {
        foreach (PathMarker p in closedList)
        {
            if (p.location.Equals(marker)) return true;
        }

        return false;
    }

    // Start is called before the first frame update
    public void Build()
    {
        BeginSearch();
        while (!done)
        {
            Search(lastPos);
        }

        maze.InitializeMap();

        MarkPath();

        //maze.DrawMap();
    }

    public PathMarker Build(MapLocation start, MapLocation end, MazeGen currentMaze)
    {
        maze = currentMaze;

        BeginSearch(new PathMarker(start, 0, 0, 0, null), new PathMarker(end, 0, 0, 0, null));
        while (!done)
        {
            Search(lastPos);
        }

        return lastPos;
    }

    void MarkPath()
    {
        PathMarker begin = lastPos;

        while (!startNode.Equals(begin) && begin != null)
        {
            maze.map[begin.location.x, begin.location.z] = 0;
            begin = begin.parent;
        }

        maze.map[startNode.location.x, startNode.location.z] = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}


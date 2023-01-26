using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FindPathAStar))]
public class FindPath : MonoBehaviour
{
    public GameObject particles;
    MazeGen thisMaze;
    FindPathAStar pathAStar;
    GameObject magic;
    PathMarker destination;

    // Start is called before the first frame update
    void Start()
    {
        pathAStar = GetComponent<FindPathAStar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(pathAStar != null)
            {
                RaycastHit hit;
                Ray ray = new Ray(this.transform.position, Vector3.down);
                if(Physics.Raycast(ray, out hit))
                {
                    thisMaze = hit.collider.gameObject.GetComponentInParent<MazeGen>();
                    MapLoc thisLocation = hit.collider.gameObject.GetComponentInParent<MapLoc>();

                    MapLocation currentLocation = new MapLocation(thisLocation.x, thisLocation.z);
                    MapLocation exitPoint = thisMaze.exitPoint;

                    destination = pathAStar.Build(currentLocation, exitPoint, thisMaze);

                    magic = Instantiate(particles, this.gameObject.transform.position, this.gameObject.transform.rotation);

                    StartCoroutine(DisplayPathGuide());
                }
            }
        }
        
    }

    IEnumerator DisplayPathGuide()
    {
        List<MapLocation> path = new List<MapLocation>();
        while(destination != null)
        {
            path.Add(new MapLocation(destination.location.x, destination.location.z));
            destination = destination.parent;
        }

        path.Reverse();

        foreach (MapLocation loc in path)
        {
            magic.transform.LookAt(thisMaze.piecePlaces[loc.x, loc.z].model.transform.position + new Vector3(0, 1, 0));

            int loopTimeout = 0;
            while (Vector2.Distance(new Vector2(magic.transform.position.x, magic.transform.position.z), new Vector2(thisMaze.piecePlaces[loc.x, loc.z].model.transform.position.x, thisMaze.piecePlaces[loc.x, loc.z].model.transform.position.z)) > 2 && loopTimeout < 100)
            {
                magic.transform.Translate(0, 0, 10f * Time.deltaTime);
                yield return new WaitForSeconds(.01f);
                loopTimeout++;
            }
        }
        Destroy(magic, 10);
    }
}

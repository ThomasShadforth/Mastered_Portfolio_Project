using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Distance Check Decision", menuName = "Scriptable Objects/Pluggable AI/Decisions/Distance Check Decision")]
public class PatrolDistDecision : Decision
{
    public override bool Decide(AIThinker thinker)
    {
        bool isAtPoint = CheckDistanceFromPoint(thinker);

        return isAtPoint;
    }

    bool CheckDistanceFromPoint(AIThinker thinker)
    {
        if(Vector3.Distance(thinker.transform.position, thinker.patrolPoints[thinker.currentPatrolIndex].position) <= thinker.minDistFromPoint)
        {
            thinker.currentPatrolIndex++;

            if(thinker.currentPatrolIndex >= thinker.patrolPoints.Length)
            {
                thinker.currentPatrolIndex = 0;
            }

            thinker._rb.velocity = Vector3.zero;
            return true;
        }
        else
        {
            return false;
        }
    }
}

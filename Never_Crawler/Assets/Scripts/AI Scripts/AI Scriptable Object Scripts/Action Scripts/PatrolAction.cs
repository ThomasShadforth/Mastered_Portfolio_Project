using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Patrol Action", menuName = "Scriptable Objects/Pluggable AI/Actions/Patrol Action")]
public class PatrolAction : Action
{
    public override void Act(AIThinker thinker)
    {
        MoveToPatrolPos(thinker);
    }

    void MoveToPatrolPos(AIThinker thinker)
    {
        Vector3 direction = GetPatrolDirection(thinker);
        

        thinker._rb.velocity = new Vector3(direction.x * 4, thinker._rb.velocity.y, direction.z * 4);
        thinker.transform.rotation = Quaternion.Euler(0, GetPatrolAngle(thinker), 0);
    }

    public float GetPatrolAngle(AIThinker thinker)
    {
        Vector3 direction = thinker.patrolPoints[thinker.currentPatrolIndex].position - thinker.transform.position;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(thinker.transform.eulerAngles.y, targetAngle, ref thinker.currSmoothVelocity, thinker.rotationSmooth);

        return angle;
    }


    public Vector3 GetPatrolDirection(AIThinker thinker)
    {
        Vector3 direction = thinker.patrolPoints[thinker.currentPatrolIndex].position - thinker.transform.position;

        //Debug.Log(direction.x);

        if (direction.x > 0.1)
        {
            direction.x = 1f;
        }
        else if(direction.x < -0.1)
        {
            direction.x = -1f;
        }
        else
        {
            direction.x = 0;
        }

        if(direction.z > 0)
        {
            direction.z = 1f;
        } else if(direction.z < 0)
        {
            direction.z = -1f;
        }

        direction = direction.normalized;

        return direction;
    }
}

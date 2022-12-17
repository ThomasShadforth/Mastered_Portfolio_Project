using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveToPlayer", menuName = "Scriptable Objects/Pluggable AI/Actions/Move To Player Action")]
public class MoveTowardsPlayerAction : Action
{
    public override void Act(AIThinker thinker)
    {
        MoveTowardsPlayer(thinker);
    }

    void MoveTowardsPlayer(AIThinker thinker)
    {
        Vector3 direction = GetMoveDirection(thinker);

        thinker._rb.velocity = new Vector3(direction.x * 4, thinker._rb.velocity.y, direction.z * 4);
    }

    public Vector3 GetMoveDirection(AIThinker thinker)
    {
        Vector3 direction = Vector3.zero;

        direction = thinker.playerTarget.position - thinker.transform.position;

        if (direction.x > 0.1)
        {
            direction.x = 1f;
        }
        else if (direction.x < -0.1)
        {
            direction.x = -1f;
        }
        else
        {
            direction.x = 0;
        }

        if (direction.z > 0)
        {
            direction.z = 1f;
        }
        else if (direction.z < 0)
        {
            direction.z = -1f;
        }

        direction = direction.normalized;

        return direction;
    }
}
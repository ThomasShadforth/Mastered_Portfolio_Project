using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveToPlayer", menuName = "Scriptable Objects/Pluggable AI/Actions/Move To Player Action")]
public class MoveTowardsPlayerAction : Action
{
    public override void Act(AIThinker thinker)
    {
        MoveTowardsPlayer(thinker);
        ReduceAttackCooldown(thinker);
    }

    void ReduceAttackCooldown(AIThinker thinker)
    {
        if (thinker.attackCoolTimer > 0)
        {
            thinker.attackCoolTimer -= GamePause.deltaTime;
        }
    }

    void MoveTowardsPlayer(AIThinker thinker)
    {
        Vector3 direction = GetMoveDirection(thinker);

        thinker._rb.velocity = new Vector3(direction.x * 4, thinker._rb.velocity.y, direction.z * 4);
        
        thinker.transform.rotation = Quaternion.Euler(0, GetLookAngle(thinker), 0);
        
    }

    public float GetLookAngle(AIThinker thinker)
    {
        Vector3 lookDirection = Vector3.zero;

        lookDirection = thinker.playerTarget.position - thinker.transform.position;

        float targetAngle = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(thinker.transform.eulerAngles.y, targetAngle, ref thinker.currSmoothVelocity, thinker.rotationSmooth);


        return angle;
    }
    public Vector3 GetMoveDirection(AIThinker thinker)
    {
        Vector3 direction = Vector3.zero;

        direction = thinker.playerTarget.position - thinker.transform.position;

        direction = direction.normalized;

        //Debug.Log(direction);

        /*
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
        */

        //direction = direction.normalized;

        return direction;
    }
}

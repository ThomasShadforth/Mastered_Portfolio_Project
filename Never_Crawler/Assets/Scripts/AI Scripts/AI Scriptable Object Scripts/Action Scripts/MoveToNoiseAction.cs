using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveToNoise", menuName = "Scriptable Objects/Pluggable AI/Actions/Move To Noise Action")]
public class MoveToNoiseAction : Action
{
    public override void Act(AIThinker thinker)
    {
        MoveTowardsNoise(thinker);
    }

    void MoveTowardsNoise(AIThinker thinker)
    {
        Vector3 direction = GetMoveDirection(thinker);

        thinker._rb.velocity = new Vector3(direction.x * 4f, thinker._rb.velocity.y, direction.z * 4f);
        thinker.transform.rotation = Quaternion.Euler(0, GetLookAngle(thinker), 0);
    }

    float GetLookAngle(AIThinker thinker)
    {
        Vector3 direction = thinker.GetNoisePosition() - thinker.transform.position;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        float angle = Mathf.SmoothDampAngle(thinker.transform.eulerAngles.y, targetAngle, ref thinker.currSmoothVelocity, thinker.rotationSmooth);

        return angle;
    }

    Vector3 GetMoveDirection(AIThinker thinker)
    {
        Vector3 direction = thinker.GetNoisePosition() - thinker.transform.position;

        if(direction.x > .1f)
        {
            direction.x = 1f;
        } else if(direction.x < -.1f)
        {
            direction.x = -1f;
        }

        if(direction.z > .1f)
        {
            direction.z = 1f;
        } else if(direction.z < -.1f)
        {
            direction.z = -1f;
        }

        direction = direction.normalized;

        return direction;


    }
}

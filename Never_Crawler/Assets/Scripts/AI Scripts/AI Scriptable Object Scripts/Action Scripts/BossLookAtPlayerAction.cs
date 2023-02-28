using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss Look At Player", menuName = "Scriptable Objects/Pluggable AI/Actions/Boss Look At Player")]
public class BossLookAtPlayerAction : Action
{
    public override void Act(AIThinker thinker)
    {
        SetRotation(thinker);
    }

    void SetRotation(AIThinker thinker)
    {
        thinker.transform.rotation = Quaternion.Euler(0, GetLookAngle(thinker), 0);
    }

    float GetLookAngle(AIThinker thinker)
    {
        Vector3 lookDirection = thinker.playerTarget.position - thinker.transform.position;

        float targetAngle = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg;

        float angle = Mathf.SmoothDampAngle(thinker.transform.eulerAngles.y, targetAngle, ref thinker.currSmoothVelocity, thinker.rotationSmooth);

        return angle;
    }
}

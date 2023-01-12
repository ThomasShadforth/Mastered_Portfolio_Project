using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Test Boss Attack Action", menuName = "Scriptable Objects/Pluggable AI/Actions/Test Boss Attack Action")]
public class TestBossAttackAction : Action
{
    public AbilitySO attackUsed;

    public override void Act(AIThinker thinker)
    {
        thinker.transform.rotation = Quaternion.Euler(0, GetLookAngle(thinker), 0);

        if (thinker.attackCoolTimer <= 0)
        {
            Attack(thinker);
            thinker.SetCooldownTimer();
        }
        else
        {
            CountdownTimer(thinker);
        }

        

    }

    public float GetLookAngle(AIThinker thinker)
    {
        Vector3 lookDirection = Vector3.zero;

        lookDirection = thinker.playerTarget.position - thinker.transform.position;

        float targetAngle = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(thinker.transform.eulerAngles.y, targetAngle, ref thinker.currSmoothVelocity, thinker.rotationSmooth);


        return angle;
    }

    void Attack(AIThinker thinker)
    {
        attackUsed.UseAbility(thinker.stats.intelligence.GetScoreModifier(), null, thinker);
    }

    void CountdownTimer(AIThinker thinker)
    {
        thinker.attackCoolTimer -= GamePause.deltaTime;

    }
}

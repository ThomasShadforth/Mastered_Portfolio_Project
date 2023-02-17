using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack End Decision", menuName = "Scriptable Objects/Pluggable AI/Decisions/Attack End Decision")]
public class AttackEndDecision : Decision
{
    public override bool Decide(AIThinker thinker)
    {
        bool attackEnded = CheckEndedAttack(thinker);

        return attackEnded;
    }

    public bool CheckEndedAttack(AIThinker thinker)
    {
        AnimatorStateInfo stateInfo = thinker.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        float nTime = stateInfo.normalizedTime;


        if(nTime >= 1.0f)
        {
            thinker.attacking = false;
            thinker.SetCooldownTimer();
            return true;
        }

        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Range Decision", menuName = "Scriptable Objects/Pluggable AI/Decisions/Attack Range Decision")]
public class AttackRangeDecision : Decision
{
    public override bool Decide(AIThinker thinker)
    {
        bool canAttack = CheckAttackRange(thinker);
        return canAttack;
    }

    bool CheckAttackRange(AIThinker thinker)
    {
        if(thinker.playerTarget != null)
        {
            float distanceToPlayer = Vector3.Distance(thinker.playerTarget.transform.position, thinker.transform.position);

            if(distanceToPlayer <= thinker.minAttackDist)
            {
                if (CheckAttackCooldown(thinker))
                {
                    
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            
            return false;
        }

        
    }

    bool CheckAttackCooldown(AIThinker thinker)
    {
        if(thinker.attackCoolTimer <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

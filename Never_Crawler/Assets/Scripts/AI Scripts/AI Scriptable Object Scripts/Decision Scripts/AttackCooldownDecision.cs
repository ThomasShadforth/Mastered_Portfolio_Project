using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Cooldown Decision", menuName = "Scriptable Objects/Pluggable AI/Decisions/Attack Cooldown Decision")]
public class AttackCooldownDecision : Decision
{
    public override bool Decide(AIThinker thinker)
    {
        bool isCooldown = CheckAttackCooldown(thinker);

        return isCooldown;
    }

    bool CheckAttackCooldown(AIThinker thinker)
    {
        if(thinker.attackCoolTimer > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

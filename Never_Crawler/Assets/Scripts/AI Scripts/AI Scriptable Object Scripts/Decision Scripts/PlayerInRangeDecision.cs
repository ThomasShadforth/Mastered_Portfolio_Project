using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Check Decision", menuName = "Scriptable Objects/Pluggable AI/Decisions/Player Check Decision")]
public class PlayerInRangeDecision : Decision
{
    public override bool Decide(AIThinker thinker)
    {
        bool playerInRange = CheckForPlayer(thinker);
        return playerInRange;
    }

    bool CheckForPlayer(AIThinker thinker)
    {
        if (thinker.canSeePlayer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

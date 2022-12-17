using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Check Decision", menuName = "Scriptable Objects/Pluggable AI/Decisions/Player Check Decision")]
public class CheckForPlayerDecision : Decision
{
    public override bool Decide(AIThinker thinker)
    {
        bool playerFound = CheckForPlayer(thinker);

        return playerFound;
    }

    bool CheckForPlayer(AIThinker thinker)
    {
        bool canSeePlayer = false;

        if (thinker.canSeePlayer)
        {
            canSeePlayer = true;
            thinker.playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        }
        else
        {
            thinker.playerTarget = null;
            canSeePlayer = false;
        }

        return canSeePlayer;

        
    }
}

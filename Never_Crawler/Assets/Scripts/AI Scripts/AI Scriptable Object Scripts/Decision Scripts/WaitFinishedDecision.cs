using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wait Finished Decision", menuName = "Scriptable Objects/Pluggable AI/Decisions/Wait Finished Decision")]
public class WaitFinishedDecision : Decision
{
    public override bool Decide(AIThinker thinker)
    {
        bool finishedWaiting = CheckWaitFinished(thinker);
        return finishedWaiting;
    }

    bool CheckWaitFinished(AIThinker thinker)
    {
        if(thinker.waitTimer <= 0)
        {
            //Reset the wait timer
            thinker.ResetWaitTimer();
            thinker.agent.enabled = true;
            thinker.SetAgentDestination();
            return true;
        }
        else
        {
            return false;
        }
        
    }
}

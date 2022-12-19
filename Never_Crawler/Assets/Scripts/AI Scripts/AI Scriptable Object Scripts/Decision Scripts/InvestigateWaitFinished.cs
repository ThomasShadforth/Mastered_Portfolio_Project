using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Investigate Wait Decision", menuName = "Scriptable Objects/Pluggable AI/Decisions/Investigate Wait Decision")]
public class InvestigateWaitFinished : Decision
{
    public override bool Decide(AIThinker thinker)
    {
        bool finishedWaiting = CheckForWaitTime(thinker);

        return finishedWaiting;
    }

    bool CheckForWaitTime(AIThinker thinker)
    {
        //Replace waitTimer with a separate timer (For investigating)
        if(thinker.investigateTimer <= 0)
        {
            thinker.ResetWaitTimer();
            thinker.Invoke("ResetNoiseValues", 0f);
            return true;
        }
        else
        {
            return false;
        }
    }
}

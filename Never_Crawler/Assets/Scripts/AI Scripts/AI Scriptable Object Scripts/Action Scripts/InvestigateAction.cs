using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InvestigateAction", menuName = "Scriptable Objects/Pluggable AI/Actions/Investigate Action")]
public class InvestigateAction : Action
{
    public override void Act(AIThinker thinker)
    {
        CountdownInvestigateTime(thinker);   
    }

    void CountdownInvestigateTime(AIThinker thinker)
    {
        thinker.investigateTimer -= GamePause.deltaTime;
    }
}

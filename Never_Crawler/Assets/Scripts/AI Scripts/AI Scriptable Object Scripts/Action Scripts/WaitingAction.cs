using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Waiting Action", menuName = "Scriptable Objects/Pluggable AI/Actions/Waiting")]
public class WaitingAction : Action
{
    public override void Act(AIThinker thinker)
    {
        thinker.waitTimer -= GamePause.deltaTime;
    }
}

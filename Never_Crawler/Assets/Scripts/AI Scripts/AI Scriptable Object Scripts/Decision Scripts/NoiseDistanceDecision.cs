using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Noise Distance Decision", menuName = "Scriptable Objects/Pluggable AI/Decisions/Noise Distance Decision")]
public class NoiseDistanceDecision : Decision
{
    public override bool Decide(AIThinker thinker)
    {
        bool isNearLocation = CheckNoiseDistance(thinker);

        return isNearLocation;
    }

    bool CheckNoiseDistance(AIThinker thinker)
    {
        float distanceToNoise = Vector3.Distance(thinker.transform.position, thinker.GetNoisePosition());

        if(distanceToNoise <= thinker.minDistFromPoint)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

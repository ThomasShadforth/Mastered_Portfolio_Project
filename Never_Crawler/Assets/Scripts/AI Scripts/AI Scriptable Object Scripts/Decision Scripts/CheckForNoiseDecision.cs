using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Check Noise Decision", menuName = "Scriptable Objects/Pluggable AI/Decisions/Check Noise Decision")]
public class CheckForNoiseDecision : Decision
{
    public override bool Decide(AIThinker thinker)
    {
        bool detectedNoise = CheckForNoiseInRange(thinker);

        return detectedNoise;
    }

    bool CheckForNoiseInRange(AIThinker thinker)
    {
        //If the AI Thinker detects a noise in it's base script, check if it's within it's range.
        //If it is, return true. Otherwise, return false;
        if (thinker.GetNoisePosition() != Vector3.zero)
        {
            if (Vector3.Distance(thinker.transform.position, thinker.GetNoisePosition()) <= thinker.noiseCheckRadius + thinker.GetNoiseRadius())
            {
                thinker.SetNoiseInvestigate();
                thinker.ResetWaitTimer();
                return true;
            }
            else
            {
                thinker.Invoke("ResetNoiseValues", 2f);
                return false;
            }
        }
        else
        {
            return false;
        }

        
    }
}

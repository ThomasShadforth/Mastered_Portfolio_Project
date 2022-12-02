using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AI Brain", menuName = "Scriptable Objects/AI Brains/AI Brain")]
public abstract class AIBrain : ScriptableObject
{
    public virtual void Initialize()
    {

    }

    public abstract void Think(AIThinker aiThink);
}

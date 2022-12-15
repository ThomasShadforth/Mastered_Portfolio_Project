using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Test Rotate Action", menuName = "Scriptable Objects/Pluggable AI/Actions/TestRotate")]
public class TestRotateAction : Action
{
    public override void Act(AIThinker thinker)
    {
        thinker.transform.Rotate(new Vector3(0, 1, 0), 60 * Time.deltaTime);
    }
}

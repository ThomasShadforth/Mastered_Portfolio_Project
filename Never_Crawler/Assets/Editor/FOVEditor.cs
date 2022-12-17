using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LineOfSight))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        LineOfSight los = (LineOfSight)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(los.transform.position, Vector3.up, Vector3.forward, 360, los.losRadius);

        Vector3 viewAngle1 = DirectionFromAngle(los.transform.eulerAngles.y, -los.losAngle / 2);
        Vector3 viewAngle2 = DirectionFromAngle(los.transform.eulerAngles.y, los.losAngle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(los.transform.position, los.transform.position + viewAngle1 * los.losRadius);
        Handles.DrawLine(los.transform.position, los.transform.position + viewAngle2 * los.losRadius);

        if (los.canSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(los.transform.position, los.player.transform.position);
        }
    }

    Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}

using UnityEngine;
using System.Collections;
using UnityEditor;
using Unity.VisualScripting;

[CustomEditor(typeof(SightTrigger)), CanEditMultipleObjects]
public class SightEditor : Editor
{
    void OnSceneGUI()
    {
        SightTrigger fow = (SightTrigger)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.AreaViewRadius);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in fow.visibleTargets)
        {
            Handles.DrawLine(fow.transform.position, visibleTarget.position);
        }
    }
}
using UnityEngine;
using System.Collections;
using UnityEditor;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using System.Drawing;

[CustomEditor(typeof(EnemySetup), true), CanEditMultipleObjects]
public class EnemyEditor : Editor
{
    private void OnEnable()
    {
        Tools.hidden = true;
    }

    private void OnDisable()
    {
        Tools.hidden = false;
    }

    void OnSceneGUI()
    {
        EnemySetup ex = (EnemySetup)target;
        EnemyAction example = (EnemyAction)ex.gameObject.transform.GetChild(0).gameObject.GetComponent<EnemyAction>();
        Handles.color = UnityEngine.Color.yellow;

        List<Vector3> points = example.PatrolPoints;

        if (points == null) return;

        for (int i = 0; i < points.Count - 1; i++)
        {
            Handles.DrawLine(points[i], points[i + 1]);
            if (i == points.Count - 2) Handles.DrawLine(points[i + 1], example.PatrolPoints[0]);
        }
    }
}
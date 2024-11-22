using UnityEngine;
using System.Collections;
using UnityEditor;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using System.Drawing;

[CustomEditor(typeof(EnemyAction)), CanEditMultipleObjects]
public class PatrolEditor : Editor
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
        EnemyAction example = (EnemyAction)target;
        Handles.color = UnityEngine.Color.red;

        List<Vector3> points = example.patrolPoints;

        if (points == null) return;

        for (int i = 0; i < points.Count - 1; i++)
        {
            Handles.DrawLine(points[i], points[i + 1]);
            if (i == points.Count - 2) Handles.DrawLine(points[i + 1], example.patrolPoints[0]);
        }

        Vector3 snap = Vector3.one;
        Vector3 holder = Vector3.zero;

        EditorGUI.BeginChangeCheck();
        List<Vector3> newPosition = new List<Vector3>();

        for (int i = 0; i < points.Count; i++)
        {
            newPosition.Add(Handles.FreeMoveHandle(example.patrolPoints[i], 0.5f, snap, Handles.SphereHandleCap));
            holder = newPosition[i];
            holder.y = example.transform.position.y;
            newPosition[i] = holder;
        }

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(example, "Change patrolPoints");
            example.patrolPoints = newPosition;
            if (newPosition.Count > 0) example.transform.position = newPosition[0];
            if (newPosition.Count > 1) example.transform.LookAt(newPosition[1]);
        }
    }
}
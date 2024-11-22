using UnityEngine;
using System.Collections;
using UnityEditor;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using System.Drawing;

[CustomEditor(typeof(Spawner))]
public class SpawnEditor : Editor
{
    private void OnEnable()
    {
        Tools.hidden = true;
    }

    private void OnDisable()
    {
        Tools.hidden = false;
    }
    /*
    void OnSceneGUI()
    {
        Spawner example = (Spawner)target;
        Handles.color = UnityEngine.Color.red;

        List<Vector3> points = example.spawnPoints;

        if (points == null) return;

        Vector3 snap = Vector3.one;
        Vector3 holder = Vector3.zero;

        EditorGUI.BeginChangeCheck();
        List<Vector3> newPosition = new List<Vector3>();

        for (int i = 0; i < points.Count; i++)
        {
            newPosition.Add(Handles.FreeMoveHandle(example.spawnPoints[i], 0.5f, snap, Handles.SphereHandleCap));
            holder = newPosition[i];
            holder.y = example.transform.position.y;
            newPosition[i] = holder;
        }

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(example, "Change spawnPoints");
            example.spawnPoints = newPosition;
        }
    }
    */
}
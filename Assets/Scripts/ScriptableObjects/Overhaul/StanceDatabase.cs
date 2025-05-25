using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StanceDatabase", menuName = "ProjectHades/Player/StanceDatabase", order = 4)]
public class StanceDatabase : ScriptableObject
{
    [SerializeReference] public List<StanceStatsScriptable> Stances = new();
    private Dictionary<EStance, StanceStatsScriptable> _stanceDict = new();
    public bool hasInitialized = false;

    public StanceStatsScriptable GetStance(EStance stance)
    {
        if (!hasInitialized)
            Initialize();
        if (_stanceDict.ContainsKey(stance))
            return _stanceDict[stance];

        Debug.LogWarning($"[WARN]({this}): No stance registered of type {stance}");
        return null;
    }

    private void Initialize()
    {
        _stanceDict.Clear();

        foreach (var stance in Stances)
        {
            _stanceDict.Add(stance.StanceType, stance);
        }
    }
}

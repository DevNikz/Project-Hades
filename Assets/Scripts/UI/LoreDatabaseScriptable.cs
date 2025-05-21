using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoreDatabase", menuName = "ProjectHades/General/LoreDatabase", order = 1)]
public class LoreDatabaseScriptable : ScriptableObject
{
    [SerializeField] public List<string> LoreList = new();
    public string GetRandomLorebit()
    {
        if (LoreList.Count == 0)
            return "";
        return LoreList[Random.Range(0, LoreList.Count)];
    }
}

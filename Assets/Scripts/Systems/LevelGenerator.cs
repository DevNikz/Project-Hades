using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LevelGenerator : MonoBehaviour
{
    [SerializeReference] private List<LevelPrefab> _levelPrefabs = new();
    [SerializeField, ReadOnly] private LevelPrefab _chosenLevel = null;

    [Button("Generate Level")]
    public void GenerateLevel(){
        if(_chosenLevel != null)
            _chosenLevel.Unload();
        
        _chosenLevel = _levelPrefabs[0];
        _chosenLevel.Load();
    }
    
}

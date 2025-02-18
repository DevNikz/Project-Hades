using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LevelGenerator : MonoBehaviour
{
    [SerializeReference] private List<LevelPrefab> _levelPrefabs = new();
    [SerializeField, ReadOnly] private LevelPrefab _chosenLevel = null;
    [SerializeField, ReadOnly] private List<GameObject> _spawnedLevelParts = null;

    [Button("Generate Level")]
    public void GenerateLevel(){
        if(_chosenLevel != null)
            UnloadLevel();
        
        _chosenLevel = _levelPrefabs[0];
        _spawnedLevelParts = _chosenLevel.Load();
    }
    
    private void UnloadLevel(){
        foreach(GameObject part in _spawnedLevelParts){
            if(Application.isEditor)
                DestroyImmediate(part);
            else 
                Destroy(part);
        }

        _spawnedLevelParts.Clear();
    }
}

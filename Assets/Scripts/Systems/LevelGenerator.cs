using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private String _nextLevelName;
    [SerializeReference] private List<LevelPrefab> _levelPrefabs = new();
    [SerializeField, ReadOnly] private LevelPrefab _chosenLevel = null;
    [SerializeField, ReadOnly] private List<GameObject> _spawnedLevelParts = null;

    [Button("Generate Level")]
    public void GenerateLevel(){
        if(_chosenLevel != null)
            UnloadLevel();
        
        _chosenLevel = SelectRandomLevelPrefab();
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

    private LevelPrefab SelectRandomLevelPrefab(){
        int index = UnityEngine.Random.Range(0, _levelPrefabs.Count);
        Debug.Log("Chose index " + index);
        return _levelPrefabs[index];
    }

    private void Start() {
        this.GenerateLevel();

        GameObject playerInputManager = GameObject.Find("PlayerInputManager");
        if(playerInputManager != null)
            playerInputManager.GetComponent<LevelRewardScript>().nextLevel = _nextLevelName;
        else
            Debug.LogWarning("Player Input Manager Missing");
    }
}

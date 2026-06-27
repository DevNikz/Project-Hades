using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.AI.Navigation;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelPrefab", menuName = "ProjectHades/LevelPrefab", order = 1)]
[InlineEditor]
public class LevelPrefab : ScriptableObject
{
    [SerializeReference] private List<GameObject> _layoutPrefabs = new();
    [SerializeReference] private List<GameObject> _spawnpointsAndObstaclesPrefabs = new();
    [SerializeReference] private List<GameObject> _decorPrefabs = new();
    [SerializeReference] private List<GameObject> _backgroundPrefabs = new();
    [SerializeReference] private List<EnemyWaveSet> _possibleWaveSets = new();

    public List<GameObject> Load(int layoutVar = -1, int spawnsAndObstaclesVar = -1, int decorVar = -1, int backgroundVar = -1){
        
        GameObject spawnedObject;
        List<GameObject> spawnedObjects = new();
        
        spawnedObject = Spawn(_layoutPrefabs, layoutVar);
        spawnedObjects.Add(spawnedObject);
        List<NavMeshSurface> navSurfaces = new(
            spawnedObject.GetComponentsInChildren<NavMeshSurface>()
        );

        spawnedObject = Spawn(_spawnpointsAndObstaclesPrefabs, spawnsAndObstaclesVar);
        spawnedObjects.Add(spawnedObject);

        foreach(NavMeshSurface surface in navSurfaces){
            surface.BuildNavMesh();
        }

        spawnedObject = Spawn(_decorPrefabs, decorVar);
        spawnedObjects.Add(spawnedObject);

        spawnedObject = Spawn(_backgroundPrefabs, backgroundVar);
        spawnedObjects.Add(spawnedObject);

        GameObject enemySpawner = GameObject.Find("EnemySpawner");
        if(enemySpawner != null)
            enemySpawner.GetComponent<EnemySpawner>().InitializeSpawner(
                RandomizeEnemyWaveSet()
            );
        else 
            Debug.LogWarning("Cannot find enemy Spawner");

        return spawnedObjects;
    }

    /*
        All objects are spawned at the origin
    */
    private GameObject Spawn(List<GameObject> prefabList, int variant = -1){
        if(prefabList.Count == 0)
            return null;
        
        if(variant == -1)
            variant = RandomizeVariant(prefabList.Count);

        GameObject toSpawn = Instantiate(
            prefabList[variant], Vector3.zero, Quaternion.identity
        );

        return toSpawn;
    }

    private EnemyWaveSet RandomizeEnemyWaveSet(){
        int index = Random.Range(0, _possibleWaveSets.Count);
        return _possibleWaveSets[index];
    }

    private int RandomizeVariant(int maxCount) {
        return Random.Range(0, maxCount);
    }
}

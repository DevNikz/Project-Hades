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

    public List<GameObject> Load(int layoutVar = -1, int spawnsAndObstaclesVar = -1, int decorVar = -1, int backgroundVar = -1){
        
        // GameObject spawnedObject;
        
        // Spawn(_layoutPrefabs, layoutVar, true);
        // List<NavMeshSurface> navSurfaces = 
        // Spawn(_spawnpointsAndObstaclesPrefabs, spawnsAndObstaclesVar);

        // foreach(NavMeshSurface surface in navSurfaces){
        //     surface.BuildNavMesh();
        // }

        Spawn(_decorPrefabs, decorVar);
        Spawn(_backgroundPrefabs, backgroundVar);
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

    private int RandomizeVariant(int maxCount) {
        return Random.Range(0, maxCount - 1);
    }
}

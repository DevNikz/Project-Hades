using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelPrefab", menuName = "ProjectHades/LevelPrefab", order = 1)]
[InlineEditor]
public class LevelPrefab : ScriptableObject
{
    [SerializeReference] private SceneNames.Enums mapBaseSceneName;
    [SerializeReference] private List<GameObject> layoutPrefabs;
    [SerializeReference] private List<GameObject> decorPrefabs;
    [SerializeReference] private List<GameObject> spawnpointPrefabs;
    [SerializeReference] private List<GameObject> backgroundPrefabs;

    public void Load(int LayoutVar = -1, int DecorVar = -1, int SpawnpointVar = -1, int BackgroundVar = -1){
        // LoadLevel();
        // SpawnLayout();
        // SpawnDecor();
        // SpawnSpawnpoints();
        // SpawnBackground();
    }

    /*
        For now all spawned stuffs are in the origin.
    */

    void SpawnMap(int count) {
        // GameObject mapTemp = Instantiate(mapPrefab[RandomGen(mapPrefab.Count)], Vector3.zero, Quaternion.identity);
        //Add logic for maptemp jic
    }

    void SpawnObj(int count) {
        // GameObject objTemp = Instantiate(objectPrefab[RandomGen(objectPrefab.Count)], Vector3.zero, Quaternion.identity);
        //Add logic for objtemp jic
    }

    void SpawnDecor(int count) {
        // GameObject decorTemp = Instantiate(decorPrefab[RandomGen(decorPrefab.Count)], Vector3.zero, Quaternion.identity);
        //Add logic for decortemp jic
    }

    int RandomGen(int count) {
        return Random.Range(0, count);
    }
}

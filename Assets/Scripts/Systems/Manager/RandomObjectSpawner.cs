using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class RandomObjectSpawner : MonoBehaviour
{
    public static RandomObjectSpawner Instance;

    [PropertySpace, Title("Properties", TitleAlignment = TitleAlignments.Centered)]
    [AssetList] private List<GameObject> mapPrefab;
    [AssetList] private List<GameObject> objectPrefab;
    [AssetList] private List<GameObject> decorPrefab;

    [PropertySpace, Title("Preview", TitleAlignment = TitleAlignments.Centered)]
    [InlineEditor(InlineEditorModes.LargePreview)] private GameObject selectedMap;
    [InlineEditor(InlineEditorModes.LargePreview)] private GameObject selectedObject;
    [InlineEditor(InlineEditorModes.LargePreview)] private GameObject selectedDecor;

    void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start() {
        transform.position = Vector3.zero; //set obj to origin
    }

    public void Init() {
        SpawnMap(mapPrefab.Count);
        SpawnObj(objectPrefab.Count);
        SpawnDecor(decorPrefab.Count);
    }

    /*
        For now all spawned stuffs are in the origin.
    */

    void SpawnMap(int count) {
        GameObject mapTemp = Instantiate(mapPrefab[RandomGen(mapPrefab.Count)], Vector3.zero, Quaternion.identity);
        //Add logic for maptemp jic
    }

    void SpawnObj(int count) {
        GameObject objTemp = Instantiate(objectPrefab[RandomGen(objectPrefab.Count)], Vector3.zero, Quaternion.identity);
        //Add logic for objtemp jic
    }

    void SpawnDecor(int count) {
        GameObject decorTemp = Instantiate(decorPrefab[RandomGen(decorPrefab.Count)], Vector3.zero, Quaternion.identity);
        //Add logic for decortemp jic
    }

    int RandomGen(int count) {
        return Random.Range(0, count);
    }

}

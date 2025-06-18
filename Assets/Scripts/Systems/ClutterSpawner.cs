using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class ClutterSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _density;
    [SerializeField] private bool _randomizeOnLoad;
    [SerializeField] private List<GameObject> _clutterPrefabs = new();
    [SerializeField] List<GameObject> _spawnedObjects = new();
    [SerializeField] List<MeshRenderer> _meshObjects = new();

    void Start()
    {
        //SceneManager.sceneLoaded += OnSceneLoaded;

        if (_randomizeOnLoad)
            SpawnClutter();
        else
        {
            //GetSpawnedObjects();
            GetMeshObjects();
            if (GameSetting.Instance != null) SetClutterVisiblity(GameSetting.Instance.highDetail);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (GameSetting.Instance != null)
        {
            SetClutterVisiblity(GameSetting.Instance.highDetail);
        }
    }

    void GetSpawnedObjects()
    {
        _spawnedObjects.Clear();
        _spawnedObjects.AddRange(transform.GetComponentsInChildren<GameObject>());
    }

    void GetMeshObjects()
    {
        _meshObjects.Clear();
        _meshObjects.AddRange(transform.GetComponentsInChildren<MeshRenderer>());
        // if (_spawnedObjects != null)
        // {
        //     for (int i = 0; i < _spawnedObjects.Count; i++)
        //     {
        //         _meshObjects.Add(_spawnedObjects[i].GetComponent<MeshRenderer>());
        //     }
        // }
    }

    void Update()
    {
        if (GameSetting.Instance != null)
        {
            SetClutterVisiblity(GameSetting.Instance.highDetail);
        }   
    }

    public void SetClutterVisiblity(bool value)
    {
        GetMeshObjects();
        foreach (MeshRenderer m in _meshObjects)
        {
            m.enabled = value;
        }
    }

    [Button("Spawn Clutter")]
    public void SpawnClutter()
    {
        // Reset
        foreach (GameObject obj in _spawnedObjects)
        {
            if(!Application.isPlaying)
                DestroyImmediate(obj);
            else
                Destroy(obj);
        }
        _spawnedObjects.Clear();

        // Spawn radius doubled so the poolsize scales by square radius, using simple pie
        int amountToSpawn = (int)(_spawnRadius * _spawnRadius * _density * 3.1415f);

        for (int i = 0; i < amountToSpawn; i++)
        {
            GameObject clutter = Instantiate(GetRandomClutter(), transform);
            clutter.transform.position = GetRandomSpawnpoint();
            clutter.transform.Rotate(Vector3.up, Random.Range(0, 360), Space.World);
            _spawnedObjects.Add(clutter);
            clutter.SetActive(true);
        }
    }

    private GameObject GetRandomClutter()
    {
        return _clutterPrefabs[Random.Range(0, _clutterPrefabs.Count)];
    }

    private Vector3 GetRandomSpawnpoint()
    {
        return transform.position + new Vector3(Random.Range(-_spawnRadius, _spawnRadius), 0.0f, Random.Range(-_spawnRadius, _spawnRadius));
    }

    private Quaternion GetRandomRotation()
    {
        return Quaternion.AngleAxis(Random.Range(0,360), Vector3.up);
    }

}

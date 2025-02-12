using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set;}
    void Start()
    {
        if(Instance == null)
            Instance = this;
        else 
            Destroy(this);
    }

    [SerializeReference] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeReference] private List<Waves> waves;
    private int waveCounter = 0;
    private int enemyCounter;
    private int RandomSpawn;
    public bool FinalWave;

    private ObjectPool _objectPool;

    // Start is called before the first frame update
    void Awake()
    {
        this._objectPool = this.gameObject.GetComponent<ObjectPool>();
    }

    // Update is called once per frame
    void Update()
    {
        if (FinalWave) this.enabled = false;

        enemyCounter = _objectPool.ReleasedCount;
        if (enemyCounter <= 0) SpawnWave();
    }

    public void InitializeSpawner(SpawnerWaves spawnPreset = null){
        if (spawnPreset != null)
            this.waves = spawnPreset.waves;
        FinalWave = false;
        waveCounter = 0;
    }

    public void AddSpawnpoint(Transform spawnpoint){
        if (spawnpoint == null) return;
        spawnPoints.Add(spawnpoint);
    }

    public void ClearSpawnPoints(){
        spawnPoints.Clear();
    }

    private void SpawnWave()
    {
        if(spawnPoints.Count <= 0) return;

        for (int i = 0; i < waves[waveCounter].Enemies.Length; i++)
        {
            for (int j = 0; j < waves[waveCounter].Amount[i]; j++)
            {
                RandomSpawn = Random.Range(0, spawnPoints.Count);
                Instantiate(waves[waveCounter].Enemies[i], spawnPoints[RandomSpawn]);
            } 
        }

        // foreach(GameObject enemy in waves[waveCounter].Enemies){
        //     this.object
        // }

        waveCounter++;
        if (waves.Count == waveCounter) FinalWave = true;
    }
}

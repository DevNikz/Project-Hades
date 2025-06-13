using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set;}
    void Start()
    {
        if(Instance == null)
            Instance = this;
        else 
            Destroy(this);
    }

    [SerializeReference] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeReference] private List<EnemyWave> waves;
    private int waveCounter = 0;
    private int enemyCounter;
    private int RandomSpawn;
    public float enemyCooldown = 0.25f;
    public bool FinalWave;
    private LevelTrigger _levelTrigger;

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

        enemyCounter = GameObject.FindGameObjectsWithTag("Enemy").Length;
        // enemyCounter = _objectPool.ReleasedCount;
        if(enemyCounter <= 0 && !FinalWave) SpawnWave();
    }

    public void InitializeSpawner(EnemyWaveSet spawnPreset = null){
        if (spawnPreset != null)
            this.waves = spawnPreset.EnemyWaves;
        FinalWave = false;
        this.enabled = true;
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

        for (int i = 0; i < waves[waveCounter].EnemyList.Count; i++)
        {
            for (int j = 0; j < waves[waveCounter].EnemyList[i].Amount; j++)
            {
                RandomSpawn = Random.Range(0, spawnPoints.Count);
                GameObject enemy = Instantiate(waves[waveCounter].EnemyList[i].Enemy, spawnPoints[RandomSpawn].transform.position, Quaternion.identity);
                enemy.GetComponent<EnemyAction>().Cooldown = enemyCooldown;
            } 
        }

        // foreach(GameObject enemy in waves[waveCounter].Enemies){
        //     this.object
        // }

        waveCounter++;
        if (waves.Count == waveCounter) FinalWave = true;
    }
}

using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set;}
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        SceneManager.sceneLoaded += OnSceneLoad;
    }

    [SerializeField] private LevelRewardScript _rewardMenu;
    [SerializeField] private float _rewardShowDelayTime = 0.2f;
    [SerializeField] private bool _rewardAugmentPerWave = false;
    [SerializeReference] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeReference] private List<EnemyWave> waves;
    private int waveCounter = 0;
    private int enemyCounter;
    private int RandomSpawn;
    public float enemyCooldown = 0.25f;
    public bool FinalWave;
    private LevelTrigger _levelTrigger;

    private ObjectPool _objectPool;
    private bool _triggeredSpawn = false;

    // Start is called before the first frame update
    void Awake()
    {
        this._objectPool = this.gameObject.GetComponent<ObjectPool>();
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode) {
        if (SaveManager.Instance != null && SaveManager.Instance.CurrentFloorWaveset != null)
            InitializeSpawner(SaveManager.Instance.CurrentFloorWaveset);
    }

    // Update is called once per frame
    void Update()
    {

        enemyCounter = GameObject.FindGameObjectsWithTag("Enemy").Length;
        // enemyCounter = _objectPool.ReleasedCount;
        if (enemyCounter <= 0 && !_triggeredSpawn)
        {
            _triggeredSpawn = true;
            if (_rewardAugmentPerWave)
            {
                StartCoroutine(DelayedRewardOpen(_rewardShowDelayTime));
                if (waves.Count <= 0 || spawnPoints.Count <= 0)
                    FinalWave = true;
                if (FinalWave) this.enabled = false;
            }
            else
            {
                SpawnWave();

            }
        }
    }

    IEnumerator DelayedRewardOpen(float time)
    {
        yield return new WaitForSeconds(time);
        
        _rewardMenu.Activate(true);
    }

    public void InitializeSpawner(EnemyWaveSet spawnPreset = null)
    {
        if (spawnPreset != null)
            this.waves = spawnPreset.EnemyWaves;
        FinalWave = false;
        this.enabled = true;
        waveCounter = 0;
        _triggeredSpawn = false;
    }

    public void AddSpawnpoint(Transform spawnpoint){
        if (spawnpoint == null) return;
        spawnPoints.Add(spawnpoint);
    }

    public void ClearSpawnPoints(){
        spawnPoints.Clear();
    }

    public void SpawnWave()
    {
        _triggeredSpawn = false;
        if (FinalWave) return;
        if (waves.Count <= 0 || spawnPoints.Count <= 0)
        {
            FinalWave = true;
            return;
        }

        for (int i = 0; i < waves[waveCounter].EnemyList.Count; i++)
            {
                for (int j = 0; j < waves[waveCounter].EnemyList[i].Amount; j++)
                {
                    RandomSpawn = Random.Range(0, spawnPoints.Count);
                    Transform spawnpoint = spawnPoints[RandomSpawn];
                    if(spawnpoint == null) continue;

                    GameObject enemy = Instantiate(waves[waveCounter].EnemyList[i].Enemy, spawnpoint.transform.position, Quaternion.identity);
                    // enemy.GetComponent<EnemyAction>().Cooldown = enemyCooldown;
                }
            }

        // foreach(GameObject enemy in waves[waveCounter].Enemies){
        //     this.object
        // }

        waveCounter++;
        if (waves.Count == waveCounter) FinalWave = true;

    }
}

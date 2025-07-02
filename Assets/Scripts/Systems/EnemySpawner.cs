using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    #region References
    [Header("References")]
    [SerializeField] private TMP_Text               _waveCallout;
    [SerializeField] private LevelRewardScript      _rewardMenu;
    #endregion

    #region Settings
    [Header("Settings")]
    [SerializeField] private float                  _rewardShowDelayTime = 0.2f;
    [SerializeField] private bool                   _rewardAugmentPerWave = false;
    [SerializeField] private int                    _enemiesPerBatch;
    [SerializeField] private float                  _spawnBatchCooldown;
    [SerializeField] private int                    _enemiesRemainingBeforeNextWave;
    [SerializeReference] private EnemyWaveSet       _waveSet;
    #endregion

    #region Setup and Activation
    private bool _isActive = false;
    private List<Transform> _spawnpoints = new();
    void Awake()
    {
        _waveCallout.gameObject.SetActive(false);
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        LocalInitialize();
    }
    void OnSceneUnloaded(Scene scene)
    {
        Deactivate();
    }
    private void LocalInitialize()
    {
        if (_isActive) return;

        GameObject playerSpawn = GameObject.Find("PlayerSpawn");
        if (playerSpawn == null) return;

        if (SaveManager.Instance != null && SaveManager.Instance.CurrentFloorWaveset != null)
            InitializeSpawner(SaveManager.Instance.CurrentFloorWaveset);
        else
            InitializeSpawner();
    }

    public void InitializeSpawner(EnemyWaveSet spawnPreset = null)
    {
        _waveSet = spawnPreset;
        _awaitingNextSpawn = false;
        _isActive = true;
        Debug.Log("Is Active Check " + _isActive);

        if (_waveSet != null)
        {
            WarmSpawns();
            SpawnWave();
        }
    }

    public void Deactivate()
    {
        _spawnpoints.Clear();
        _waveSet = null;
        _isActive = false;

        // foreach (var wave in _toSpawnEnemyWaves)
        // {
        //     foreach (GameObject enemy in wave) Destroy(enemy);
        // }
        _toSpawnEnemyWaves.Clear();
    }

    public void AddSpawnpoint(Transform spawnpoint)
    {
        if (spawnpoint == null) return;
        _spawnpoints.Add(spawnpoint);
    }
    #endregion

    #region Check Wave Completion
    private bool _awaitingNextSpawn = false;
    public bool AreWavesOver { get { return GameObject.FindGameObjectsWithTag("Enemy").Length <= 0 && IsFinalWave; } }
    public bool IsFinalWave { get { return _toSpawnEnemyWaves.Count <= 0; } }
    private bool CanSpawnNextWave { get { return _activeEnemyCount <= _enemiesRemainingBeforeNextWave && !_awaitingNextSpawn; }}
    void Update()
    {
        if (_waveSet != null)
        {
            if (CanSpawnNextWave && _isActive)
            {
                _awaitingNextSpawn = true;
                if (IsFinalWave) _isActive = false;
                Debug.Log("Is Active Check " + _isActive);
                if (!_rewardAugmentPerWave) SpawnWave();
                else StartCoroutine(DelayedRewardOpen(_rewardShowDelayTime));
            }
        }
        else
        {
            if (AreWavesOver && _isActive)
            {
                _awaitingNextSpawn = true;
                _isActive = false;
                if (!_rewardAugmentPerWave) SpawnWave();
                else StartCoroutine(DelayedRewardOpen(_rewardShowDelayTime));
            }
            Debug.Log("Is Active Check " + _isActive);
        }
    }

    IEnumerator DelayedRewardOpen(float time)
    {
        yield return new WaitForSeconds(time);

        _rewardMenu.Activate(true);
    }

    #endregion

    #region Spawning
    private int _activeEnemyCount;
    private Queue<Queue<GameObject>> _toSpawnEnemyWaves = new();
    public void SpawnWave()
    {
        _awaitingNextSpawn = false;
        if (_toSpawnEnemyWaves.Count > 0)
            StartCoroutine(SpawnWaveCoroutine(_toSpawnEnemyWaves.Dequeue()));
    }
    private IEnumerator SpawnWaveCoroutine(Queue<GameObject> waveEnemies)
    {
        SpawnBatch(waveEnemies);
        yield return new WaitForSeconds(_spawnBatchCooldown);
    }
    private void SpawnBatch(Queue<GameObject> waveEnemies)
    {
        for (int i = 0; i < _enemiesPerBatch && waveEnemies.Count > 0; i++)
            SpawnEnemy(waveEnemies.Dequeue());
    }
    private void SpawnEnemy(GameObject enemy)
    {
        int randomSpawn = _spawnpoints.Count > 0 ? Random.Range(0, _spawnpoints.Count) : -1;
        Transform spawnpoint = randomSpawn >= 0 ? _spawnpoints[randomSpawn] : null;
        Vector3 spawnPosition = spawnpoint == null ? gameObject.transform.position : spawnPosition = spawnpoint.position;

        _activeEnemyCount++;
        enemy.transform.position = spawnPosition;
        enemy.SetActive(true);
    }
    private void WarmSpawns()
    {
        foreach (EnemyWave wave in _waveSet.EnemyWaves)
        {
            List<GameObject> spawnedEnemies = new();
            foreach (EnemyWave.EnemyCountPair countpair in wave.EnemyList)
            {
                for (int i = 0; i < countpair.Amount; i++)
                {
                    GameObject obj = WarmSpawn(countpair.Enemy);
                    if(obj != null) spawnedEnemies.Add(obj);
                }
            }

            Queue<GameObject> waveEnemies = new();
            while (spawnedEnemies.Count > 0)
            {
                int randomEnemy = Random.Range(0, spawnedEnemies.Count);
                waveEnemies.Enqueue(spawnedEnemies[randomEnemy]);
                spawnedEnemies.RemoveAt(randomEnemy);
            }

            _toSpawnEnemyWaves.Enqueue(waveEnemies);
        }
    }
    private GameObject WarmSpawn(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, gameObject.transform.position, Quaternion.identity);
        obj.SetActive(false);

        EnemyDeath deathScript = obj.GetComponentInChildren<EnemyDeath>();
        if (obj == null) return null;

        deathScript.OnDeathSpawnerCallback = OnDeathCallback;
        return obj;
    }
    public void OnDeathCallback(GameObject obj)
    {
        _activeEnemyCount--;
    }
    #endregion

    public static EnemySpawner Instance { get; private set; }
    void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Debug.Log("Start");
        Instance = this;
        SceneManager.sceneLoaded += OnSceneLoad;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        LocalInitialize();
    }
}

using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
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
    [SerializeField] private float                  _fallingSpawnDelay;
    [SerializeField] private Vector3                _fallSpawnOffset;
    [SerializeField] private int                    _enemiesRemainingBeforeNextWave;
    [SerializeField] private float                  _waveCalloutFadeTime;
    [SerializeField] private float                  _waveCalloutHoverTime;
    [SerializeField] private string                 _waveCalloutPrefix;
    [SerializeField] private string                 _finalWaveCallout;
    [SerializeField] private string                 _levelClearCallout;
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
        if (scene.name == "HubLevel" || scene.name == "Title Screen") _isActive = false;
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
        _activeEnemyCount = 0;

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
        else StartCoroutine(WaveCalloutHover(_levelClearCallout));
    }
    private IEnumerator SpawnWaveCoroutine(Queue<GameObject> waveEnemies)
    {
        if(IsFinalWave) StartCoroutine(WaveCalloutHover(_finalWaveCallout));
        else StartCoroutine(WaveCalloutHover(_waveCalloutPrefix + (_waveSet.EnemyWaves.Count - _toSpawnEnemyWaves.Count)));
        while (waveEnemies.Count > 0)
        {
            SpawnBatch(waveEnemies);
            yield return new WaitForSeconds(_spawnBatchCooldown);
        }
    }
    private IEnumerator WaveCalloutHover(string calloutText)
    {
        if(_waveCalloutHoverTime <= 0) yield break;
        
        _waveCallout.text = calloutText;
        _waveCallout.gameObject.SetActive(true);
        Color textColor = _waveCallout.color;

        float elapsedTime = 0f;
        while (elapsedTime < _waveCalloutFadeTime) {
            elapsedTime += Time.deltaTime;
            _waveCallout.color = new Color(textColor.r, textColor.g, textColor.b, elapsedTime / _waveCalloutFadeTime);

            yield return new WaitForEndOfFrame();
        }
        _waveCallout.color = new Color(textColor.r, textColor.g, textColor.b, 1f);

        yield return new WaitForSeconds(_waveCalloutHoverTime);

        elapsedTime = _waveCalloutFadeTime;
        while (elapsedTime > 0f)
        {
            elapsedTime -= Time.deltaTime;
            _waveCallout.color = new Color(textColor.r, textColor.g, textColor.b, elapsedTime / _waveCalloutFadeTime);

            yield return new WaitForEndOfFrame();
        }
        _waveCallout.gameObject.SetActive(false);
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
        Vector3 spawnPosition = spawnpoint == null ? gameObject.transform.position : spawnpoint.position;
        _activeEnemyCount++;

        if (_fallingSpawnDelay > 0)
        {
            enemy.transform.position = spawnPosition + _fallSpawnOffset;
            StartCoroutine(SpawnEnemyFalling(enemy, spawnPosition + _fallSpawnOffset, spawnPosition));
            enemy.SetActive(true);
        }
        else
        {
            enemy.transform.position = spawnPosition;
            enemy.SetActive(true);
        }

    }

    private IEnumerator SpawnEnemyFalling(GameObject enemy, Vector3 startPos, Vector3 endPos)
    {
        NavMeshAgent agent = enemy.GetComponentInChildren<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
            agent.updatePosition = false;
        }

        float elapsedTime = 0f;
        while (elapsedTime < _fallingSpawnDelay)
        {
            enemy.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / _fallingSpawnDelay);
            if (agent != null)
            {
                agent.nextPosition = enemy.transform.position;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        enemy.transform.position = endPos;
        agent.nextPosition = endPos;


        if (agent != null)
        {
            agent.enabled = true;
            agent.updatePosition = true;
        }
        /*

        NavMeshHit hit;
        if (NavMesh.SamplePosition(endPos, out hit, 5.0f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position); // Only use Warp to relocate
            else
            {
                enemy.transform.position = hit.position;
            }
        }
        */
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
                    if (obj != null) spawnedEnemies.Add(obj);
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

        if(SceneManager.GetActiveScene().name == "HubLevel" || SceneManager.GetActiveScene().name == "Title Screen") _isActive = false;
    }
}

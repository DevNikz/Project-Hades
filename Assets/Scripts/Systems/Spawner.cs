using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public  List<Transform> spawnPoints;
    public List<Waves> waves;
    private int waveCounter = 0;
    private int enemyCounter;
    private int RandomSpawn;
    public bool FinalWave;

    // Start is called before the first frame update
    void Awake()
    {
        if (spawnPoints == null)
        {
            Transform transform = null;
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            spawnPoints.Add(transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (FinalWave) this.enabled = false;

        enemyCounter = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (enemyCounter <= 0) SpawnWave();
    }

    private void SpawnWave()
    {
        for (int i = 0; i < waves[waveCounter].Enemies.Length; i++)
        {
            for (int j = 0; j < waves[waveCounter].Amount[i]; j++)
            {
                RandomSpawn = Random.Range(0, spawnPoints.Count);
                Instantiate(waves[waveCounter].Enemies[i], spawnPoints[RandomSpawn]);
            } 
        }

        waveCounter++;
        if (waves.Count == waveCounter) FinalWave = true;
    }
}

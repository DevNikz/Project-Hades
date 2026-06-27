using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnpoint : MonoBehaviour
{
    void OnEnable()
    {
        GameObject enemySpawner = GameObject.Find("EnemySpawner");
        if(enemySpawner != null){
            enemySpawner.GetComponent<EnemySpawner>().AddSpawnpoint(this.transform);
        } else
            Debug.LogWarning("Spawnpoint cannot find enemy spawner");
    }
}

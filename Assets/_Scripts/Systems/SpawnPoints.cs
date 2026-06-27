using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnPoints
{
    public List<int> spawnPoints;

    public int this[int key]
    {
        get
        {
            return spawnPoints[key];
        }
        set
        {
            spawnPoints[key] = value;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    void Awake(){
        //Debug.Log("Spawn Awoken");
    }

    void OnEnable() {
        // SceneManager.sceneLoaded += OnSceneLoaded;
        //Debug.Log("Spawn Enabled");
        this.StartCoroutine(DelayedSpawn());
    }

    void OnDestroy() {
        // SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Spawn();
        //Debug.Log("Try TP Player");
    }

    private IEnumerator DelayedSpawn(){
        yield return new WaitWhile(didFindPlayer);
        Spawn();
    }

    private bool didFindPlayer(){
        return GameObject.Find("Player") == null;
    }

    void Spawn() {
        GameObject player = GameObject.Find("Player");

        //Instantiating player causes a bug.
        // if(player == null){
        //     player = GameObject.Instantiate(playerPrefab);
        //     player.gameObject.tag = "Player";
        //     player.SetActive(true);
        // }
        
        player.transform.position = this.transform.position;

        //Destroy(gameObject);
    }
}

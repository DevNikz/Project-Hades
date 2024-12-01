using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    void Awake(){
        if(this.playerPrefab != null)
            this.playerPrefab.SetActive(false);
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Spawn();
        Debug.Log("Try TP Player");
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

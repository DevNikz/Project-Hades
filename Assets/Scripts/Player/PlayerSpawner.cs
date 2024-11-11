using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    void Awake(){
        this.playerPrefab.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        if(player == null){
            player = GameObject.Instantiate(playerPrefab);
            player.SetActive(true);
        }
        
        player.transform.position = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

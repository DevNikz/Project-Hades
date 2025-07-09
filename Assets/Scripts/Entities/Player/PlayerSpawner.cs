using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private string _startOfLevelDialogueTag;
    [SerializeField] private bool _tagIsDatabase;

    void Awake()
    {
        //Debug.Log("Spawn Awoken");
    }

    void OnEnable()
    {
        // SceneManager.sceneLoaded += OnSceneLoaded;
        //Debug.Log("Spawn Enabled");
        this.StartCoroutine(DelayedSpawn());
    }

    void OnDestroy()
    {
        // SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Spawn();
        //Debug.Log("Try TP Player");
    }

    private IEnumerator DelayedSpawn()
    {
        yield return new WaitWhile(didFindPlayer);
        Spawn();
    }

    private bool didFindPlayer()
    {
        return GameObject.Find("Player") == null;
    }

    void Spawn()
    {
        GameObject player = GameObject.Find("Player");

        //Instantiating player causes a bug.
        // if(player == null){
        //     player = GameObject.Instantiate(playerPrefab);
        //     player.gameObject.tag = "Player";
        //     player.SetActive(true);
        // }
        Camera camera = player.GetComponentInChildren<Camera>();
        Vector3 camOffset = Vector3.zero;
        if (camera != null)
            camOffset = camera.transform.position - player.transform.position;

        player.transform.position = this.transform.position;

        if (camera != null)
            camera.transform.position = player.transform.position + camOffset;

        //Destroy(gameObject);
        TryStartDialogue();
    }

    private void TryStartDialogue()
    {
        if (_startOfLevelDialogueTag.IsNullOrWhitespace()) return;
        if (DialogueManager.Instance == null) return;

        if (_tagIsDatabase)
            DialogueManager.Instance.StartRandomDialogueFromDatabase(_startOfLevelDialogueTag);
        else
            DialogueManager.Instance.StartDialogue(_startOfLevelDialogueTag);
    }
}

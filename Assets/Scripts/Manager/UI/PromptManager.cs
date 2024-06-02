using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PromptManager : MonoBehaviour
{
    [ReadOnly] private GameObject[] canvasArray;
    [SerializeField] public List<GameObject> canvasList;

    private string canvasName;

    public const string PROMPT_NAME = "PROMPT_NAME";


    private void Start() {
        canvasList = new List<GameObject>();
        
        canvasArray = GameObject.FindGameObjectsWithTag("UI");

        foreach(GameObject i in canvasArray) {
            this.canvasList.Add(i);
        }

        foreach(GameObject i in canvasList) {
            i.GetComponent<Canvas>().enabled = false;
        }

        EventBroadcaster.Instance.AddObserver(EventNames.Prompt.PROMPT_NAMES_ADD, this.EnablePrompt);
        EventBroadcaster.Instance.AddObserver(EventNames.Prompt.PROMPT_NAMES_DELETE, this.DisablePrompt);
    }

    private void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.Prompt.PROMPT_NAMES_ADD);
        EventBroadcaster.Instance.RemoveObserver(EventNames.Prompt.PROMPT_NAMES_DELETE);
    }

    public void EnablePrompt(Parameters parameters) {
        canvasName = parameters.GetStringExtra(PROMPT_NAME, "None");

        foreach(GameObject i in canvasList) {
            if(i.name == canvasName) {
                i.GetComponent<Canvas>().enabled = true;
            }
        }
    }

    public void DisablePrompt(Parameters parameters) {
        canvasName = parameters.GetStringExtra(PROMPT_NAME, "None");

        foreach(GameObject i in canvasList) {
            if(i.name == canvasName) {
                i.GetComponent<Canvas>().enabled = false;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptManager : MonoBehaviour
{

    [SerializeField] public List<Canvas> canvasList;

    private string canvasName;

    public const string PROMPT_NAME = "PROMPT_NAME";


    private void Start() {
        foreach(Canvas i in canvasList) {
            i.enabled = false;
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

        foreach(Canvas i in canvasList) {
            if(i.name == canvasName) {
                i.enabled = true;
            }
        }
    }

    public void DisablePrompt(Parameters parameters) {
        canvasName = parameters.GetStringExtra(PROMPT_NAME, "None");

        foreach(Canvas i in canvasList) {
            if(i.name == canvasName) {
                i.enabled = false;
            }
        }
    }
}

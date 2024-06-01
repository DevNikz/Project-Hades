using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    public static InteractManager Instance;

    [SerializeField] public List<GameObject> interactableList;

    public Dictionary<string, bool> activeList;
    
    private string interactName;

    private Color color;

    private MeshRenderer meshRenderer;

    public const string INTERACT_NAME = "INTERACT_NAME";


    private void Start() {
        this.activeList = new Dictionary<string, bool>();

        foreach(GameObject i in interactableList) {
            this.activeList.Add(i.name, false);
        }

        EventBroadcaster.Instance.AddObserver(EventNames.Active.INTERACT_ENABLE, this.EnableInteract);
        EventBroadcaster.Instance.AddObserver(EventNames.Active.INTERACT_DISABLE, this.DisableInteract);
    }

    private void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.Active.INTERACT_ENABLE);
        EventBroadcaster.Instance.RemoveObserver(EventNames.Active.INTERACT_DISABLE);
    }

    public void EnableInteract(Parameters parameters) {
        interactName = parameters.GetStringExtra(INTERACT_NAME, "None");

        foreach(GameObject i in interactableList) {
            if(i.name == interactName) {
                activeList[i.name] = true;

                //Change Color
                color = new Color(0.25f, 1f, 0f, 1f);
                meshRenderer = i.GetComponent<MeshRenderer>();
                meshRenderer.material.SetColor("_BaseColor", color);

                //Debug
                Debug.Log(i.name + ": " + this.activeList[i.name]);
            }
        }
    }

    public void DisableInteract(Parameters parameters) {
        interactName = parameters.GetStringExtra(INTERACT_NAME, "None");

        foreach(GameObject i in interactableList) {
            if(i.name == interactName) {
                activeList[i.name] = false;

                //Change Color
                color = new Color(0.541f, 0.541f, 0.541f, 1f);
                meshRenderer = i.GetComponent<MeshRenderer>();
                meshRenderer.material.SetColor("_BaseColor", color);

                //Debug
                Debug.Log(i.name + ": " + this.activeList[i.name]);
            }
        }
    }

    public bool GetActive(string name) {
        if(this.activeList.ContainsKey(name)) {
            return this.activeList[name];
        }
        return false;
    }
}

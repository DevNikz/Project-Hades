using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance;
    [ReadOnly] private GameObject[] eventArray;
    [ReadOnly] private GameObject[] pressArray;
    [ReadOnly] private GameObject[] holdArray;
    [ReadOnly] private GameObject[] toggleArray;

    [SerializeField] public List<GameObject> eventList;
    [SerializeField] public List<GameObject> pressList;
    [SerializeField] public List<GameObject> holdList;
    [SerializeField] public List<GameObject> toggleList;

    private Dictionary<string, bool> activeList;

    private string interactName;

    private MeshRenderer meshRenderer;
    private Color color;

    public const string INTERACT_NAME = "INTERACT_NAME";

    private void Start() {
        activeList = new Dictionary<string, bool>();

        eventList = new List<GameObject>();
        pressList = new List<GameObject>();
        holdList = new List<GameObject>();
        toggleList = new List<GameObject>();

        eventArray = GameObject.FindGameObjectsWithTag("Event"); //For Specific Scene / Area Triggers
        pressArray = GameObject.FindGameObjectsWithTag("Press"); //Interact w/ Merchants etc
        holdArray = GameObject.FindGameObjectsWithTag("Hold"); //Hold Event Triggers
        toggleArray = GameObject.FindGameObjectsWithTag("Toggle"); //Open / Close Door Triggers

        //Set List
        foreach(GameObject i in eventArray) {
            this.eventList.Add(i);
        }

        foreach(GameObject i in pressArray) {
            this.pressList.Add(i);
        }

        foreach(GameObject i in holdArray) {
            this.holdList.Add(i);
        }

        foreach(GameObject i in toggleArray) {
            this.toggleList.Add(i);
        }

        //Set Active Dictionary per Type
        foreach(GameObject i in eventList) {
            this.activeList.Add(i.name, false);
        }

        foreach(GameObject i in pressList) {
            this.activeList.Add(i.name, false);
        }

        foreach(GameObject i in holdList) {
            this.activeList.Add(i.name, false);
        }

        foreach(GameObject i in toggleList) {
            this.activeList.Add(i.name, false);
        }

        EventBroadcaster.Instance.AddObserver(EventNames.Active.INTERACT_ENABLE, this.inputEnable);
        EventBroadcaster.Instance.AddObserver(EventNames.Active.INTERACT_DISABLE, this.inputDisable);
        EventBroadcaster.Instance.AddObserver(EventNames.Active.INTERACT_EXIT, this.ExitInteract);
    }

    private void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.Active.INTERACT_ENABLE);
        EventBroadcaster.Instance.RemoveObserver(EventNames.Active.INTERACT_EXIT);
    }

    public void inputEnable(Parameters parameters) {
        interactName = parameters.GetStringExtra(INTERACT_NAME, "NONE");
        foreach(GameObject i in pressList) {
            if(i.name == interactName) {
                activeList[i.name] = true; //Enable Reference

                //Change Color
                color = new Color(1f, 1f, 1f, 1f);
                meshRenderer = i.GetComponent<MeshRenderer>();
                meshRenderer.material.SetColor("_BaseColor", color);
            }
        }
    }

    public void inputDisable(Parameters parameters) {
        interactName = parameters.GetStringExtra(INTERACT_NAME, "NONE");
        foreach(GameObject i in pressList) {
            if(i.name == interactName) {
                activeList[i.name] = false; //Disable Reference

                color = new Color(0f, 0f, 1f, 1f);
                meshRenderer = i.GetComponent<MeshRenderer>();
                meshRenderer.material.SetColor("_BaseColor", color);
            }
        }
    }

    public void ExitInteract(Parameters parameters) {
        interactName = parameters.GetStringExtra(INTERACT_NAME, "None");

        foreach(GameObject i in eventList) {
            if(i.name == interactName) {
                activeList[i.name] = false;
            }
        }

        foreach(GameObject i in pressList) {
            if(i.name == interactName) {
                activeList[i.name] = false;

                //Change Color
                color = new Color(0.541f, 0.541f, 0.541f, 1f);
                meshRenderer = i.GetComponent<MeshRenderer>();
                meshRenderer.material.SetColor("_BaseColor", color);
            }
        }

        foreach(GameObject i in holdList) {
            if(i.name == interactName) {
                activeList[i.name] = false;

                //Change Color
                color = new Color(0.541f, 0.541f, 0.541f, 1f);
                meshRenderer = i.GetComponent<MeshRenderer>();
                meshRenderer.material.SetColor("_BaseColor", color);

            }
        }

        foreach(GameObject i in toggleList) {
            if(i.name == interactName) {
                activeList[i.name] = false;

                //Change Color
                color = new Color(0.541f, 0.541f, 0.541f, 1f);
                meshRenderer = i.GetComponent<MeshRenderer>();
                meshRenderer.material.SetColor("_BaseColor", color);
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

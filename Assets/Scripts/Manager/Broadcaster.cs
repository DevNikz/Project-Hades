using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broadcaster : MonoBehaviour
{
    public static Broadcaster Instance;

    private void Awake() {
        if(Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    public void AddIntParam(string stateName, string eventName, int Value) {
        Parameters tempParam = new Parameters();
        tempParam.PutExtra(stateName, Value);
        EventBroadcaster.Instance.PostEvent(eventName, tempParam);
    }

    public void AddBoolParam(string stateName, string eventName, bool Value) {
        Parameters tempParam = new Parameters();
        tempParam.PutExtra(stateName, Value);
        EventBroadcaster.Instance.PostEvent(eventName, tempParam);
    }

    public void AddStringParam(string stateName, string eventName, string Value) {
        Parameters tempParam = new Parameters(); 
        tempParam.PutExtra(stateName, Value);
        EventBroadcaster.Instance.PostEvent(eventName, tempParam);
    }
    
    public void AddFloatParam(string stateName, string eventName, float Value) {
        Parameters tempParam = new Parameters();
        tempParam.PutExtra(stateName, Value);
        EventBroadcaster.Instance.PostEvent(eventName, tempParam);
    }

    public void AddVectorParam(string stateName, string eventName, Vector3 Value) {
        Parameters tempParam = new Parameters();
        tempParam.PutExtra(stateName, Value);
        EventBroadcaster.Instance.PostEvent(eventName, tempParam);
    }

    public void AddVectorBParam(string stateName, string stateName2, string eventName, Vector3 Value, bool Value2) {
        Parameters tempParam = new Parameters();
        tempParam.PutExtra(stateName, Value);
        tempParam.PutExtra(stateName2, Value2);
        EventBroadcaster.Instance.PostEvent(eventName, tempParam);
    }
}

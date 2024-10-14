using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SingletonDoNotDestroy : MonoBehaviour
{
    private string ObjectName;
    public static Dictionary<string, GameObject> SingletonObjects = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        this.ObjectName = this.gameObject.name;

        if(!SingletonDoNotDestroy.SingletonObjects.ContainsKey(ObjectName)){
            SingletonObjects.Add(this.ObjectName, this.gameObject);
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        } 
    }

    void OnDestroy()
    {
        SingletonDoNotDestroy.SingletonObjects.Remove(ObjectName);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        // if(scene.buildIndex == 0) {
        //     Destroy(gameObject);
        // }
    }

     void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnDestroy()
    {
        if(SingletonDoNotDestroy.SingletonObjects.ContainsKey(this.ObjectName)
            && SingletonDoNotDestroy.SingletonObjects[this.ObjectName] == this.gameObject){
            SingletonDoNotDestroy.SingletonObjects.Remove(ObjectName);
        }
    }
}

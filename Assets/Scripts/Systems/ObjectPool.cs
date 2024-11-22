using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField, AssetSelector] private GameObject poolObject;
    [SerializeField] private int poolSize;
    
    [SerializeField, HideInInspector] private List<GameObject> availableObjects = new List<GameObject>();
    [SerializeField, HideInInspector] private List<GameObject> releasedObjects = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++){
            GameObject obj = Instantiate(poolObject);
            obj.SetActive(false);
            this.availableObjects.Add(obj);
        }
    }

    public void ReturnObject(GameObject obj){
        if(!this.releasedObjects.Contains(obj)) return;

        obj.SetActive(false);

        this.releasedObjects.Remove(obj);
        this.availableObjects.Add(obj);
    }

    public GameObject ReleaseObject(){
        if(this.availableObjects.Count <= 0) return null;

        GameObject obj = this.availableObjects[this.availableObjects.Count - 1];
        obj.SetActive(true);

        this.availableObjects.Remove(obj);
        this.releasedObjects.Add(obj);

        return obj;  
    }

    public GameObject ReleaseObjectAt(Vector3 location){
        GameObject obj = ReleaseObject();
        if(obj != null)
            obj.transform.position = location;
        return obj;
    }

    public GameObject ReleaseObjectAt(Transform transform){
        GameObject obj = ReleaseObject();
        if(obj != null)
            obj.transform.SetLocalPositionAndRotation(transform.position, transform.rotation);
        return obj;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

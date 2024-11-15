using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject poolObject;
    [SerializeField] private int poolSize;
    
    [SerializeField, HideInInspector] private List<GameObject> availableObjects = new List<GameObject>();
    [SerializeField, HideInInspector] private List<GameObject> releasedObjects = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++){
            GameObject gameObject = Instantiate(poolObject);
            gameObject.SetActive(false);
            this.availableObjects.Add(gameObject);
        }
    }

    public void ReturnObject(GameObject gameObject){
        if(!this.releasedObjects.Contains(gameObject)) return;

        this.gameObject.SetActive(false);

        this.releasedObjects.Remove(gameObject);
        this.availableObjects.Add(gameObject);
    }

    public GameObject ReleaseObject(){
        if(this.availableObjects.Count <= 0) return null;

        GameObject gameObject = this.availableObjects[this.availableObjects.Count - 1];
        this.gameObject.SetActive(true);

        this.availableObjects.Remove(gameObject);
        this.releasedObjects.Add(gameObject);

        return gameObject;  
    }

    public GameObject ReleaseObjectAt(Vector3 location){
        GameObject gameObject = ReleaseObject();
        if(gameObject != null)
            gameObject.transform.position = location;
        return gameObject;
    }

    public GameObject ReleaseObjectAt(Transform transform){
        GameObject gameObject = ReleaseObject();
        if(gameObject != null)
            gameObject.transform.SetLocalPositionAndRotation(transform.position, transform.rotation);
        return gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

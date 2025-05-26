using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField, AssetSelector] private GameObject poolObject;
    [SerializeField] private int poolSize;
    [SerializeField] private bool _makeParent = true;
    
    [SerializeField, HideInInspector] private List<GameObject> availableObjects = new List<GameObject>();
    [SerializeField, HideInInspector] private List<GameObject> releasedObjects = new List<GameObject>();

    public int ReleasedCount { get { return releasedObjects.Count; } }
    public int RemainingCount { get { return availableObjects.Count;}}
    public int TotalCount { get { return poolSize;}}

    void Start()
    {
        for (int i = 0; i < poolSize; i++){
            GameObject obj = Instantiate(poolObject);
            if(_makeParent)
                obj.transform.SetParent(transform);
            obj.SetActive(false);
            this.availableObjects.Add(obj);
        }
    }

    public void InitializePool(){
        ResetPool();
        for (int i = 0; i < poolSize; i++){
            GameObject obj = Instantiate(poolObject);
            if(_makeParent)
                obj.transform.SetParent(transform);
            obj.SetActive(false);
            this.availableObjects.Add(obj);
        }
    }

    public void InitializePool(int poolSize, GameObject poolObject = null){
        if(poolObject != null)
            this.poolObject = poolObject;
        this.poolSize = poolSize;
        InitializePool();
    }

    public void ResetPool(){
        List<GameObject> toRelease = new List<GameObject>();
        foreach(GameObject obj in releasedObjects){
            toRelease.Add(obj);
        }
        foreach(GameObject obj in toRelease){
            ReturnObject(obj);
        }
        toRelease.Clear();

        foreach(GameObject obj in availableObjects){
            toRelease.Add(obj);
        }
        foreach (GameObject obj in toRelease){
            availableObjects.Remove(obj);
            GameObject.Destroy(obj);
        }
        toRelease.Clear();
        availableObjects.Clear();
    }

    public void ReturnObject(GameObject obj){
        if(!this.releasedObjects.Contains(obj)) return;

        obj.SetActive(false);

        this.releasedObjects.Remove(obj);
        this.availableObjects.Add(obj);
    }
    public void ReturnAllObjects()
    {
        foreach (GameObject obj in releasedObjects)
            ReturnObject(obj);
    }

    public GameObject ReleaseObject()
    {
        if (this.availableObjects.Count <= 0) return null;

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Maintains a pool of reusable game objects. Attach this to a game object as a component and add reference to a poolable game object
/// By: NeilDG
/// </summary>
public class GameObjectPool : MonoBehaviour {

	[SerializeField] private APoolable poolableObjectCopy; //the poolable object copy
	[SerializeField] private Transform poolableParent; //where the poolable object will spawn in the hierarchy
	[SerializeField] private int maxPoolSize = 20; //the maxinum number of allowable reusable objects
	[SerializeField] private bool fixedAllocation = true; //if TRUE, number of poolable objects instantiated is fixed. User cannot create poolable objects during run-time.

	[SerializeField] private List<APoolable> availableObjects = new List<APoolable> ();
	[SerializeField] private List<APoolable> usedObjects = new List<APoolable>();

	// Use this for initialization
	void Start () {
		this.poolableObjectCopy.gameObject.SetActive (false); //hide the poolable object copy
	}

	public void Initialize() {
		for (int i = 0; i < this.maxPoolSize; i++) {
			APoolable poolableObject = GameObject.Instantiate<APoolable> (this.poolableObjectCopy, this.poolableParent);
			poolableObject.Initialize ();
			poolableObject.gameObject.SetActive (false);
			this.availableObjects.Add (poolableObject);
		}
	}

	public bool HasObjectAvailable(int requestSize) {
		return this.availableObjects.Count >= requestSize;
	}

	public APoolable RequestPoolable() {
		if (this.HasObjectAvailable (1)) {
			APoolable poolableObject = this.availableObjects [this.availableObjects.Count - 1];
			poolableObject.SetPoolRef (this);
			this.availableObjects.RemoveAt (this.availableObjects.Count - 1);
			this.usedObjects.Add (poolableObject);

			poolableObject.gameObject.SetActive (true);
			poolableObject.OnActivate ();
			return poolableObject;
		} else {
			Debug.LogError ("[GameObjectPool] No more poolable object available!");
			return null;
		}
	}

	public APoolable[] RequestPoolableBatch(int size) {
		if (this.HasObjectAvailable(size)) {
			APoolable[] poolableObjects = new APoolable[size];

			for (int i = 0; i < size; i++) {
				poolableObjects [i] = this.RequestPoolable ();
			}

			return poolableObjects;
		} else {
			Debug.LogError ("[GameObjectPool] Insufficient objects available in pool. Count is: " + this.availableObjects.Count + " while requested is " + size + "!");
			return null;
		}
	}

	public void ReleasePoolable(APoolable poolableObject) {
		this.usedObjects.Remove (poolableObject);

		poolableObject.Release ();
		poolableObject.gameObject.SetActive (false);
		this.availableObjects.Add (poolableObject);
	}

}

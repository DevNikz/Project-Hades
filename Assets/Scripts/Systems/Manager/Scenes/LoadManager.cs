using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Convenience function for loading a new scene while cleaning up the existing scene
/// </summary>
public class LoadManager {
	private static LoadManager sharedInstance = null;
	public static LoadManager Instance {
		get {
			if (sharedInstance == null) {
				sharedInstance = new LoadManager ();
			}

			return sharedInstance;
		}
	}

	/// <summary>
	/// Loads a new scene
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="clearEventBroadcast">If set to <c>true</c> clears all attached observers and events in the broadcast system.</param>
	public void LoadScene(string name, bool clearEventBroadcast = true) {
		if (clearEventBroadcast) {
			EventBroadcaster.Instance.RemoveAllObservers (); //remove all observers associated with the scene
		}

		SceneManager.LoadScene (name);

	}

	/// <summary>
	/// Loads a new scene but does not destroy the old scene. Game objects will be added.
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="clearEventBroadcast">If set to <c>true</c> clears all attached observers and events in the broadcast system.</param>
	public void LoadSceneAdditive(string name, bool clearEventBroadcast = true) {
		if (clearEventBroadcast) {
			EventBroadcaster.Instance.RemoveAllObservers (); //remove all observers associated with the scene
		}

		SceneManager.LoadScene (name, LoadSceneMode.Additive);
	}
}

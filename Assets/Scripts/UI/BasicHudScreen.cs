﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicHudScreen : View {

	// Use this for initialization
	void Start () {
		
	}

	public void OnMainMenuClicked() {
		LoadManager.Instance.LoadScene(SceneNames.Names[(int)SceneNames.Enums.MAIN_SCENE]);
	}

}

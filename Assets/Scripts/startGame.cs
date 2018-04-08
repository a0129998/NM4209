using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class startGame : MonoBehaviour {
	public Button startGameBtn;
	// Use this for initialization
	void Start () {
		startGameBtn.onClick.AddListener (startMain);
	}

	void startMain(){
		SceneManager.LoadSceneAsync (1);
	}
}

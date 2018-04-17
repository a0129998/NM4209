using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class startGame : MonoBehaviour {
	public Button startGameBtn;
	public GameObject loadingPage;
	// Use this for initialization
	void Start () {
		loadingPage.gameObject.SetActive (false);
		startGameBtn.onClick.AddListener (startMain);
	}

	void startMain(){
		loadingPage.gameObject.SetActive (true);
		SceneManager.LoadSceneAsync (1);
	}
}

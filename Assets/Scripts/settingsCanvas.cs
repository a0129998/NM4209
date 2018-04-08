using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class settingsCanvas : MonoBehaviour {
	public Button goToStartBtn;
	// Use this for initialization
	void Start () {
		goToStartBtn.onClick.AddListener (goToStartMenu);
	}
	void goToStartMenu(){
		Debug.Log ("go to start screen");
		SceneManager.LoadSceneAsync ("Scenes/startScene");
	}
}

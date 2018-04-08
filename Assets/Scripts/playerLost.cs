using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class playerLost : MonoBehaviour {
	public Button restartBtn;
	public Text lostText;
	// Use this for initialization
	void Start () {
		lostText.text = "You Lost \n" + "Your Score: " + moveScene.score;
		restartBtn.onClick.AddListener (restart);
	}

	void restart(){
		SceneManager.LoadSceneAsync ("Scenes/startScene");
	}
}

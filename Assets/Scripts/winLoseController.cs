using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class winLoseController : MonoBehaviour {

	public GameObject gameManager;
	private GameManager gm;

	public Image loseImg;
	public Image winImg;
	public Text winLoseText;
	public Button startScreenBtn;

	// Use this for initialization
	void Start () {
		gm = gameManager.GetComponent<GameManager> ();
		setEnableWinLose (false);
		startScreenBtn.onClick.AddListener (restart);
		loseImg.enabled = false;
		winImg.enabled = false;
	}

	void restart(){
		SceneManager.LoadSceneAsync ("Scenes/startScene");
	}

	void setEnableWinLose(bool isEnabled){
		winLoseText.enabled = isEnabled;
		startScreenBtn.gameObject.SetActive (isEnabled);
	}
	
	// Update is called once per frame
	void Update () {
		if (gm.winCondition) {
			//playerwon, do something
			winImg.enabled = true;
			setEnableWinLose(true);
			winLoseText.text = "You Win! \n Score: " + gm.score.text;
		}else if(gm.loseCondition){
			//player lost, do sth
			loseImg.enabled = true;
			setEnableWinLose(true);
			winLoseText.text = "Try again? \n Score: " + gm.score.text;
		}
	}
}

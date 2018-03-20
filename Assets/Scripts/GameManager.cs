using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	private static GameManager _instance;
	public static GameManager Instance { get { return _instance; } }

	private void Awake(){
		if (_instance != null && _instance != this) {
			Destroy (this.gameObject);
		} else {
			_instance = this;
		}
	}

	public GameObject player;
	private PlayerControler p;
	private Vector2 playerLocation;
	public GameObject[] enemnies;
	public int maxLevel;
	private int currLevel;
	public SpawnEnemies sE;
	private menuScript mS;

	public float left;
	public float right;
	public float up;
	public float down;

	//display
	public Text gold;
	public Text time;
	public Text Hp;
	public Text metalOre;

	public bool isPaused;

	//wait 5 secs
	float betweenLevelsWaitingTime;
	bool isBetweenLevels;


	//menu
	public Button menuBtn;
	public Canvas menuCanvas;
	public Canvas usualCanvas;
	public Button unpauseBtn;
	public Button goldToHealth;
	public Button goldToTime;
	public Button goldToOre;
	public Button blackSmithButton;
	public Canvas blackSmithCanvas;
	public Button blackSmithBack;

	//blacksmithoptions


	// Use this for initialization
	void Start () {
		playerLocation = player.transform.position;
		p = player.GetComponent<PlayerControler> ();
		currLevel = 0;
		initialise (0);
		isBetweenLevels = false;
		betweenLevelsWaitingTime = 0.0f;
		mS = menuCanvas.GetComponent<menuScript> ();

		menuBtn.onClick.AddListener (showMenu);
		unpauseBtn.onClick.AddListener (unpause);
		goldToHealth.onClick.AddListener (changeGoldToHealth);
		goldToTime.onClick.AddListener (changeGoldToTime);
		blackSmithButton.onClick.AddListener (blackSmith);
		blackSmithBack.onClick.AddListener (blackSmithBackRun);
		goldToOre.onClick.AddListener (buyOre);
		menuCanvas.enabled = false;
		blackSmithCanvas.enabled = false;

	}
	public void buyOre(){
		int oreTOBuy = mS.getOre ();
		if (oreTOBuy * 10 <= p.getGold()) {
			p.buyOre (oreTOBuy);
			p.removeGold (10 * oreTOBuy);
			mS.resetOreMenu ();
		}
	}
	public int checkOre(){
		return p.metalOre;
	}
	public void blackSmith(){
		blackSmithCanvas.enabled = true;
		menuCanvas.enabled = false;
		usualCanvas.enabled = false;
	}
	public void blackSmithBackRun(){
		blackSmithCanvas.enabled = false;
		usualCanvas.enabled = true;
	}

	void showMenu(){
		pauseGame ();
		//usualCanvas.enabled = false;
		menuBtn.enabled = false;
		menuCanvas.enabled = true;
	}

	public void pauseGame(){
		p.pause ();
		EnemyControler.isPaused = true;
		isPaused = true;
	}

	public void unpause(){
		p.isPaused = false;
		isPaused = false;
		EnemyControler.isPaused = false;
		//usualCanvas.enabled = true;
		menuBtn.enabled = true;
		menuCanvas.enabled = false;
	}


	
	// Update is called once per frame
	public void setWaitingTime(){
		betweenLevelsWaitingTime = 5.0f;
		p.waiting = true;
	}
	void Update () {
		gold.text = "Gold: " + p.getGold();
		time.text = "Time Left: " + p.getTimeLeft();
		Hp.text = "HP: " + p.getHP () + "/" + p.getHPTotal ();
		metalOre.text = "Ore: " + p.getOre ();
		if (isPaused) {
			return;
		}
		if (betweenLevelsWaitingTime > 0) {
			betweenLevelsWaitingTime -= Time.deltaTime;
			isBetweenLevels = true;

		} else if (isBetweenLevels) {
			initialiseNext ();
			isBetweenLevels = false;
			p.waiting = false;
		}
		playerLocation = player.transform.position;
		//display

	}

	public PlayerControler getpC(){
		return p;
	}

	public Vector3 getPlayerLocation(){
		return playerLocation;
	}

	private void initialise(int level){
		//hard code level 1
		switch (level){
		case 0:
			p.initialisePlayer (40, 40.0f, 1, 1, 1, 0);
			sE.initSpawn (new int[5]{ 0, 0, 0, 0, 0 }, new float[5]{ 1.0f, 1.0f, 1.0f, 1.0f, 1.0f });
			break;
		case 1:
			p.initPlayerAddtitional (40);
			sE.initSpawn (new int[4]{ 1, 1, 1, 1 }, new float[4]{ 5.0f, 5.0f, 5.0f, 5.0f});
			break;
		case 2:
			p.initPlayerAddtitional (40);
			sE.initSpawn (new int[3]{ 2, 2, 2}, new float[3]{ 1.0f, 1.0f, 1.0f});
			break;
		case 3:
			p.initPlayerAddtitional (60);
			sE.initSpawn (new int[4]{ 0, 0, 0, 0 }, new float[4]{ 1.0f, 2.0f, 1.0f, 2.0f});
			break;
		case 4:
			p.initPlayerAddtitional (60);
			sE.initSpawn (new int[5]{ 0, 1, 0, 1, 0 }, new float[5]{ 1.0f, 5.0f, 1.0f, 10.0f, 1.0f});
			break;
		}

	}

	public void initialiseNext(){
		if (++currLevel <= maxLevel) {
			Debug.Log ("init level " + currLevel);
			initialise (currLevel);	
		} else {
			Debug.Log ("you won");
		}
	}

	public void changeGoldToHealth(){
		int toBuy = mS.hpToBuy ();
		//check if legal
		if (p.getGold () >= toBuy) {
			p.removeGold (toBuy);
			p.addHPCurrent (toBuy);
			mS.resethealth ();
		}
	}

	public void changeGoldToTime(){
		int timeToBuy = mS.timeToBuy ();
		if (timeToBuy <= p.getGold()) {
			p.removeGold (timeToBuy);
			p.addTime (timeToBuy);
			mS.resetTimeMenu ();
		}
	}
}

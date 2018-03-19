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

	public float left;
	public float right;
	public float up;
	public float down;

	//display
	public Text gold;
	public Text time;
	public Text Hp;

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

	// Use this for initialization
	void Start () {
		playerLocation = player.transform.position;
		p = player.GetComponent<PlayerControler> ();
		currLevel = 0;
		initialise (0);
		isBetweenLevels = false;
		betweenLevelsWaitingTime = 0.0f;

		menuBtn.onClick.AddListener (showMenu);
		unpauseBtn.onClick.AddListener (unpause);
		goldToHealth.onClick.AddListener (changeGoldToHealth);
		goldToTime.onClick.AddListener (changeGoldToTime);
		menuCanvas.enabled = false;

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
		p.removeGold (1);
		p.addHPCurrent (1);
	}

	public void changeGoldToTime(){
		p.removeGold (1);
		p.addTime (1);
	}
}

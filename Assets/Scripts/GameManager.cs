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

	public static bool debug = true;
	public static bool enemiesMove = true;

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
	public Text ore;
	public Image healthBar;
	public Text time;

    public bool isPaused;

    //wait 5 secs
    float betweenLevelsWaitingTime;
    bool isBetweenLevels;


    //menu
    public Button menuBtn;
    public Canvas menuCanvas;
    public Canvas usualCanvas;
    public Button goldToOre;
    public Button blackSmithButton;
    public Canvas blackSmithCanvas;
	public Button pauseBtn;

	public Text progressMsgs;
	private Color originalProgressMsgsColor;

	IEnumerator fadeSlowly(Text t){
		float totalFadeTime = 2.0f;
		Color originalColour = t.color;
		while (totalFadeTime > 0) {
			totalFadeTime -= Time.deltaTime;
			yield return new WaitForFixedUpdate ();
			t.color = Color.Lerp (originalColour, Color.clear, 1.0f - totalFadeTime);

		}
		t.color = Color.clear;
		yield return null;
	}

    // Use this for initialization
    void Start () {
        playerLocation = player.transform.position;
        p = player.GetComponent<PlayerControler> ();
        currLevel = 0;
        
        isBetweenLevels = false;
        betweenLevelsWaitingTime = 0.0f;
        mS = menuCanvas.GetComponent<menuScript> ();

        menuBtn.onClick.AddListener (showMenu);
        blackSmithButton.onClick.AddListener (blackSmith);
        goldToOre.onClick.AddListener (buyOre);
		pauseBtn.onClick.AddListener (pauseResume);
        menuCanvas.enabled = false;
        blackSmithCanvas.enabled = false;
		originalProgressMsgsColor = progressMsgs.color;
		progressMsgs.color = Color.clear;

		initialise (0);

    }
	public void pauseResume(){
		if (isPaused) {
			unpause2 ();
		} else {
			pauseGame ();
		}
	}
    public void buyOre(){
        int oreTOBuy = mS.getOre ();
		if (oreTOBuy * 10 <= p.gold && oreTOBuy > 0) {
            p.buyOre (oreTOBuy);
            p.removeGold (10 * oreTOBuy);
            mS.resetOreMenu ();
		} else {
			//mS.warnNotEnoughGold ();
		}
    }
    public int checkOre(){
        return p.metalOre;
    }
    public void blackSmith(){
		pauseGame ();
        blackSmithCanvas.enabled = true;
        menuCanvas.enabled = false;
    }
    public void blackSmithBackRun(){
        blackSmithCanvas.enabled = false;
        usualCanvas.enabled = true;
		unpause2 ();
    }

    void showMenu(){
        pauseGame ();
        menuBtn.enabled = false;
        menuCanvas.enabled = true;
    }

    public void pauseGame(){
		p.isPlayerPaused = true;
        EnemyControler.isPaused = true;
        isPaused = true;
    }

    public void unpause(){
		p.isPlayerPaused = false;
        isPaused = false;
        EnemyControler.isPaused = false;
        //usualCanvas.enabled = true;
        menuBtn.enabled = true;
        menuCanvas.enabled = false;
    }
	public void unpause2(){
		p.isPlayerPaused = false;
		isPaused = false;
		EnemyControler.isPaused = false;
	}

    public void setWaitingTime(){
        betweenLevelsWaitingTime = 5.0f;
		p.isTimePaused = true;
		useProgressMsgs ("CurrentLevelOver");
    }

	public void useProgressMsgs(string str){
		progressMsgs.color = originalProgressMsgsColor;
		progressMsgs.text = str;
		StartCoroutine (fadeSlowly (progressMsgs));
	}
    void Update () {
		if (p.waveTimeLeft < 0) {
			//gameover
			useProgressMsgs("Time Ran Out");
			pauseGame ();
		}
		gold.text = p.gold.ToString();
		ore.text = p.metalOre.ToString();
		time.text = "Time Left: " + Mathf.Ceil(p.waveTimeLeft);
		if (Input.GetKeyDown (KeyCode.Escape)) {
			unpause ();
		}
        if (isPaused) {
            return;
        }
        if (betweenLevelsWaitingTime > 0) {
            betweenLevelsWaitingTime -= Time.deltaTime;
            isBetweenLevels = true;

        } else if (isBetweenLevels) {
            initialiseNext ();
            isBetweenLevels = false;
			p.isTimePaused = false;

        }
        playerLocation = player.transform.position;
        //display
		if(debug){
			if (Input.GetKeyDown (KeyCode.O)) {
				this.p.metalOre += 10;
			}
		}
    }

    public PlayerControler getpC(){
        return p;
    }

    public Vector3 getPlayerLocation(){
        return playerLocation;
    }

    private void initialise(int level){
        //hard code level 1
		useProgressMsgs ("Level " + level + " Start!");
		level = level % 11;
        switch (level){
		case 0:
			p.waveTimeLeft = 40;
			sE.initSpawn (new int[5]{ 0, 1, 2, 0, 0 }, new float[5]{ 1.0f, 1.0f, 1.0f, 1.0f, 1.0f }, new int[5]{1,1,1,1,1});
            break;
        case 1:
			p.waveTimeLeft = 40.0f;
			sE.initSpawn (new int[4]{ 1, 1, 1, 1 }, new float[4]{ 5.0f, 5.0f, 5.0f, 5.0f}, new int[5]{5,5,5,5,5});
            break;
		case 2:
			p.waveTimeLeft = 40.0f;
			sE.initSpawn (new int[3]{ 2, 2, 2}, new float[3]{ 1.0f, 1.0f, 1.0f}, new int[3]{1,1,1});
            break;
		case 3:
			p.waveTimeLeft = 60.0f;
			sE.initSpawn (new int[4]{ 0, 0, 0, 0 }, new float[4]{ 1.0f, 2.0f, 1.0f, 2.0f}, new int[4]{1,2,1,2});
            break;
		case 4:
			p.waveTimeLeft = 60.0f;
			sE.initSpawn (new int[5]{ 0, 1, 0, 1, 0 }, new float[5]{ 1.0f, 5.0f, 1.0f, 10.0f, 1.0f}, new int[5]{1,5,1,10,1});
            break;
		case 5:
			p.waveTimeLeft = 60.0f;
			sE.initSpawn (new int[5]{ 0, 1, 0, 1, 0 }, new float[5]{ 1.0f, 5.0f, 1.0f, 10.0f, 1.0f }, new int[5]{1,2,1,10,1});
			break;
		case 6:
			p.waveTimeLeft = 60.0f;
			sE.initSpawn (new int[4]{ 0, 2, 0, 2}, new float[4]{ 1.0f, 1.0f, 1.0f, 2.0f }, new int[4]{1,1,1,2});
			break;
		case 7:
			p.waveTimeLeft = 60.0f;
			sE.initSpawn (new int[4]{ 1, 2, 1, 1}, new float[4]{ 1.0f, 2.0f, 1.0f, 1.0f }, new int[4]{1,2,1,1});
			break;
		case 8:
			p.waveTimeLeft = 60.0f;
			sE.initSpawn (new int[4]{ 1, 1, 1, 2}, new float[4]{ 1.0f, 2.0f, 1.0f, 1.0f }, new int[4]{5,10,5,5});
			break;
		case 9:
			p.waveTimeLeft = 60.0f;
			sE.initSpawn (new int[4]{ 2, 2, 2, 2}, new float[4]{ 1.0f, 2.0f, 1.0f, 1.0f }, new int[4]{1,2,1,1});
			break;
		case 10:
			p.waveTimeLeft = 60.0f;
			sE.initSpawn (new int[6]{ 0, 1, 2, 2, 2, 2}, new float[6]{ 2.0f, 10.0f, 2.0f, 1.0f, 5.0f, 1 }, new int[]{2,10,2,1,5,1});
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

}

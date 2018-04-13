using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {


	public static bool debug = true;
	public static bool enemiesMove = true;

    public GameObject player;
    private PlayerControler p;
    private Vector2 playerLocation;
    public GameObject[] enemnies;
    public int maxLevel;
	public int currLevel;
    public SpawnEnemies sE;
    private menuScript mS;

    public float left;
    public float right;
    public float up;
    public float down;

    //display
	public Text gold;
	public Text ore;
	public Text time;
	public Text timeMSecText;
	public Text score;

    public bool isPaused;

    //wait 5 secs
    public float betweenLevelsWaitingTime;
    bool isBetweenLevels;


    //menu
    public Canvas menuCanvas;
    public Canvas usualCanvas;
    public Button goldToOre;
    public Canvas blackSmithCanvas;
	public Button pauseBtn;

	public Text progressMsgs;
	private Color originalProgressMsgsColor;

	public Button blackSmithTabBtn;
	public Button blackSmithTabBtn2;
	public Button shopTabBtn;
	public Button shopTabBtn2;

	public bool winCondition;
	public bool loseCondition;

	public Button closeMenuBtn;
	public Button closeBlackSmithBtn;
	public Canvas settingsCanvas;
	public Button settingsCanvasTab;
	public Button settingsCanvasTab2;
	public Button closeSettingsCanvasBtn;

	public bool isInfinite;

	public AudioSource openMenuSound;
	public AudioSource closeMenuSound;

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

        goldToOre.onClick.AddListener (buyOre);
		pauseBtn.onClick.AddListener (shopTab);
        menuCanvas.enabled = false;
        blackSmithCanvas.enabled = false;
		settingsCanvas.enabled = false;
		originalProgressMsgsColor = progressMsgs.color;
		progressMsgs.color = Color.clear;

		shopTabBtn.onClick.AddListener (shopTab);
		shopTabBtn2.onClick.AddListener (shopTab);
		blackSmithTabBtn.onClick.AddListener (blackSmithTab);
		blackSmithTabBtn2.onClick.AddListener (blackSmithTab);
		winCondition = false;
		loseCondition = false;
		closeMenuBtn.onClick.AddListener (unpause);
		closeBlackSmithBtn.onClick.AddListener (blackSmithBackRun);
		closeSettingsCanvasBtn.onClick.AddListener (closeAllCanvas);
		settingsCanvasTab.onClick.AddListener (openSettings);
		settingsCanvasTab2.onClick.AddListener (openSettings);
		isPaused = false;
		initialise (0);

    }


	public void openSettings(){
		if (blackSmithCanvas.enabled) {
			blackSmithBackRun ();
		}
		if (menuCanvas.enabled) {
			unpause ();
		}
		pauseGame ();
		settingsCanvas.enabled = true;
	}

	public void closeSettings(){
		settingsCanvas.enabled = false;
		unpause2 ();
	}

	public void closeAllCanvas(){
		if (blackSmithCanvas.enabled) {
			blackSmithBackRun ();
		}

		if (menuCanvas.enabled) {
			unpause ();
		}

		if (settingsCanvas.enabled) {
			closeSettings ();
		}
	}

	public void shopTab(){
		//openMenu.Play ();
		blackSmithBackRun ();
		if (settingsCanvas.enabled) {
			closeSettings ();
		}
		showMenu ();
	}
	public void blackSmithTab(){
		//openMenu.Play ();
		if (settingsCanvas.enabled) {
			closeSettings ();
		}
		unpause ();
		blackSmith ();
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
		if (((oreTOBuy * mS.goldOreExchangeRate) <= p.gold) && (oreTOBuy > 0)) {
			p.buyOre (oreTOBuy, mS.goldOreExchangeRate);
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
        menuCanvas.enabled = true;
    }

    public void pauseGame(){
		p.isPlayerPaused = true;
        EnemyControler.isPaused = true;
        isPaused = true;
    }

    public void unpause(){
		if (settingsCanvas.enabled) {
			closeSettings ();
		}
		//Debug.Log ("tries to unpause");
		p.isPlayerPaused = false;
        isPaused = false;
        EnemyControler.isPaused = false;
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
		useProgressMsgs ("Current Level Cleared!");
    }

	public void useProgressMsgs(string str){
		progressMsgs.color = originalProgressMsgsColor;
		progressMsgs.text = str;
		StartCoroutine (fadeSlowly (progressMsgs));
	}
    void Update () {
		if (winCondition) {
			pauseGame ();
			//go to win page- should have replay option
		}
		if (p.waveTimeLeft < 0) {
			//gameover
			useProgressMsgs("Time Ran Out");
			pauseGame ();
			loseCondition = true;
		}
		if (!p.isPlayerAlive) {
			loseCondition = true;
		}
		gold.text = p.gold.ToString();
		ore.text = p.metalOre.ToString();
		timeDisplay ();
		score.text = p.playerScore.ToString();
		if (Input.GetKeyDown (KeyCode.Escape) && menuCanvas.enabled) {//close menu
			Debug.Log ("close menu");
			closeMenuSound.PlayOneShot (closeMenuSound.clip);
			unpause ();
		} else if (Input.GetKeyDown (KeyCode.Escape) && (!blackSmithCanvas.enabled && !menuCanvas.enabled && !settingsCanvas.enabled)) {
			Debug.Log ("show menu");
			openMenuSound.PlayOneShot (openMenuSound.clip);
			showMenu ();
		}

		if (Input.GetKeyDown (KeyCode.Escape) && settingsCanvas.enabled) {
			Debug.Log("close settings");
			closeMenuSound.PlayOneShot (closeMenuSound.clip);
			closeSettings ();
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

		moveScene.score = p.playerScore;

        //display
		if(debug){
			if (Input.GetKeyDown (KeyCode.O)) {
				this.p.metalOre += 10;
			}
			if (Input.GetKeyDown (KeyCode.Escape)) {//close menu
				Debug.Log(menuCanvas.enabled);
				Debug.Log (blackSmithCanvas.enabled);
			}
		}
    }

	void timeDisplay(){
		string timeForSec = Mathf.Ceil (p.waveTimeLeft).ToString();
		string timeForMs = (Mathf.Ceil( p.waveTimeLeft * 100 % 100)).ToString ("F0");

		time.text = timeForSec;
		timeMSecText.text = timeForMs;
	}

    public PlayerControler getpC(){
        return p;
    }

    public Vector3 getPlayerLocation(){
        return playerLocation;
    }

	int getEffectiveLevel(int level){
		if (level > 9) {//loop levels 5 to 9
			return (level - 5) % 5 + 5;
		} else {
			return level;
		}
	}

	void setEnemyScaling(int level){
		float multiplier = Mathf.Floor (level / 5);
		float sF = 1.0f;
		for (int i = 0; i < multiplier; i++) {
			sF *= 1.25f;
		}
		EnemyControler.scalingFactor = sF;
	}

    private void initialise(int level){
        //hard code level 1
		if(winCondition){
			return;
		}
		setEnemyScaling (level);
		useProgressMsgs ("Wave " + level);
		if (isInfinite){
			level = getEffectiveLevel(level);
		}
        switch (level){
		case 0:
			p.waveTimeLeft += 30.0f;
			sE.initSpawn (new int[4]{0, 0, 0, 0}, new float[4]{ 1.0f, 4.0f, 4.0f, 4.0f}, new int[4]{1, 1, 1, 1});
            break;
        case 1:
			p.waveTimeLeft += 20.0f;
			sE.initSpawn (new int[4]{1, 1, 1, 1 }, new float[4]{1.0f, 4.0f, 4.0f, 4.0f}, new int[4]{5, 5, 5, 5});
            break;
		case 2:
			p.waveTimeLeft += 20.0f;
			sE.initSpawn (new int[3]{ 2, 2, 2}, new float[3]{ 1.0f, 7.0f, 7.0f}, new int[3]{1, 1, 2});
            break;
		case 3:
			p.waveTimeLeft += 20.0f;
			sE.initSpawn (new int[4]{0, 1, 2, 0}, new float[4]{1.0f, 7.0f, 7.0f, 7.0f}, new int[4]{1, 5, 1, 2});
            break;
		case 4:
			p.waveTimeLeft += 20.0f;
			sE.initSpawn (new int[4]{4, 1, 0, 3 }, new float[4]{1.0f, 10.0f, 4.0f, 4.0f}, new int[4]{1, 5, 2, 5});
            break;
		case 5:
			p.waveTimeLeft += 25.0f;
			sE.initSpawn (new int[5]{ 0, 0, 0, 0, 0}, new float[5]{ 1.0f, 5.0f, 5.0f, 5.0f, 5.0f }, new int[5]{2, 1, 2, 1, 2});
			break;
		case 6:
			p.waveTimeLeft += 25.0f;
			sE.initSpawn (new int[5]{1, 1, 1, 1, 1}, new float[5]{1.0f, 5.0f, 5.0f, 5.0f, 5.0f}, new int[5]{10, 5, 10, 5, 10});
			break;
		case 7:
			p.waveTimeLeft += 25.0f;
			sE.initSpawn (new int[5]{ 2, 0 , 2, 0 ,2}, new float[5]{1.0f, 5.0f, 5.0f, 5.0f, 5.0f }, new int[5]{1, 1, 1, 2, 2});
			break;
		case 8:
			p.waveTimeLeft += 25.0f;
			sE.initSpawn (new int[6]{ 0, 1, 0, 2, 1, 2}, new float[6]{1.0f, 1.0f, 10.0f, 1.0f, 10.0f, 1.0f}, new int[6]{2, 10, 3, 2, 10, 3});
			break;
		case 9:
			p.waveTimeLeft += 45.0f;
			sE.initSpawn (new int[6]{ 5, 3, 4, 5, 3, 4}, new float[6]{ 1.0f, 10.0f, 10.0f, 10.0f, 10.0f, 1.0f}, new int[6]{1, 5, 1, 1, 5, 1});
			break;
		case 10://not used
			p.waveTimeLeft += 60.0f;
			sE.initSpawn (new int[6]{ 5, 3, 5, 4, 5, 3}, new float[6]{ 1.0f, 10.0f, 10.0f, 10.0f, 10.0f, 1.0f }, new int[6]{1, 5, 1, 2, 1, 5});
			break;
        }

    }

    public void initialiseNext(){
		if (++currLevel <= maxLevel && !isInfinite) {
			Debug.Log ("init level " + currLevel);
			initialise (currLevel); 

		} else if (isInfinite) {
			Debug.Log ("init level " + currLevel);
			initialise (currLevel); 
		}else{
            Debug.Log ("you won");
			useProgressMsgs ("You Won!");
			winCondition = true;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyControler : MonoBehaviour {
	public static GameObject gameManager;
	public static bool isPaused;
	public int atk;
	public int hp;
	public int maxHp;
	public int goldDropped;
	public int regen;
	private static GameManager gm;
	private float s;//5sec regen

	public float speed;
	public GameObject healthBarObject;
	private Image healthBar;

	public GameObject coin;
	private float freeze;
	public float freezeTime;

	public GameObject vanishingText;
	public float startPauseTime;
	private float startPauseTimer;

	public bool spawnYoungWhenDead;
	public GameObject youngToSpawn;
	public int numYoungToSpawn;

	public float moveBackTimer;
	public float moveBackTime;

	public static float scalingFactor;

	// Use this for initialization
	void Start () {
		//scale
		maxHp = (int)Mathf.Ceil(maxHp * scalingFactor);
		atk = (int)Mathf.Ceil(atk * scalingFactor);


		gameManager = GameObject.Find ("GameManager");
		gm = gameManager.GetComponent<GameManager> ();
		s = 5.0f;
		this.hp = maxHp;
		healthBar = healthBarObject.GetComponent<Image> ();
		startPauseTimer = 0.0f;
		moveBackTimer = 0.0f;
		isPaused = false;
	}
	
	// Update is called once per frame
	void Update () {
		float effectiveSpeed = speed;
		if (startPauseTime > startPauseTimer) {
			startPauseTimer += Time.deltaTime;
			effectiveSpeed = Mathf.Lerp (0, speed, startPauseTimer/startPauseTime);
		}

		if (isPaused | !GameManager.enemiesMove) {
			return;
		}
		if (freeze > 0.0f) {
			freeze -= Time.deltaTime;
			return;
		}
		Vector3 playerLocation = gm.getPlayerLocation ();
		Vector3 direction = playerLocation - gameObject.transform.position;
		direction = direction.normalized;
		if (moveBackTimer > 0) {
			moveBackTimer -= Time.deltaTime;
			gameObject.transform.Translate (direction * effectiveSpeed * -1.0f);
		} else {
			gameObject.transform.Translate (direction * effectiveSpeed);
		}

		s -= Time.deltaTime;
		if (s < 0) {
			this.hp = Mathf.Min (hp + regen, maxHp);
			s = 5.0f;
		}

		healthBar.fillAmount = (float)hp / (float)maxHp;
	}

	public void dies(){
		Instantiate (coin, transform.position, Quaternion.identity);
		if (spawnYoungWhenDead) {
			for (int i = 0; i < numYoungToSpawn; i++) {
				SpawnEnemies.numEnemies++;//increase the number of enemies 
				Instantiate (youngToSpawn, transform.position, Quaternion.identity);
			}
		}

		Destroy (gameObject);
	}

	public void isHit(int hpToRed){//reduce
		freeze = freezeTime;
		this.hp -= hpToRed;

		GameObject vText = (GameObject)Instantiate(vanishingText, gameObject.transform.position, Quaternion.identity);
		vanishingNumbers vT = vText.GetComponentInChildren<vanishingNumbers> ();
		vT.text.text = "-" + hpToRed;
		vT.text.color = Color.white;
		vT.transform.position = gameObject.transform.position;
	}

	public int getDamage(){
		return atk;
	}

	public int getGoldDropped(){
		return goldDropped;
	}

	public int getHp(){
		return this.hp;
	}
}

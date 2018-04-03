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

	public GameObject vanishingText;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find ("GameManager");
		gm = gameManager.GetComponent<GameManager> ();
		s = 5.0f;
		this.hp = maxHp;
		healthBar = healthBarObject.GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
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
		gameObject.transform.Translate (direction * speed);

		s -= Time.deltaTime;
		if (s < 0) {
			this.hp = Mathf.Min (hp + regen, maxHp);
			s = 5.0f;
		}

		healthBar.fillAmount = (float)hp / (float)maxHp;
	}

	public void dies(){
		Instantiate (coin, transform.position, Quaternion.identity);
		Destroy (gameObject);
	}

	public void isHit(int hpToRed){//reduce
		freeze = 0.5f;
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

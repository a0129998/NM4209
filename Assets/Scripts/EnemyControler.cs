using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find ("GameManager");
		gm = gameManager.GetComponent<GameManager> ();
		s = 5.0f;
		this.hp = maxHp;
	}
	
	// Update is called once per frame
	void Update () {
		if (isPaused) {
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
	}

	public void isHit(int hpToRed){//reduce
		this.hp -= hpToRed;
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

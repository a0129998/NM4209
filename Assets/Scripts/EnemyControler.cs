using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour {
	public static GameObject gameManager;
	public static bool isPaused;
	public int damage;
	public int hp;
	public int goldDropped;
	private static GameManager gm;

	public float speed;
	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find ("GameManager");
		gm = gameManager.GetComponent<GameManager> ();
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
	}

	public void isHit(int hpToRed){//reduce
		hp -= hpToRed;
	}

	public int getDamage(){
		return damage;
	}

	public int getGoldDropped(){
		return goldDropped;
	}

	public int getHp(){
		return hp;
	}
}

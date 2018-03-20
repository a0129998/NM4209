using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControler : MonoBehaviour {
	public float speed;
	private bool alive;
	private int hpCurrent;
	public GameObject sword;
	private SwordControler sC;
	public int gold;
	public int metalOre;
	public bool waiting;
	public bool pauseTime;

	//stats
	private int atk;
	private int def;//currently unused
	public int regen;
	public int hpTotal;
	public float spdMultiplyer;
	private float timeLeft;
	public bool isPaused;

	// Use this for initialization
	public void initialisePlayer(int hpTotal, float timeLeft, int atk, int def, int speed, int gold){
		//initial
		this.sC = sword.GetComponent<SwordControler>();

		this.alive = true;
		this.hpTotal = hpTotal;
		this.hpCurrent = hpTotal;
		this.timeLeft = timeLeft;
		this.atk = atk;
		this.def = def;
		this.spdMultiplyer = speed;
		this.gold = gold;
		this.waiting = false;
		this.isPaused = false;
		this.metalOre = 0;
		this.regen = 0;
	}

	public int getOre(){
		return metalOre;
	}

	public void buyOre(int toBuy){
		int cost = toBuy * 10;
		if (cost <= gold) {
			gold -= cost; 
			StartCoroutine (aO (toBuy));
		}
	}
	IEnumerator aO(int toAdd){
		yield return new WaitForSeconds (10);//wait 10 secs
		metalOre += toAdd;
	}


	public void initPlayerAddtitional(float timeLeft){
		this.timeLeft = timeLeft;

	}
	public void setDead(){
		alive = false;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("enemy")) {
			//get damage
			EnemyControler eC = other.GetComponentInParent<EnemyControler>();
			int damage = eC.getDamage ();
			hpCurrent -= damage;
		}
	}
	public void pause(){
		isPaused = true;
	}

	// Update is called once per frame
	void Update () {
		if (isPaused) {
			return;
		}
		//movement
		if (hpCurrent <= 0){//dead
			alive = false;
		}
		if (this.timeLeft > 0 && !waiting) {
			this.timeLeft -= Time.deltaTime;
		}
		if (alive) {//only move when alive
			float vertical = Input.GetAxis ("Vertical");
			float horizontal = Input.GetAxis ("Horizontal");
			Vector2 movement = new Vector2 (horizontal * speed * spdMultiplyer, vertical * speed);
			gameObject.transform.Translate (movement);

			//attack
			if (Input.GetButtonDown("Fire1")){
				sword.SetActive (true);
				this.sC.cutTrigger ();
			}
		}
	}

	public int getGold(){
		return gold;
	}

	public void addGold(int g){
		gold += g;
	}

	public bool removeGold(int amt){
		if (amt <= this.gold) {
			this.gold -= amt;
			return true;
		} else {
			return false;
		}
	}

	public int getHP(){
		return hpCurrent;
	}

	public int getHPTotal(){
		return hpTotal;
	}

	public void addHPCurrent(int a){
		hpCurrent = Mathf.Min(hpTotal, hpCurrent + a);
	}

	public void increHPTotal(int a){
		hpTotal += a;
	}



	public void hitEnemy(EnemyControler eC){
		eC.isHit (this.atk);
		if (eC.getHp () <= 0) {
			SpawnEnemies.numEnemies--;
			addGold (eC.goldDropped);
			Destroy (eC.gameObject);
		}

	}
	public void addTime(float t){
		this.timeLeft += t;
	}
	public float getTimeLeft(){
		return timeLeft;
	}
}



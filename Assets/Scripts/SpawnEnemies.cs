using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour {
	private float timeToNext;
	public GameObject gameManager;
	private GameManager gm;
	private int[] enemyType;
	private float[] times;
	private GameObject[] enemies;
	private int counter;
	public static int numEnemies;
	public int[] enemyNumber;

	public float left;
	public float right;
	public float up;
	public float down;
	//do not spend within this range from player position
	public float range;

	public void initSpawn(int[] enemyType, float[] time, int[] enemyNumber){
		this.gm = gameManager.GetComponent<GameManager> ();
		this.enemyType = enemyType;
		this.times = time;
		this.enemies = gm.enemnies;
		this.counter = 0;
		this.timeToNext = times [counter];
		this.enemyNumber = enemyNumber;
		numEnemies = 0;
	}


	
	// Update is called once per frame
	void Update () {
		if (this.gm.isPaused) {
			return;
		}
		if (timeToNext > 0) {
			timeToNext -= Time.deltaTime;
		} else {
			//gm.useProgressMsgs ("Wave " + counter);
			if (counter < this.times.Length) {
				
				//get random loc and instantiate
				for (int i = 0; i < enemyNumber [counter]; i++) {
					Instantiate (enemies [enemyType [counter]], getRandomLocation (), Quaternion.identity);
					timeToNext = times [counter];
					numEnemies++;

				}
				counter++;

			} else if (counter == this.times.Length) {
				//5 sec break till next wave
				//tell gm current level over


				if (numEnemies == 0) {
					gm.setWaitingTime ();
					numEnemies++;//escape this section
				}//only continue to next wave when all enemies are gone
			}

		}

		if (GameManager.debug) {
			if (Input.GetKeyDown (KeyCode.Alpha0)) {
				Debug.Log ("spawn 0");
				Instantiate (enemies [0], getRandomLocation (), Quaternion.identity);
			}
			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				Instantiate (enemies [1], getRandomLocation (), Quaternion.identity);
			}
			if (Input.GetKeyDown (KeyCode.Alpha2)) {
				Instantiate (enemies [2], getRandomLocation (), Quaternion.identity);
			}
		}

	}

	Vector3 getRandomLocation(){
		float xVal = Random.Range (left, right);
		float yVal = Random.Range (down, up);
		Vector3 playerLoc = gm.getPlayerLocation ();
		if (Mathf.Abs (playerLoc.x - xVal) < range && Mathf.Abs (playerLoc.y - yVal) < range) {
			//too close to player
			if (Mathf.Abs (playerLoc.x - xVal) < range && Random.Range (-1, 1) > 0) {
				xVal += range;	
			} else {
				xVal -= range;
			}
			if (Mathf.Abs (playerLoc.y - yVal) < range && Random.Range (-1, 1) > 0) {
				yVal += range;
			} else {
				yVal -= range;
			}
		}
		xVal = Mathf.Max (left, xVal);
		xVal = Mathf.Min (right, xVal);
		yVal = Mathf.Max (down, yVal);
		yVal = Mathf.Min (up, yVal);//onstrain 
		return new Vector3 (xVal, yVal, 0.0f);
	}




}

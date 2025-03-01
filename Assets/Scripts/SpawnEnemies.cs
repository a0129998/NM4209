﻿using System.Collections;
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
	private int totalEnemiesThisWave;
	public static int numEnemiesKilledThisWave;

	public void initSpawn(int[] enemyType, float[] time, int[] enemyNumber){
		this.gm = gameManager.GetComponent<GameManager> ();
		this.enemyType = enemyType;
		this.times = time;
		this.enemies = gm.enemnies;
		this.counter = 0;
		this.timeToNext = times [counter];
		this.enemyNumber = enemyNumber;
		numEnemies = 0;
		gm.waveCompletionPercentage = 0;

		totalEnemiesThisWave = 0;
		for (int i = 0; i < enemyNumber.Length; i++) {
			totalEnemiesThisWave += enemyNumber [i];
		}
		numEnemiesKilledThisWave = 0;
	}

	int getWaveCompletePercent(){
		//Debug.Log (Mathf.RoundToInt( (numEnemiesKilledThisWave * 100) / totalEnemiesThisWave));
		return Mathf.RoundToInt( (numEnemiesKilledThisWave * 100) / totalEnemiesThisWave);
	}

	
	// Update is called once per frame
	void Update () {
		if (this.gm.isPaused) {
			return;
		}
		gm.waveCompletionPercentage = getWaveCompletePercent ();
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
			if (Mathf.Abs (playerLoc.x - xVal) < range && (playerLoc.x - xVal) > 0) {
				xVal += range;

			} else {
				xVal -= range;

			}
			if (Mathf.Abs (playerLoc.y - yVal) < range && (playerLoc.y - yVal) > 0) {
				yVal += range;

			} else {
				yVal -= range;
				yVal = Mathf.Max (down, yVal);
			}

			if (xVal > right) {
				xVal = Mathf.Min (right, xVal);
			}
			if (xVal < left) {
				xVal = Mathf.Max (left, xVal);
			}
			if (yVal > up) {
				yVal = Mathf.Min (up, yVal);
			}
			if (yVal < down) {
				yVal = Mathf.Max (down, yVal);
			}
		}

		return new Vector3 (xVal, yVal, -11.0f);
	}




}

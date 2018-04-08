using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expandRetract : MonoBehaviour {
	public Vector3 scaleMax;
	public Vector3 scaleMin;
	public float scaleUpTime;
	private float scaleUpTimer;
	private float multiplier;
	public bool startExpending;
	// Use this for initialization
	void Start () {
		scaleUpTimer = 0.0f;
		multiplier = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (startExpending) {
			if (scaleUpTimer >= scaleUpTime) {
				multiplier = -1;
			} else if (scaleUpTimer <= 0.0f) {
				multiplier = 1;
			}

			scaleUpTimer += multiplier * Time.deltaTime;
			//Debug.Log (scaleUpTimer/scaleUpTime);
			gameObject.transform.localScale = Vector3.Lerp (scaleMin, scaleMax, scaleUpTimer / scaleUpTime);
		}
	}
}

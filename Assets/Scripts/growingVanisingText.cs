using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class growingVanishingNumbers : MonoBehaviour {

	// Use this for initialization
	public float timeToVanish;
	public float speed;
	private float internalTimer;
	public Text text;//so this can be changed
	private Color c;
	void Start () {
		timeToVanish = 50.0f;
		speed = 0.01f;
		internalTimer = timeToVanish;
		text = GetComponent<Text> ();
	}

	// Update is called once per frame
	void Update () {
		if (internalTimer < 0) {
			Destroy (gameObject);
		} else {
			internalTimer -= Time.deltaTime;
			text.color = Color.Lerp (text.color, Color.clear, 1.0f - internalTimer / timeToVanish);
			transform.position += Vector3.one * speed;
		}
	}
}

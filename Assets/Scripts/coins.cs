using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coins : MonoBehaviour {
	public float fadeTime;
	public float fadeSpeed;
	private float fadeTimer;
	private SpriteRenderer sr;
	private Color originalColour;
	// Use this for initialization
	void Start () {
		fadeTimer = 0.0f;
		sr = GetComponent<SpriteRenderer> ();
		originalColour = sr.color;
	}
	
	// Update is called once per frame
	void Update () {
		if (fadeTimer > fadeTime) {
			Destroy (gameObject);
		} else {
			fadeTimer += Time.deltaTime*fadeSpeed;
			sr.color = Color.Lerp (originalColour, Color.clear, fadeTimer / fadeTime);

		}
	}
}

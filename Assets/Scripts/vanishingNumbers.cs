using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class vanishingNumbers : MonoBehaviour {

	// Use this for initialization
	public float timeToVanish;
	public float speed;
	private float internalTimer;
	public Text text;//so this can be changed
	private Color c;
	public GameObject parent;
	void Start () {
		timeToVanish = 5.0f;
		speed = 0.01f;
		internalTimer = timeToVanish;
		text = GetComponent<Text> ();
	}

	void destroyParent(){
		
		Destroy (parent);
	}
	
	// Update is called once per frame
	void Update () {
		if (internalTimer <= 0) {
			destroyParent ();
		} else {
			internalTimer -= Time.deltaTime;
			text.color = Color.Lerp (text.color, Color.clear, 1.0f - internalTimer / timeToVanish);
			transform.position += Vector3.one * speed;
		}
	}
}

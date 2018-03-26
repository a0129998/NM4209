using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class blackSmithScript : MonoBehaviour {
	public GameObject gameManager;
	private GameManager gM;

	void Start(){
		gM = gameManager.GetComponent<GameManager> ();
	}
	void Update(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			gM.blackSmithBackRun ();
		}
	}
}

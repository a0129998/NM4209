using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class blackSmithScript : MonoBehaviour {
	public Button a1;
	public GameObject gameManager;
	private GameManager gm;
	// Use this for initialization
	void Start () {
		a1.onClick.AddListener (a1Click);
		gm = gameManager.GetComponent<GameManager> ();
	}

	public void a1Click(){
		//check ore
		if(gm.checkOre()>= 10){
			gm.getpC ().hpTotal += 20;

		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class openCloseScript : MonoBehaviour {
	public GameObject gO;
	public Button opnbtn;
	public Button closeBtn;
	// Use this for initialization
	void Start () {
		gO.SetActive (false);
		opnbtn.onClick.AddListener (gOActive);
		closeBtn.onClick.AddListener (gODeActive);
	}

	void gOActive(){
		gO.SetActive (true);
	}

	void gODeActive(){
		gO.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

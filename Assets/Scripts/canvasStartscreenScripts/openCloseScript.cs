using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//this script is not deactivated
public class openCloseScript : MonoBehaviour {
	public GameObject gO;
	public Button opnbtn;
	public Button closeBtn;
	public AudioSource opSound;
	public AudioSource closeSound;
	// Use this for initialization
	void Start () {
		gO.SetActive (false);
		opnbtn.onClick.AddListener (gOActive);
		closeBtn.onClick.AddListener (gODeActive);
	}

	void gOActive(){
		gO.SetActive (true);
		opSound.Play ();
	}

	void gODeActive(){
		gO.SetActive (false);
		closeSound.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

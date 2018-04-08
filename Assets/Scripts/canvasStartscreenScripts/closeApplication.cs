using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class closeApplication : MonoBehaviour {
	public Button closeAppBtn;
	// Use this for initialization
	void Start () {
		closeAppBtn.onClick.AddListener (close);
	}
	
	void close(){
		Application.Quit ();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class instructionsBehaviour : MonoBehaviour {
	public Sprite[] instructionPages;
	public Button backBtn;
	public Button nextBtn;
	public Image img;
	private int currPage;//starts with 0 to instructionPages.length - 1
	// Use this for initialization
	void Start () {
		currPage = 0;
		backBtn.onClick.AddListener (previousPage);
		nextBtn.onClick.AddListener (nextPage);
	}
	void updatePage(){
		img.sprite = instructionPages[currPage];
	}

	void nextPage(){
		currPage++;
		updatePage ();
	}

	void previousPage(){
		currPage--;
		updatePage();
	}

	void Update(){
		if (currPage == 0 && backBtn.gameObject.activeInHierarchy) {
			backBtn.gameObject.SetActive (false);
		} else if (currPage > 0 && !backBtn.gameObject.activeInHierarchy) {
			backBtn.gameObject.SetActive (true);
		}

		if (currPage == (instructionPages.Length - 1) && nextBtn.gameObject.activeInHierarchy) {
			nextBtn.gameObject.SetActive (false);
		} else if (currPage < (instructionPages.Length - 1) && !nextBtn.gameObject.activeInHierarchy) {
			nextBtn.gameObject.SetActive (true);
		}
	}
}

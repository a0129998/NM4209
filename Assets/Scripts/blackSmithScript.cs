using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class blackSmithScript : MonoBehaviour {
	public GameObject gameManager;
	private GameManager gM;
	public Text coinText;
	public Text oreText;
	private PlayerControler pC;
	public AudioSource closeMenuSound;

	void Start(){
		gM = gameManager.GetComponent<GameManager> ();
		pC = gM.player.GetComponent<PlayerControler> ();
	}
	void Update(){
		if (Input.GetKeyDown (KeyCode.Escape) &&  gM.blackSmithCanvas.enabled) {
			Debug.Log ("enable black smith");
			closeMenuSound.PlayOneShot (closeMenuSound.clip);
			gM.blackSmithBackRun ();
		}

		coinText.text = pC.gold.ToString();
		oreText.text = pC.metalOre.ToString ();
	}
}

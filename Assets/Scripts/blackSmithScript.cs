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
	public GameObject warnNotEnoughOreObj;
	private Color originalImgClr;
	private Color originalTxtClr;
	private Image warnNotEnoughOreImg;
	private Text warnNotEnoughOreText;
	public AudioSource notEnoughOre;

	void Start(){
		gM = gameManager.GetComponent<GameManager> ();
		pC = gM.player.GetComponent<PlayerControler> ();
		warnNotEnoughOreImg = warnNotEnoughOreObj.GetComponent<Image> ();
		warnNotEnoughOreText = warnNotEnoughOreObj.GetComponentInChildren<Text> ();
		originalImgClr = warnNotEnoughOreImg.color;
		originalTxtClr = warnNotEnoughOreText.color;
		warnNotEnoughOreImg.enabled = false;
		warnNotEnoughOreText.enabled = false;
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

	public void warnNotEnoughOre(){
		notEnoughOre.PlayOneShot (notEnoughOre.clip);
		warnNotEnoughOreImg.color = originalImgClr;
		warnNotEnoughOreText.color = originalTxtClr;
		StartCoroutine (fadeSlowlyImgText (warnNotEnoughOreImg, warnNotEnoughOreText));
	}

	IEnumerator fadeSlowlyImgText(Image img, Text t){
		float totalFadeTime = 2.0f;
		img.enabled = true;
		t.enabled = true;
		Color originalImgColor = img.color;
		Color originalTextColour = t.color;
		while (totalFadeTime > 0) {
			totalFadeTime -= Time.deltaTime;
			img.color = Color.Lerp (originalImgColor, Color.clear, 1.0f - totalFadeTime);
			t.color = Color.Lerp (originalTextColour, Color.clear, 1.0f - totalFadeTime);
			yield return new WaitForFixedUpdate ();
		}
		img.enabled = false;
		t.enabled = false;
		img.color = originalImgColor;
		t.color = originalTextColour;
	}
}

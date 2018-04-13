using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuScript : MonoBehaviour {
	public GameObject player;
	private PlayerControler pC;
	//shop
	//buy health
	public Image fillImage;
	public Text incrementHealthText;
	public Text notEnoughGoldHealth;
	public Button buyHpBtn;
	private Color originalIncrementHealthTextColor;
	private Color originalNotEnoughGoldHealthColor;


	//time 
	public Button buyTimeBtn;
	public Text incrementTimeText;
	private Color originalIncrementTimeTextColor;
	public Text showTimeText;

	//public Text oreGoldT;
	public Text oreEndT;
	public Button oreInc;
	public Button oreDec;
	public int goldOreExchangeRate;

	public Image blockOre;
	public int oreStored;
	public Text oreTimeText;
	public Text oreTimeText2;
	public Button takeOreBtn;
	public GameObject takeOreBackground;
	public GameObject takeOreObj;
	public Button buyOreBtn;

	//display
	public Text coin;
	public Text ore;
	public Image oreAlert;

	public Text hpText;
	public Image warnNotEnoughGoldImg;
	public Text warnNotEnoughGoldImgText;
	public AudioSource notEnoughGoldClip;
	private int oreGold, oreGain;
	// Use this for initialization
	void Start () {
		pC = player.GetComponent<PlayerControler> ();//get player controller to access player attributes
		//buy 10 health at 1 go
		buyHpBtn.onClick.AddListener(buy10hp);
		originalIncrementHealthTextColor = incrementHealthText.color;
		incrementHealthText.color = Color.clear;
		originalNotEnoughGoldHealthColor = notEnoughGoldHealth.color;
		notEnoughGoldHealth.color = Color.clear;

		//but 10 secs at 1 go
		buyTimeBtn.onClick.AddListener(buy10sec);
		originalIncrementTimeTextColor = incrementTimeText.color;
		incrementTimeText.color = Color.clear;

		oreGold = 0;
		oreGain = 0;
		oreInc.onClick.AddListener (addOre);
		oreDec.onClick.AddListener (reduceOre);
		blockOre.gameObject.SetActive (false);

		takeOreBtn.onClick.AddListener (takeOre);
		oreTimeText.gameObject.SetActive(false);
		oreTimeText2.gameObject.SetActive(false);
		oreAlert.gameObject.SetActive (false);
		warnNotEnoughGoldImg.enabled = false;
		warnNotEnoughGoldImgText.enabled = false;

	}

	void setTakeOre(bool isActive){
		takeOreBackground.gameObject.SetActive (isActive);
		takeOreBtn.gameObject.SetActive (isActive);
		takeOreObj.gameObject.SetActive (isActive);
	}

	public void buy10hp(){
		if (pC.gold >= 10 && (pC.currentPlayerHp < pC.maxPlayerHp)) {
			//buy 10 hp
			pC.gold -= 10;
			pC.currentPlayerHp = Mathf.Min(pC.currentPlayerHp+10, pC.maxPlayerHp);
			incrementHealthText.color = originalIncrementHealthTextColor;
			incrementHealthText.text = "+10hp";
			fillImage.fillAmount = (float)pC.currentPlayerHp / (float)pC.maxPlayerHp;
			StartCoroutine (fadeSlowly (incrementHealthText));
		} else if (pC.gold < 10 && (pC.currentPlayerHp < pC.maxPlayerHp)) {
			warnNotEnoughGold();
		} else {
			notEnoughGoldHealth.text = "HP Full";
			notEnoughGoldHealth.color = originalNotEnoughGoldHealthColor;
			StartCoroutine (fadeSlowly (notEnoughGoldHealth));
		}
	}

	public void buy10sec(){
		if (pC.gold >= 10) {
			//can buy
			pC.gold -= 10;//take gold
			pC.waveTimeLeft += 10.0f;//give time
			incrementTimeText.color = originalIncrementTimeTextColor;
			incrementTimeText.text = "+10s";
			StartCoroutine (fadeSlowly (incrementTimeText));
		} else {
			warnNotEnoughGold();
		}
	}

	void warnNotEnoughGold(){
		notEnoughGoldClip.Play ();
		StartCoroutine (fadeSlowlyImgText (warnNotEnoughGoldImg, warnNotEnoughGoldImgText));
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


	IEnumerator fadeSlowly(Text t){
		float totalFadeTime = 2.0f;
		Color originalColour = t.color;
		while (totalFadeTime > 0) {
			totalFadeTime -= Time.deltaTime;
			t.color = Color.Lerp (originalColour, Color.clear, 1.0f - totalFadeTime);
			yield return new WaitForFixedUpdate ();


		}
	}
		

	public void addOre(){
		if (oreStored > 0) {//can not click when ore is ready to be collected
			return;
		}
		if (pC.gold >= (oreGold + 10)) {//player has at least 10 gold, enough to buy the ore
			oreGain += 10;
			oreGold += goldOreExchangeRate * 10;
		} else {
			//not enough gold
			warnNotEnoughGold();
		}
	}
	public void reduceOre(){
		if (oreStored > 0) {//can not click when ore is ready to be collected
			return;
		}
		if (oreGain > 0) {
			oreGain = oreGain - 10;
			oreGold -= goldOreExchangeRate * 10;
		}
	}

	public void resetOreMenu(){
		oreGain = 0;
		oreGold = 0;
	}

	public int getOre(){
		return oreGain;
	}


	
	// Update is called once per frame
	void Update () {
		oreEndT.text = oreGain + " Ore";
		coin.text = pC.gold.ToString();
		ore.text = pC.metalOre.ToString();
		showTimeText.text = Mathf.Floor(pC.waveTimeLeft).ToString();
		fillImage.fillAmount = (float)pC.currentPlayerHp / (float)pC.maxPlayerHp;
		hpText.text = pC.currentPlayerHp.ToString() + "/" + pC.maxPlayerHp.ToString();
		if (oreStored > 0) {
			//there is ore stored and can be taken
			oreAlert.gameObject.SetActive(true);
			//takeOreBtn.gameObject.SetActive (true);
			setTakeOre(true);
			buyOreBtn.gameObject.SetActive (false);
		} else {
			oreAlert.gameObject.SetActive (false);
			buyOreBtn.gameObject.SetActive (true);
			//takeOreBtn.gameObject.SetActive (false);
			setTakeOre(false);
		}
	}

	public void takeOre(){
		pC.metalOre += oreStored;//take stored ore
		oreStored = 0;//reset stored
		resetOreMenu();
		disableBlockOre();
	}

	public void disableBlockOre(){
		blockOre.gameObject.SetActive (false);
	}
}

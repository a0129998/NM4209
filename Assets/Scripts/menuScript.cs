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

	public Image blockOre;
	public int oreStored;
	public Text oreTimeText;
	public Text oreTimeText2;
	public Button takeOreBtn;
	public Button buyOreBtn;

	//display
	public Text coin;
	public Text ore;
	public Image oreAlert;

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

	}

	public void buy10hp(){
		if (pC.gold >= 10 && (pC.currentPlayerHp < pC.maxPlayerHp)) {
			//buy 10 hp
			pC.gold -= 10;
			pC.currentPlayerHp += 10;
			incrementHealthText.color = originalIncrementHealthTextColor;
			incrementHealthText.text = "+10hp";
			fillImage.fillAmount = (float)pC.currentPlayerHp / (float)pC.maxPlayerHp;
			StartCoroutine (fadeSlowly (incrementHealthText));
		} else if (pC.gold < 10 && (pC.currentPlayerHp < pC.maxPlayerHp)) {
			notEnoughGoldHealth.text = "Not Enough Gold";
			notEnoughGoldHealth.color = originalNotEnoughGoldHealthColor;
			StartCoroutine (fadeSlowly (notEnoughGoldHealth));
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
			incrementTimeText.color = originalIncrementTimeTextColor;
			incrementTimeText.text = "Not Enough Gold!";
			StartCoroutine (fadeSlowly (incrementTimeText));
		}
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
		if (pC.gold >= (oreGold + 10)) {//player has at least 10 gold, enough to buy the ore
			oreGain++;
			oreGold += 10;
		} else {
			//not enough gold
		}
	}
	public void reduceOre(){
		oreGain = Mathf.Max (0, oreGain - 1);
		oreGold -= 10;
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
		showTimeText.text = Mathf.Floor(pC.waveTimeLeft).ToString() + "s";
		fillImage.fillAmount = (float)pC.currentPlayerHp / (float)pC.maxPlayerHp;
		if (oreStored > 0) {
			//there is ore stored and can be taken
			oreAlert.gameObject.SetActive(true);
			takeOreBtn.gameObject.SetActive (true);
			buyOreBtn.gameObject.SetActive (false);
		} else {
			oreAlert.gameObject.SetActive (false);
			buyOreBtn.gameObject.SetActive (true);
			takeOreBtn.gameObject.SetActive (false);
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

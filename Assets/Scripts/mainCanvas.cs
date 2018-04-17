using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainCanvas : MonoBehaviour {
	public GameObject fillObj;
	private Image fillImg;
	public GameObject player;
	private PlayerControler p;
	private int playerGoldCounter;
	private float playerTimeCounter;
	private int playerOreCounter;
	private int playerHealthCounter;
	public Text addCoinText;
	private Color originalCoinTextColour;
	public Text hpText;//now in uibehindplayercanvas

	public GameObject gameManager;
	private GameManager gM;
	public Text waitingTimeText;


	// Use this for initialization
	void Start () {
		fillImg = fillObj.GetComponent<Image> ();
		p = player.GetComponent<PlayerControler> ();
		playerGoldCounter = p.gold;
		originalCoinTextColour = addCoinText.color;
		addCoinText.color = Color.clear;
		gM = gameManager.GetComponent<GameManager> ();
		waitingTimeText.gameObject.SetActive (false);
		//playerOreCounter = p.metalOre;
		//addOreText.color = Color.clear;
	}
	
	// Update is called once per frame
	void Update () {
		fillImg.fillAmount = (float)p.currentPlayerHp / (float)p.maxPlayerHp;
		hpText.text = p.currentPlayerHp.ToString () + "/" + p.maxPlayerHp.ToString ();
		//Debug.Log (p.gold);
		if (playerGoldCounter < p.gold) {
			//player gained gold
			//Debug.Log("player gained gold");
			addCoinText.text = "+" + (p.gold - playerGoldCounter);
			addCoinText.color = originalCoinTextColour;
			StartCoroutine (fadeSlowly(addCoinText));
			playerGoldCounter = p.gold;

		}

		if (gM.betweenLevelsWaitingTime > 0) {
			waitingTimeText.gameObject.SetActive (true);
			waitingTimeText.text = "Next Wave Begin In: " + gM.betweenLevelsWaitingTime.ToString ("F0");
		} else if(waitingTimeText.gameObject.activeInHierarchy){
			waitingTimeText.gameObject.SetActive (false);
		}
	}

	IEnumerator fadeSlowly(Text t){
		float totalFadeTime = 1.0f;
		Color originalColour = t.color;
		while (totalFadeTime > 0) {
			totalFadeTime -= Time.deltaTime;
			t.color = Color.Lerp (originalColour, Color.clear, 1.0f - totalFadeTime);
			yield return new WaitForFixedUpdate ();
		}
	}
}

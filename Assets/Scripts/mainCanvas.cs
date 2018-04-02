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
	// Use this for initialization
	void Start () {
		fillImg = fillObj.GetComponent<Image> ();
		p = player.GetComponent<PlayerControler> ();
		playerGoldCounter = p.gold;
		originalCoinTextColour = addCoinText.color;
		addCoinText.color = Color.clear;
		//playerOreCounter = p.metalOre;
		//addOreText.color = Color.clear;
	}
	
	// Update is called once per frame
	void Update () {
		fillImg.fillAmount = (float)p.currentPlayerHp / (float)p.maxPlayerHp;
		//Debug.Log (p.gold);
		if (playerGoldCounter < p.gold) {
			//player gained gold
			//Debug.Log("player gained gold");
			addCoinText.text = "+" + (p.gold - playerGoldCounter);
			addCoinText.color = originalCoinTextColour;
			StartCoroutine (fadeSlowly(addCoinText));
			playerGoldCounter = p.gold;
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

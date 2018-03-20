using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuScript : MonoBehaviour {
	public Text healthGold;
	public Text healthEnd;
	public Button healthInc;
	public Button healthDec;

	public Text timeGoldT;
	public Text timeEndT;
	public Button timeInc;
	public Button timeDec;

	public Text oreGoldT;
	public Text oreEndT;
	public Button oreInc;
	public Button oreDec;

	private int hpGold, hp, timeGold, timeGain, oreGold, oreGain;
	// Use this for initialization
	void Start () {
		hpGold = 0;
		hp = 0;
		healthGold.text = hpGold + " G";
		healthEnd.text = hp + " HP";
		healthInc.onClick.AddListener (addHealth);
		healthDec.onClick.AddListener (reduceHealth);

		timeGold = 0;
		timeGain = 0;
		timeGoldT.text = timeGold + " G";
		timeGoldT.text = timeGain + " s";
		timeInc.onClick.AddListener (addTime);
		timeDec.onClick.AddListener (reduceTime);

		oreGold = 0;
		oreGain = 0;
		oreGoldT.text = oreGold + " G";
		oreGoldT.text = oreGain + " Ore";
		oreInc.onClick.AddListener (addOre);
		oreDec.onClick.AddListener (reduceOre);
	}

	public void addOre(){
		oreGold += 10;
		oreGain++;
	}
	public void reduceOre(){
		oreGold = Mathf.Max (0, oreGold - 10);
		oreGain = Mathf.Max (0, oreGain - 1);
	}

	public void addTime(){
		timeGain++;
		timeGold++;
	}
	public void reduceTime(){
		timeGold = Mathf.Max (0, timeGold - 1);
		timeGain = Mathf.Max (0, timeGain - 1);
	}

	public int timeToBuy(){
		return timeGain;
	}
	public void resetTimeMenu(){
		timeGold = 0;
		timeGain = 0;
	}

	public void resetOreMenu(){
		oreGain = 0;
		oreGold = 0;
	}

	public int getOre(){
		return oreGain;
	}

	public void addHealth(){
		hpGold++;
		hp++;//rate is 1 to 1
	}
	public int hpToBuy(){
		return hp;
	}
	public void resethealth(){
		hp = 0;
		hpGold = 0;
	}

	public void reduceHealth(){
		hpGold = Mathf.Max (0, hpGold - 1);
		hp = Mathf.Max (0, hp - 1);
	}
	
	// Update is called once per frame
	void Update () {
		healthGold.text = hpGold + " G";
		healthEnd.text = hp + " HP";
		timeGoldT.text = timeGold + " G";
		timeGoldT.text = timeGain + " s";
		oreGoldT.text = oreGold + " G";
		oreGoldT.text = oreGain + " Ore";
	}
}

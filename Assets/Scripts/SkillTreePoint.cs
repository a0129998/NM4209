using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreePoint : MonoBehaviour {
	public bool activated;
	public GameObject player;
	private PlayerControler pC;
	public int hpAdd;
	public int hpReg;
	public int costInOre;
	public int atkAdd;
	public float critAdd;
	public int totalHPAdd;
	public bool poison;//not implemented
	public float MSAdd;//not implemented
	public float ASAdd;//not implemented

	public Button thisButton;
	// Use this for initialization

	void Start(){
		pC = player.GetComponent<PlayerControler> ();
		thisButton.onClick.AddListener (upgradePlayer);
		activated = false;
	}
	public void upgradePlayer(){
		if (pC.metalOre > costInOre && canActivate()) {
			pC.metalOre -= costInOre;
			pC.attack += atkAdd;
			pC.hpRegenPer5Sec += hpReg;
			pC.maxPlayerHp += totalHPAdd;
			pC.critRate *= critAdd;
			pC.playerSpeed += MSAdd;
			pC.atkSpd += ASAdd;
			activated = true;
		}
	}

	public bool canActivate(){
		Debug.Log (gameObject.name);
		switch (gameObject.name) {
		case "2a":
			return GameObject.Find ("1a").GetComponent<SkillTreePoint> ().activated;
		case "3a":
			return GameObject.Find ("2a").GetComponent<SkillTreePoint> ().activated;
		case "4a":
			return GameObject.Find ("3a").GetComponent<SkillTreePoint> ().activated;
		case "2b":
			return GameObject.Find ("1b").GetComponent<SkillTreePoint> ().activated;
		case "3b":
			return GameObject.Find ("2b").GetComponent<SkillTreePoint> ().activated;
		case "4b":
			return GameObject.Find ("3b").GetComponent<SkillTreePoint> ().activated;
		case "2c":
			return GameObject.Find ("1c").GetComponent<SkillTreePoint> ().activated;
		case "3c":
			return GameObject.Find ("2c").GetComponent<SkillTreePoint> ().activated;
		case "4c":
			return GameObject.Find ("3c").GetComponent<SkillTreePoint> ().activated;

		default:
			return true;
		}
	}
}

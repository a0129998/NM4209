using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillTreePoint : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{
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
	public float MSAdd;//multiplication
	public float ASAdd;//multiplication

	public Text tooltip;
	public GameObject panel;

	public Button thisButton;

	private SpriteRenderer sR;
	private Image buttonImg;
	public Sprite locked;
	public Sprite notObtained;
	public Sprite obtained;

	public float rightOffset;
	public float downOffset;
	// Use this for initialization

	void Start(){
		pC = player.GetComponent<PlayerControler> ();
		thisButton.onClick.AddListener (upgradePlayer);
		activated = false;
		tooltip.enabled = false;
		panel.SetActive (false);
		//notObtained.enabled = false;
		//locked.enabled = false;
		buttonImg = gameObject.GetComponent<Image>();
		rightOffset = tooltip.GetPixelAdjustedRect ().width;
		downOffset = tooltip.GetPixelAdjustedRect ().height;
	}
	public void upgradePlayer(){
		if (pC.metalOre >= costInOre && canActivate() && !activated) {
			pC.metalOre -= costInOre;
			pC.attack += atkAdd;
			pC.hpRegenPer5Sec += hpReg;
			pC.maxPlayerHp += totalHPAdd;
			pC.playerSpeed = Mathf.Max (pC.playerSpeed * MSAdd, pC.playerSpeed);
			pC.atkSpd = Mathf.Max (pC.atkSpd * ASAdd, pC.atkSpd);
			pC.critRate = Mathf.Max (pC.critRate * critAdd, pC.critRate);
			activated = true;
		}
	}

	void Update(){
		tooltipFollowMouse ();
		buttonStateUpdate ();
	}

	private void tooltipFollowMouse(){
		if (tooltip.enabled) {
			tooltip.transform.position = Input.mousePosition + new Vector3(rightOffset, -1 * downOffset, 0.0f);
		}
	}
	private void buttonStateUpdate(){
		if (!canActivate ()) {
			buttonImg.sprite = locked;
		} else if (!activated) {
			buttonImg.sprite = notObtained;
		} else {
			buttonImg.sprite = obtained;
		}		
	}


	public bool canActivate(){
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


	public void OnPointerEnter(PointerEventData p){
		tooltip.enabled = true;
		panel.SetActive (true);
	}
	public void OnPointerExit(PointerEventData p){

		tooltip.enabled = false;
		panel.SetActive (false);
	}
		

}

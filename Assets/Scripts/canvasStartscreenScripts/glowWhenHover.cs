using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class glowWhenHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	public Sprite glowingSprite;
	public Sprite normalSprite;
	private Image img;
	// Use this for initialization
	void Start () {
		img = GetComponent<Image> ();
	}
	
	public void OnPointerEnter(PointerEventData eD){
		img.sprite = glowingSprite;
	}

	public void OnPointerExit(PointerEventData eD){
		img.sprite = normalSprite;
	}


}

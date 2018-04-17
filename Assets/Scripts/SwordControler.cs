using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordControler : MonoBehaviour {
	public GameObject gameManager;
	private GameManager gm;
	private PlayerControler player;
	private float pT;
	private bool isSlashing;//player cannot slash when is slashing
	private Quaternion originalLocalRotate;
	private float atkAngleCounter;

	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);//only active when called
		gm = gameManager.GetComponent<GameManager> ();
		player = gameObject.GetComponentInParent<PlayerControler> ();
		isSlashing = false;
		originalLocalRotate = transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
		if ( atkAngleCounter > 0) {
			atkAngleCounter -= Time.deltaTime * player.atkSpd;
			transform.Rotate (Vector3.back * Time.deltaTime * player.atkSpd);
		} else if(isSlashing){
			//just finished slashing
			isSlashing = false;
			transform.localRotation = originalLocalRotate;
			gameObject.SetActive (false);
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.CompareTag ("enemy")) {
			player.hitEnemy(col.gameObject.GetComponent<EnemyControler>());
		}
	}

	public void cutTrigger(){
		if(!isSlashing){
			isSlashing = true;
			Vector3 mousePos = Camera.main.ScreenToWorldPoint( Input.mousePosition);
			Vector3 pos = mousePos - gm.getPlayerLocation();
			pos.z = 0.0f;//set z value to 0
			//set sword to be pointing towards the mouse
			float angleFromXAxis = Vector3.Angle(Vector3.right, pos);//positive angle from x axis
			//Debug.Log (angleFromXAxis);
			if (pos.y > 0) {
				//rotate clockwise
				transform.Rotate(new Vector3(0.0f, 0.0f, angleFromXAxis - 90.0f));
			} else {
				transform.Rotate(new Vector3(0.0f, 0.0f, angleFromXAxis * -1 - 90.0f));
			}
			//rotate half of atk angle anticlockwise
			transform.Rotate(new Vector3(0.0f, 0.0f, player.atkAngle/2.0f));
			atkAngleCounter = player.atkAngle;
		}
	}
}

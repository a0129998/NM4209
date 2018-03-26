using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordControler : MonoBehaviour {
	public GameObject gameManager;
	private GameManager gm;
	private PlayerControler player;
	private float t;
	private float atkSpd;
	private float pT;
	private float angleStart;
	private float angleEnd;


	public Vector3 pos;
	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);//only active when called
		gm = gameManager.GetComponent<GameManager> ();
		t = 0.0f;
		player = gameObject.GetComponentInParent<PlayerControler> ();
		atkSpd = player.atkSpd;
	}
	
	// Update is called once per frame
	void Update () {
		if (t > 0) {
			t -= Time.deltaTime;
			transform.localRotation = Quaternion.Euler (new Vector3 (0.0f, 0.0f, t * atkSpd));
		} else {
			gameObject.SetActive (false);
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.CompareTag ("enemy")) {
			//tell player that enemy is hit
			player.hitEnemy(col.gameObject.GetComponent<EnemyControler>());
		}
	}

	public void cutTrigger(){
		t = 1.0f;
		Vector3 mousePos = Camera.main.ScreenToWorldPoint( Input.mousePosition);
		Vector3 pos = mousePos - gm.getPlayerLocation();
		//transform.eulerAngles = pos;
		this.pos = new Vector3(pos.x, pos.y, 0.0f); 
		Debug.Log (this.pos);
	}
}

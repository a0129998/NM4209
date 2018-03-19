using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordControler : MonoBehaviour {
	public GameObject gameManager;
	private GameManager gm;
	private PlayerControler player;
	private float t;
	private float speed;
	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);//only active when called
		gm = gameManager.GetComponent<GameManager> ();
		t = 0.0f;
		speed = 350;
		player = gameObject.GetComponentInParent<PlayerControler> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (t > 0) {
			t -= Time.deltaTime;
			gameObject.transform.Rotate (Vector3.back * Time.deltaTime * speed);
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
		Vector3 mousePos = Input.mousePosition;
		Vector3 pos = mousePos - gm.getPlayerLocation();
		//gameObject.transform.rotation = new Quaternion (Mathf.Ceil( pos.x), Mathf.Ceil( pos.y), 0.0f, 1.0f);
	}
}

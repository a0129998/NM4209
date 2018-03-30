using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControler : MonoBehaviour {
    public bool isPlayerPaused;
	public bool isTimePaused;

    public float playerSpeed;//speed of player
    public bool isPlayerAlive;//can not move if dead
    //player stats
    public int maxPlayerHp;
    public int hpRegenPer5Sec;
    public int attack;
    public float critDamageMultiplier;
    public float atkSpd;
	public float atkAngle; //0 to 360 degrees
    public float slashDegrees;


    public GameObject sword;
    private SwordControler sC;
    //player resources
    public int gold;
    public int metalOre;
    public int currentPlayerHp;
    public float waveTimeLeft;
    public float critRate;

	//sprites change direction
	private SpriteRenderer sR;
	public Sprite frontSprite;
	public Sprite backSprite;
	public Sprite rightSprite;

	public GameObject mScript;
	private menuScript mS;

	private float s;

	public bool invincible;


    void Start(){
        this.isPlayerAlive = true;
		sC = sword.GetComponent<SwordControler> ();
		sR = gameObject.GetComponent<SpriteRenderer> ();
		mS = mScript.GetComponent<menuScript> ();
		s = 5.0f;
    }

    public void buyOre(int toBuy){
        int cost = toBuy * 10;
		if (cost <= gold) {
			gold -= cost; 
			StartCoroutine (aO (toBuy, mS));
		}
    }
	IEnumerator aO(int toAdd, menuScript mS){
		
		mS.blockOre.enabled = true;
		while (isPlayerPaused) {
			yield return null;
		}
        yield return new WaitForSeconds (10);//wait 10 secs
        metalOre += toAdd;
		mS.blockOre.enabled = false;
    }


    void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("enemy") && this.CompareTag("Player") && !invincible) {
            //get damage
			//Debug.Log("playerhit");
            EnemyControler eC = other.GetComponentInParent<EnemyControler>();
            int damage = eC.getDamage ();
			currentPlayerHp -= damage;
        }
    }

    // Update is called once per frame
    void Update () {
        if (isPlayerPaused) {
            return;
        }
        if (!isPlayerAlive){
            return;//dead player does not move
        }
        //movement
		if (currentPlayerHp <= 0){//dead
            this.isPlayerAlive = false;
        }
		if (waveTimeLeft > 0 && !isTimePaused) {
			waveTimeLeft -= Time.deltaTime;
        }
    
        float vertical = Input.GetAxis ("Vertical");
        float horizontal = Input.GetAxis ("Horizontal");
		Vector2 movement = new Vector2 (horizontal * playerSpeed, vertical * playerSpeed);
        gameObject.transform.Translate (movement);

        //attack
        if (Input.GetButtonDown("Fire1")){
            sword.SetActive (true);
            this.sC.cutTrigger ();
        }

		//sprites
		Rect t = sR.sprite.textureRect;
		if (vertical > 0) {
			if (vertical > Mathf.Abs (horizontal)) {
				sR.sprite = backSprite;
			} else if (horizontal > 0) {
				sR.flipX = false;
				sR.sprite = rightSprite;
			} else if (horizontal < 0) {
				sR.sprite = rightSprite;
				sR.flipX = true;
			}

		} else{
			if (Mathf.Abs( vertical) > Mathf.Abs (horizontal)) {
				sR.sprite = frontSprite;
			}else if(horizontal > 0){
				sR.flipX = false;
				sR.sprite = rightSprite;
			}else if (horizontal < 0) {
				sR.sprite = rightSprite;
				sR.flipX = true;
			}
		}
		sR.sprite.textureRect.Set (t.x, t.y, t.width, t.height);

		if (s < 0) {
			s = 5.0f;
			currentPlayerHp += hpRegenPer5Sec;
		}
    }



    public bool removeGold(int amt){
        if (amt <= this.gold) {
            this.gold -= amt;
            return true;
        } else {
            return false;
        }
    }

    public void addHPCurrent(int a){
		currentPlayerHp = Mathf.Min(maxPlayerHp, currentPlayerHp + a);
    }



    public void hitEnemy(EnemyControler eC){
		int effectiveAtk = attack;
		if (Random.Range(0.0f, 1.0f) < critRate) {
			effectiveAtk = (int)(1.5f *  (float)effectiveAtk);//add crit rate later
		}
		eC.isHit (effectiveAtk);
        if (eC.getHp () <= 0) {
            SpawnEnemies.numEnemies--;
			gold += eC.goldDropped;
            Destroy (eC.gameObject);
        }

    }
}



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
	public GameObject healthBarObject;
	private Image healthBar;


    public GameObject sword;
    private SwordControler sC;
    //player resources
    public int gold;
    public int metalOre;
    public int currentPlayerHp;
    public float waveTimeLeft;
    public float critRate;
	public int playerScore;

	//sprites change direction
	private SpriteRenderer sR;
	public Sprite frontSprite;
	public Sprite backSprite;
	public Sprite rightSprite;

	public GameObject mScript;
	private menuScript mS;

	private float s;

	public bool invincible;
	public int hpBlink;//times to blink


	//detect walls
	private bool contactLeft;
	private bool contactRight;
	private bool contactUp;
	private bool contactDown;

	public GameObject vanishingText;
	public float chanceToLeach;
	public float chanceToPassiveDodge;

	public float invulnerabilityTimer;

	public AudioSource playerSlash;
	public AudioSource playerIsHit;
	public AudioSource enemyIsHit;


    void Start(){
        this.isPlayerAlive = true;
		sC = sword.GetComponent<SwordControler> ();
		sR = gameObject.GetComponent<SpriteRenderer> ();
		mS = mScript.GetComponent<menuScript> ();
		s = 5.0f;
		healthBar = healthBarObject.GetComponent<Image> ();
		invulnerabilityTimer = 0.0f;
		playerScore = 0;
		isPlayerPaused = false;
    }

	public void buyOre(int toBuy, int exchangeRate){
		//cost is checked in game manager
		int cost = toBuy * exchangeRate;
		Debug.Log (cost);
		this.gold -= cost; 
		StartCoroutine (aO (toBuy, mS));
    }
	IEnumerator aO(int toAdd, menuScript mS){
		
		mS.blockOre.gameObject.SetActive (true);
		float timer = 30.0f;
		mS.oreTimeText.gameObject.SetActive(true);
		mS.storedOreText.gameObject.SetActive (true);
		mS.oreTimeText.text = Mathf.Floor (timer).ToString();
		mS.storedOreText.text = mS.oreGain.ToString ();
		while (isPlayerPaused) {
			yield return null;
		}
        //yield return new WaitForSeconds (10);//wait 10 secs

		while (timer >= 0.0f) {
			if (isPlayerPaused) {
				yield return null;
			} else {
				yield return new WaitForFixedUpdate ();
				timer -= Time.deltaTime;
				mS.oreTimeText.text = Mathf.Floor (timer).ToString () + "S";
				mS.storedOreText.text = toAdd.ToString ();
			}
		}
		mS.oreStored = toAdd;
		mS.blockOre.gameObject.SetActive (false);
		mS.oreTimeText.gameObject.SetActive(false);
		mS.storedOreText.gameObject.SetActive (false);
		yield return null;
    }
	IEnumerator blinkPlayer(){
		for (int i = 0; i < hpBlink; i++) {
			yield return new WaitForFixedUpdate ();
			healthBar.enabled = !healthBar.enabled;
			Color faded = new Color (1.0f, 1.0f, 1.0f, 0.5f);
			if (healthBar.enabled) {
				sR.color = Color.white;
			} else {
				sR.color = faded;
			}
		}
		healthBar.enabled = true;
		sR.color = Color.white;
		yield return null;
	}


    void OnTriggerEnter2D(Collider2D other){
		//Debug.Log ("player hit the " + other.gameObject.tag);
		if (isPlayerAlive) {
			if (other.CompareTag ("enemy") && this.CompareTag ("Player")) {
				other.gameObject.GetComponent<EnemyControler> ().moveBackTimer = other.gameObject.GetComponent<EnemyControler> ().moveBackTime;
				//get damage
				//Debug.Log ("playerhit");
				if (invulnerabilityTimer > 0.0f) {
					sR.color = new Color (1.0f, 1.0f, 1.0f, 0.75f);
					invincible = true;
				} else {
					sR.color = Color.white;
					invincible = false;
				}
				bool haveDodged = false;
				float ran = Random.Range (0.0f, 1.0f);
				if (ran < chanceToPassiveDodge) {
					//Debug.Log ("dodged");
					spawnVanishingTextWithWords ("dodged");
					//invulnerabilityTimer = 0.5f;
					haveDodged = true;
				}
				if (!invincible && !haveDodged) {
					playerIsHit.Play ();
					StartCoroutine (blinkPlayer ());
					EnemyControler eC = other.GetComponentInParent<EnemyControler> ();
					int damage = eC.getDamage ();
					currentPlayerHp -= damage;
					spawnVanishingTextWithWords ("-" + damage);
				}
			}

			if (other.CompareTag ("leftwall") && this.CompareTag ("Player")) {
				contactLeft = true;
			}
			if (other.CompareTag ("rightwall") && this.CompareTag ("Player")) {
				contactRight = true;
			}
			if (other.CompareTag ("upwall") && this.CompareTag ("Player")) {
				contactUp = true;
			}
			if (other.CompareTag ("downwall") && this.CompareTag ("Player")) {
				contactDown = true;
			}
		}
    }

	void OnTriggerExit2D(Collider2D other){
		if (other.CompareTag ("leftwall") && this.CompareTag ("Player")) {
			contactLeft = false;
		}
		if (other.CompareTag ("rightwall") && this.CompareTag ("Player")) {
			contactRight = false;
		}
		if (other.CompareTag ("upwall") && this.CompareTag ("Player")) {
			contactUp = false;
		}
		if (other.CompareTag ("downwall") && this.CompareTag ("Player")) {
			contactDown = false;
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
		if (invulnerabilityTimer > 0.0f) {
			invulnerabilityTimer -= Time.deltaTime;//count down invulnerbility timer
		}

		//healthbar
		healthBar.fillAmount = (float)currentPlayerHp/(float)maxPlayerHp;

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
		if (contactLeft) {
			movement.x = Mathf.Max (movement.x, 0.0f);
		}
		if (contactRight) {
			movement.x = Mathf.Min (movement.x, 0.0f);
		}
		if (contactDown) {
			movement.y = Mathf.Max (movement.y, 0.0f);
		}
		if (contactUp) {
			movement.y = Mathf.Min (movement.y, 0.0f);
		}
        gameObject.transform.Translate (movement);

        //attack
        if (Input.GetButtonDown("Fire1")){
			playerSlash.PlayOneShot (playerSlash.clip);
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

		if (s <= 0) {
			s = 5.0f;
			currentPlayerHp = Mathf.Min (currentPlayerHp + hpRegenPer5Sec, maxPlayerHp);
		} else {
			s -= Time.deltaTime;
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
			effectiveAtk = (int)(critDamageMultiplier *  (float)effectiveAtk);//add crit rate later
		}
		enemyIsHit.PlayOneShot (enemyIsHit.clip);
		eC.isHit (effectiveAtk);
        if (eC.getHp () <= 0) {
            SpawnEnemies.numEnemies--;
			gold += eC.goldDropped;
			playerScore += eC.goldDropped * 100;
			eC.dies ();
        }

		if (Random.Range (0.0f, 1.0f) < chanceToLeach) {
			//leach hp
			currentPlayerHp = Mathf.Min (maxPlayerHp, currentPlayerHp + (int)Mathf.Ceil((float)attack*0.05f));
		}

    }

	public void spawnVanishingTextWithWords(string words){
		Debug.Log ("spawn vanishing words");
		GameObject vText = (GameObject)Instantiate (vanishingText, gameObject.transform.position, Quaternion.identity);
		vanishingNumbers vT = vText.GetComponentInChildren<vanishingNumbers> ();
		vT.text.text = words;
		vT.text.color = Color.red;
		vT.transform.position = gameObject.transform.position;
	}
}



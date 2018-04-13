using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicPlayer : MonoBehaviour {
	public AudioSource mainBGM;
	public AudioSource menuBGM;
	public AudioSource loseBGM;
	public AudioSource bossGBM;
	public AudioSource inbetweenWavesBGM;
	private AudioSource[] allAudioSources;

	public GameObject gamemanager;
	private GameManager gM;
	// Use this for initialization
	void Start () {
		gM = gamemanager.GetComponent<GameManager> ();
		allAudioSources = new AudioSource[5] {mainBGM, menuBGM, loseBGM, bossGBM, inbetweenWavesBGM};
	}



	void StopAllAudio() {
		
		foreach( AudioSource audioS in allAudioSources) {
			audioS.Stop();
		}
	}
	// Update is called once per frame
	void Update () {
		if (gM.loseCondition && !loseBGM.isPlaying) {
			//lose
			StopAllAudio();
			loseBGM.Play ();
			return;
		}
		if ((gM.blackSmithCanvas.enabled || gM.settingsCanvas.enabled || gM.menuCanvas.enabled) && !menuBGM.isPlaying) {
			//stop main, play menu
			StopAllAudio();
			menuBGM.Play ();
			return;
		}

		if (!(gM.blackSmithCanvas.enabled || gM.settingsCanvas.enabled || gM.menuCanvas.enabled)) {
			//main
			if (gM.betweenLevelsWaitingTime > 0) {
				if (inbetweenWavesBGM.isPlaying) {
					return;
				} else {
					StopAllAudio ();
					inbetweenWavesBGM.Play ();
					return;
				}

			}

			if ((gM.currLevel + 1) % 5 == 0) {
				//bosslevel
				if (!bossGBM.isPlaying) {
					StopAllAudio ();
					bossGBM.Play ();
				}
				return;
			}

			if(!mainBGM.isPlaying){
				StopAllAudio ();
				mainBGM.Play ();
			}
		}


	}

	bool anyMainMusicPlaying(){
		return mainBGM.isPlaying || inbetweenWavesBGM.isPlaying || bossGBM.isPlaying;
	}
}

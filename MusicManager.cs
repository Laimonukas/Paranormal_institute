using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {




	public List<SoundObject> soundList = new List<SoundObject> ();



	private AudioSource audioSource;

	public bool loopSounds = false;
	public bool randomiseDelay = false;
	public bool needsTrigger = false;
	public float delay = 5.0f;
	public float delayMax = 25.0f;

	private float delayToUse;

	private int trackId = 0;
	// Use this for initialization
	void Start () {
		audioSource = gameObject.GetComponent<AudioSource> ();
		if (!randomiseDelay) {
			delayToUse = delay;
		} else {
			delayToUse = Random.Range (delay, delayMax);
		}
	}






	public void playAudio(){
		if (!audioSource.isPlaying) {
			if (soundList.Count!=0&&trackId<soundList.Count) {
				audioSource.clip = soundList [trackId].audioClip;
				audioSource.Play ();
				trackId++;
				if (trackId>=soundList.Count&&loopSounds) {
					trackId = 0;
				}
			}

		}
	}


	void OnTriggerEnter(Collider other){
		if (needsTrigger) {
			playAudio ();


			
		}
	
	}

	// Update is called once per frame
	void Update () {
		if (!needsTrigger) {
			delayToUse -= Time.deltaTime;
			if (delayToUse<=0) {


				playAudio ();


				if (!randomiseDelay) {
					delayToUse = delay;
				} else {
					delayToUse = Random.Range (delay, delayMax);
				}
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoors : MonoBehaviour {


	public Animator anim;
	public bool doorOpen = false;
	public bool doorLocked = false;
	public float closeTimer = 3.0f;

	public GameObject lightOn;
	public GameObject lightOff;


	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator> ();
		if (lightOn!=null&&lightOff!=null) {
			if (doorLocked) {
				lightOff.SetActive (true);
				lightOn.SetActive (false);
			} else {
				lightOff.SetActive (false);
				lightOn.SetActive (true);
			}
		}

	}


	public void openDoor(){
		if (!doorOpen&&!doorLocked) {
			anim.Play ("OpenDoor");
			doorOpen = true;
		}
	}

	public void closeDoor(){
		if (doorOpen&&!doorLocked) {
			anim.Play ("CloseDoor");
			doorOpen = false;
		}
	}



	// Update is called once per frame
	void Update () {
		if (lightOn!=null&&lightOff!=null) {
			if (doorLocked) {
				lightOff.SetActive (true);
				lightOn.SetActive (false);
			} else {
				lightOff.SetActive (false);
				lightOn.SetActive (true);
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCollider : MonoBehaviour {
	public GameObject door;
	private float timer;



	// Use this for initialization
	void Start () {
		if (door!=null) {
			timer = door.GetComponent<OpenDoors> ().closeTimer;
		}
	}




	// Update is called once per frame
	void Update () {
		if (door.GetComponent<OpenDoors>().doorOpen&&!door.GetComponent<OpenDoors>().doorLocked) {
			timer -= Time.deltaTime;
			if (timer<=0) {
				door.GetComponent<OpenDoors> ().closeDoor ();
				timer = door.GetComponent<OpenDoors> ().closeTimer;
			}
		}
		
	}

	void OnTriggerEnter(Collider other){
		if (door!=null) {
			if (!door.GetComponent<OpenDoors>().doorOpen&&!door.GetComponent<OpenDoors>().doorLocked) {
				door.GetComponent<OpenDoors> ().openDoor ();
			}

		}
	
	}



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCollider : MonoBehaviour {

	public GameObject room;


	// Use this for initialization
	void Start () {
		room = gameObject.transform.parent.gameObject;
	}



	void OnTriggerStay(Collider other){
		if (GameObject.FindGameObjectWithTag ("Player").GetComponent<OptManager> ().currentRoom!=room) {
			GameObject.FindGameObjectWithTag ("Player").GetComponent<OptManager> ().currentRoom = room;
		}



	}
	void OnTriggerExit(Collider other){
		
		GameObject.FindGameObjectWithTag ("Player").GetComponent<OptManager> ().currentRoom = null;


	}

	// Update is called once per frame
	void Update () {
		
	}
}

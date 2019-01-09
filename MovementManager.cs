using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour {
	
	public float moveSpeed =0.25f;

	// Use this for initialization
	void Start () {
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		GameObject.Find ("AllEvents").GetComponent<Events> ().ev.playFadeFromBlack ();
		GameObject.Find ("FlashLight").SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {



	}

	void FixedUpdate(){
		movement ();
	}








	void movement(){

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);



		transform.Translate (movement * moveSpeed*Time.deltaTime);



	}



}

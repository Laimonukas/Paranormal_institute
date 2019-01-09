using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour {


	private bool pauseOpen = false;
	// Use this for initialization
	void Start () {
		
	}

	public void quitGame(){
		Application.Quit ();
	}

	// Update is called once per frame
	void Update () {


		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (pauseOpen) {
				gameObject.transform.Find ("PauseCanvas").gameObject.SetActive (false);
				GameObject.Find ("Main Camera").GetComponent<MouseLook> ().enabled = true;
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
				Time.timeScale = 1.0f;
				pauseOpen = false;
			}else{
				gameObject.transform.Find ("PauseCanvas").gameObject.SetActive (true);
				GameObject.Find ("Main Camera").GetComponent<MouseLook> ().enabled = false;
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
				Time.timeScale = 0.0f;
				pauseOpen = true;
			}

		}
		
	}
}

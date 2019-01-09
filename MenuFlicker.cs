using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MenuFlicker : MonoBehaviour {



	public AllEvents ev =new AllEvents();
	public float min= 2.0f;
	public float max= 5.0f;
	private float timer ;
	public string sceneName = "story";

	// Use this for initialization
	void Start () {
		timer = returnTimer ();
		ev.turnOffAllLamps ();
	}

	public void loadLevel(){
		GameObject.Find ("Loading").GetComponent<Text> ().enabled = true;
		GameObject.Find ("Button").SetActive (false);
		GameObject.Find ("Button (1)").SetActive (false);
		SceneManager.LoadSceneAsync (sceneName);
	}


	public void quitGame(){
		Application.Quit();
	}

	float returnTimer(){
		float number = Random.Range (min, max);
		return number;
	}


	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer<=0) {
			
			timer = returnTimer ();
			ev.flickerSelectedObjects ();
		}


		
	}
}

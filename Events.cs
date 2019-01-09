using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllEvents{
	private List<GameObject> allLamps = new List<GameObject>();
	public List<GameObject> selectLamps = new List<GameObject>();
	public List<GameObject> objectsToActivate = new List<GameObject>();
	public List<GameObject> objectsToDeactivate = new List<GameObject>();
	private List<GameObject> allDoors= new List<GameObject>();
	public List<GameObject> selectedDoors= new List<GameObject>();
	private List<GameObject> allFlickerObjects= new List<GameObject>();
	public List<GameObject> selectedFlickerObjects= new List<GameObject>();

	public List<GameObject> interactableObjectsInRoom = new List<GameObject>();


	private List<GameObject> allSparkObjects= new List<GameObject>();
	public List<GameObject> selectedSparkObjects= new List<GameObject>();

	public Material lightOn;
	public Material lightOff;

	public float emissionTime = 2.0f;
	public bool debugMode = false;


	public bool elevatorDown=false;


	public bool unlockSDoors = false;
	public bool lockSDoors = false;
	public bool turnOffAll= false;
	public bool sparkSelectedLamps = false;
	public bool enableSelectedObjects = false;
	public bool disableSelectedObjects = false;




	public void Start(){
	}
	public void playFadeFromBlack(){
		GameObject.Find ("BlackBackground").GetComponent<Animator> ().Play ("FadeFromBlack",0,0f);
	}
	public void playFadeToBlack(){
		GameObject.Find ("BlackBackground").GetComponent<Animator> ().Play ("FadeToBlack",0,0f);
	}


	public List<GameObject> returnAllFlickerObjects(){
		getAllFlickerObjects ();
		return allFlickerObjects;
	}

	public void callDownElevator(){
		var anim = GameObject.Find ("Lift").GetComponent<Animator> ();
		elevatorDown = true;
		anim.Play ("callDownElevator");

	}
	public void callUpElevator(){
		var anim = GameObject.Find ("Lift").GetComponent<Animator> ();

		anim.Play ("ElevatorUp");

	}

	private void getLights(){


		allLamps = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Lamp"));
	}

	private void getAllDoors(){
		allDoors = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Door"));
	}
		
	private void getAllFlickerObjects(){
		if (allLamps.Count > 0) {
			for (int i = 0; i < allLamps.Count; i++) {
				if (!allFlickerObjects.Contains(allLamps[i].transform.parent.gameObject)) {
					allFlickerObjects.Add (allLamps [i].transform.parent.gameObject);	
				}
			}
		} else {
			getLights ();
			getAllFlickerObjects ();
		}

	}
		
	private void getAllSparkObjects(){
		allSparkObjects =new List<GameObject> (GameObject.FindGameObjectsWithTag ("Sparks"));
	}




	public void unlockSelectedDoors(){
		for (int i = 0; i < selectedDoors.Count; i++) {
			selectedDoors [i].GetComponent<OpenDoors> ().doorLocked = false;
		}
	}

	public void lockSelectedDoors(){
		for (int i = 0; i < selectedDoors.Count; i++) {
			selectedDoors [i].GetComponent<OpenDoors> ().doorLocked = true;
		}
	}


	public void turnOffAllLamps(){
		getLights ();
		if (allLamps.Count > 0) {
			for (int i = 0; i < allLamps.Count; i++) {
				Renderer rend = allLamps [i].transform.parent.Find ("LampSourceObject").GetComponent<Renderer> ();
				rend.enabled = true;
				rend.sharedMaterials[0].DisableKeyword ("_EMISSION");
				rend.sharedMaterials[0].color = Color.black;
				rend.materials[0].DisableKeyword ("_EMISSION");
				rend.materials[0].color = Color.black;
				allLamps [i].GetComponent<Light> ().enabled=false;
				RenderSettings.ambientIntensity = 0.0f;
				if (GameObject.FindGameObjectWithTag ("Player")!=null) {

					GameObject.FindGameObjectWithTag ("Player").GetComponent<OptManager> ().lightsOn = false;
				}


			}
		}
  	}

	public void turnOnAllLamps(){
		getLights ();
		if (allLamps.Count > 0) {
			for (int i = 0; i < allLamps.Count; i++) {
				Renderer rend = allLamps [i].transform.parent.Find ("LampSourceObject").GetComponent<Renderer> ();
				rend.enabled = true;
				rend.sharedMaterials[0].EnableKeyword ("_EMISSION");
				rend.sharedMaterials[0].color = Color.white;
				rend.materials[0].EnableKeyword ("_EMISSION");
				rend.materials[0].color = Color.white;
				allLamps[i].GetComponent<Light> ().enabled =true;
				RenderSettings.ambientIntensity = 0.5f;
				if (GameObject.FindGameObjectWithTag ("Player")!=null) {
					GameObject.FindGameObjectWithTag ("Player").GetComponent<OptManager> ().lightsOn = true;
				}
			}
		}
	}

	public void turnOnSelectedLamps(){
		if (selectLamps.Count > 0) {
			for (int i = 0; i < selectLamps.Count; i++) {
				Renderer rend = selectLamps [i].transform.parent.Find ("LampSourceObject").GetComponent<Renderer> ();
				rend.enabled = true;
				rend.sharedMaterials[0].EnableKeyword ("_EMISSION");
				rend.sharedMaterials[0].color = Color.white;
				rend.materials[0].EnableKeyword ("_EMISSION");
				rend.materials[0].color = Color.white;
				selectLamps[i].GetComponent<Light> ().enabled =true;
			}
		}
	}

	public void turnOffSelectedLamps(){
		if (selectLamps.Count > 0) {
			
			for (int i = 0; i < selectLamps.Count; i++) {
				Renderer rend = selectLamps [i].transform.parent.Find ("LampSourceObject").GetComponent<Renderer> ();
				rend.enabled = true;
				rend.sharedMaterials[0].DisableKeyword ("_EMISSION");
				rend.sharedMaterials[0].color = Color.black;
				rend.materials[0].DisableKeyword ("_EMISSION");
				rend.materials[0].color = Color.black;
				selectLamps[i].GetComponent<Light> ().enabled =false;
			}
		}
	}

	public void enableObjects(){
		if (objectsToActivate.Count > 0) {
			for (int i = 0; i < objectsToActivate.Count; i++) {
				objectsToActivate[i].SetActive(true);
			}
		}
	}

	public void disableObjects(){
		if (objectsToActivate.Count > 0) {
			for (int i = 0; i < objectsToActivate.Count; i++) {
				objectsToActivate[i].SetActive(false);
			}
		}
	}

	public void openAllDoors(){
		if (allDoors.Count > 0) {
			for (int i = 0; i < allDoors.Count; i++) {
				if (!allDoors[i].GetComponent<OpenDoors>().doorOpen) {
					allDoors [i].GetComponent<OpenDoors> ().openDoor ();
				}
			}
		}
	}

	public void closeAllDoors(){
		if (allDoors.Count > 0) {
			for (int i = 0; i < allDoors.Count; i++) {
				if (allDoors[i].GetComponent<OpenDoors>().doorOpen) {
					allDoors [i].GetComponent<OpenDoors> ().closeDoor ();
				}
			}
		}
	}

	public void openSelectedDoors(){
		if (allDoors.Count > 0) {
			for (int i = 0; i < allDoors.Count; i++) {
				if (!allDoors[i].GetComponent<OpenDoors>().doorOpen) {
					allDoors [i].GetComponent<OpenDoors> ().openDoor ();
				}
			}
		}
	}

	public void closeSelectedDoors(){
		if (allDoors.Count > 0) {
			for (int i = 0; i < allDoors.Count; i++) {
				if (allDoors[i].GetComponent<OpenDoors>().doorOpen) {
					allDoors [i].GetComponent<OpenDoors> ().closeDoor ();
				}
			}
		}
	}

	public void flickerAllObjects(){
		
		if (allFlickerObjects.Count > 0) {
			for (int i = 0; i < allFlickerObjects.Count; i++) {
				if (allFlickerObjects [i].transform.Find ("Light").GetComponent<Light> ().enabled) {
					allFlickerObjects [i].GetComponent<Animator> ().enabled = true;
					allFlickerObjects [i].GetComponent<Animator> ().Play ("LightFlicker", -1, 0f);
				} else {
					allFlickerObjects [i].GetComponent<Animator> ().enabled = true;
					allFlickerObjects [i].GetComponent<Animator> ().Play ("FlickerWhenDark",0,0f);
				}

			}
		} else {
			getAllFlickerObjects ();
			flickerAllObjects ();
		}



	}



	public void flickerSelectedObjects(){

		if (selectedFlickerObjects.Count > 0) {

			for (int i = 0; i < selectedFlickerObjects.Count; i++) {
				if (selectedFlickerObjects [i].transform.Find ("Light").GetComponent<Light> ().enabled) {
					selectedFlickerObjects [i].GetComponent<Animator> ().Play ("LightFlicker", 0, 0f);
				} else {
					selectedFlickerObjects [i].GetComponent<Animator> ().Play ("FlickerWhenDark",0,0f);
				}

			}
		}

  	}

	public void turnOnAllSparks(){
		if (allSparkObjects.Count>0) {

			for (int i = 0; i < allSparkObjects.Count; i++) {
				ParticleSystem ps = allSparkObjects [i].GetComponent<ParticleSystem> ();
				var em = ps.emission;
				em.enabled = true;
			}

		} else {
			getAllSparkObjects ();
			turnOnAllSparks ();
		}
	}

	public void turnOffAllSparks(){
		if (allSparkObjects.Count>0) {

			for (int i = 0; i < allSparkObjects.Count; i++) {
				ParticleSystem ps = allSparkObjects [i].GetComponent<ParticleSystem> ();
				var em = ps.emission;
				em.enabled = false;
			}

		} else {
			getAllSparkObjects ();
			turnOffAllSparks ();
		}
	}

	public void turnOnSelectedSparks(){
		if (selectedSparkObjects.Count>0) {

			for (int i = 0; i < selectedSparkObjects.Count; i++) {
				ParticleSystem ps = selectedSparkObjects [i].GetComponent<ParticleSystem> ();
				var em = ps.emission;
				em.enabled = true;
			}

		}
	}

	public void turnOffSelectedSparks(){
		if (selectedSparkObjects.Count>0) {

			for (int i = 0; i < selectedSparkObjects.Count; i++) {
				ParticleSystem ps = selectedSparkObjects [i].GetComponent<ParticleSystem> ();
				var em = ps.emission;
				em.enabled =false;
			}

		}
	}

	public List<GameObject> getInteractableObjectsInRoom(GameObject room){
		List<GameObject> objects = new List<GameObject> ();




		return objects;
	}



	public IEnumerator disableAnimator(Animator anim){
		if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime>1.6f) {
			Debug.Log ("Waiting.");
			anim.enabled = false;
		} else {
			yield return new WaitForSeconds (1.0f);

		}
	}


}






[System.Serializable]
public class Events : MonoBehaviour {
	public AllEvents ev = new AllEvents();
	private float timer;


	// Use this for initialization
	void Start () {
		timer = ev.emissionTime;
		ev.turnOnAllLamps ();
	}
	
	// Update is called once per frame
	void Update () {
		var arr = ev.returnAllFlickerObjects ();
		if (arr.Count>0) {
			for (int i = 0; i < arr.Count; i++) {
				var anim = arr [i].GetComponent<Animator> ();
				if (anim.enabled) {
					if (anim.GetCurrentAnimatorStateInfo(0).length<anim.GetCurrentAnimatorStateInfo(0).normalizedTime) {
						anim.enabled = false;
					}
				}
			}
		}



		if (ev.debugMode) {
			if (Input.GetKeyDown(KeyCode.L)) {
				ev.flickerAllObjects ();
			}
			if (Input.GetKeyDown(KeyCode.E)) {
				ev.turnOnAllSparks ();
			}
			if (Input.GetKeyDown(KeyCode.K)) {
				ev.turnOffAllLamps ();
			}

			if (Input.GetKeyDown(KeyCode.O)) {
				ev.turnOnAllLamps ();
			}
		}

		timer -= Time.deltaTime;
		if (timer<=0) {
			ev.turnOffAllSparks ();
			timer = ev.emissionTime;
		}
	}
}

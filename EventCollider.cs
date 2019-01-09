using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCollider : MonoBehaviour {

	public bool needsEventToSound = true;
	public bool noCurrentEvent=false;
	public eventObject currentEvent= new eventObject();

	public bool addEvents = false;
	public eventObject addEventObject= new eventObject();


	private GameObject eventManager;

	public bool addLines = false;
	public List<SoundObject> soundList = new List<SoundObject> ();

	public GameObject checkpoint;
	public AllEvents ev = new AllEvents ();

	void handleAllEvents(){
		if (ev.selectedDoors.Count>0&&ev.unlockSDoors) {
			ev.unlockSelectedDoors ();

		}
		if (ev.enableSelectedObjects&&ev.objectsToActivate.Count>0) {
			for (int i = 0; i < ev.objectsToActivate.Count; i++) {
				ev.objectsToActivate [i].SetActive (true);
				ev.objectsToActivate.Remove (ev.objectsToActivate [i]);
			}
		}

		if (ev.disableSelectedObjects&&ev.objectsToDeactivate.Count>0) {
			for (int i = 0; i < ev.objectsToDeactivate.Count; i++) {
				ev.objectsToDeactivate [i].SetActive (false);
				ev.objectsToDeactivate.Remove (ev.objectsToDeactivate [i]);
			}
		}

		if (ev.selectedDoors.Count>0&&ev.unlockSDoors) {
			ev.unlockSelectedDoors ();
			for (int i = 0; i < ev.selectedDoors.Count; i++) {
				ev.selectedDoors.Remove (ev.selectedDoors [i]);
			}
		}


	}

	// Use this for initialization
	void Start () {
		eventManager = GameObject.Find ("EventManager");
	}
	
	// Update is called once per frame
	void Update () {
		handleAllEvents ();
	}

	public void addLinesToMain(){
		if (addLines) {
			for (int i = 0; i < soundList.Count; i++) {
				GameObject.Find ("SoundManager").GetComponent<SoundManager> ().soundList.Add (soundList [i]);
			}
		}
	}





	void handleCheckpoints(){

		if (checkpoint!=null) {
			GameObject[] arr = GameObject.FindGameObjectsWithTag ("Checkpoint");
			for (int i = 0; i < arr.Length; i++) {
				if (arr[i]!=checkpoint) {
					arr [i].SetActive (false);
				}

			}

			checkpoint.SetActive (true);
		}


	}

	void OnTriggerEnter(Collider other)
	{	

		if (gameObject!=checkpoint) {
			gameObject.SetActive (false);
		}



		handleCheckpoints ();
		if (noCurrentEvent) {
			if (addEvents) {
				eventManager.GetComponent<EventManager> ().addEvent (addEventObject);

			}

			if (needsEventToSound) {
				if (eventManager.GetComponent<EventManager> ().doesEventExist (currentEvent.eventName)) {
					addLinesToMain ();
				}
			} else {
				addLinesToMain ();
			}

			if (gameObject!=checkpoint) {
				gameObject.SetActive (false);
			}

		} else {
			if (eventManager.GetComponent<EventManager> ().doesEventExist (currentEvent.eventName)) {
				if (addEvents) {
					eventManager.GetComponent<EventManager> ().removeEvent (currentEvent.eventName);
					eventManager.GetComponent<EventManager> ().addEvent (addEventObject);

				}

				if (needsEventToSound) {
					if (eventManager.GetComponent<EventManager> ().doesEventExist (currentEvent.eventName)) {
						addLinesToMain ();
					}
				} else {
					addLinesToMain ();
				}
			}

		}
	}



}

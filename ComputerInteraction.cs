using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class screenObject{
	public string name;
	public string type;
	[TextArea(3,10)]
	public string objText;
	public Sprite objSprite;

	public bool addLines = false;
	public List<SoundObject> soundList = new List<SoundObject> ();

	public bool requiresCurrentEvent=false;
	public eventObject currentEvent= new eventObject();

	public bool addEvent = false;
	public eventObject addEventObject= new eventObject();
	public bool eventHandled = false;

	public bool eventAdded = false;
}




[System.Serializable]
public class ComputerInteraction : MonoBehaviour {



	public List<screenObject> screenObjects = new List<screenObject> ();
	public Color iconColorToSet = Color.cyan;
	public Color exitColorToSet = Color.red;
	public screenObject failedScreenObject = new screenObject();



	private GameObject eventManager;
	private GameObject soundManager;





	// Use this for initialization
	void Start () {

		failedScreenObject.type="error";
		failedScreenObject.objText = "Failed to open file!";
		eventManager = GameObject.Find ("EventManager");
		soundManager = GameObject.Find ("SoundManager");

	}

	public void handleEvent(screenObject obj){
		if (obj.addLines) {
			soundManager.GetComponent<SoundManager> ().addLines (obj.soundList);
		}

		if (obj.addEvent) {
			if (obj.requiresCurrentEvent) {
				if (eventManager.GetComponent<EventManager> ().doesEventExist (obj.currentEvent.eventName)) {
					eventManager.GetComponent<EventManager> ().removeEvent (obj.currentEvent.eventName);
					if (!obj.eventAdded) {
						if (!eventManager.GetComponent<EventManager> ().doesEventExist (obj.addEventObject.eventName)) {
							eventManager.GetComponent<EventManager> ().addEvent (obj.addEventObject);
						}
						obj.eventAdded = true;
					}

				}
			} else {
				if (!obj.eventAdded) {
					if (!eventManager.GetComponent<EventManager> ().doesEventExist (obj.addEventObject.eventName)) {
						eventManager.GetComponent<EventManager> ().addEvent (obj.addEventObject);
					}
					obj.eventAdded = true;
				}
			}



		}


		
	}


	public screenObject returnObjectData(string objName){
		for (int i = 0; i < screenObjects.Count; i++) {
			if (screenObjects[i].name==objName) {
				return screenObjects [i];
			}
		}
	
		return failedScreenObject;

	}


	// Update is called once per frame
	void Update () {
		
	}
}

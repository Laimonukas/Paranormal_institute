using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class SoundObject{
	public AudioClip audioClip;
	public string audioClipName;

	public bool addEvent = false;
	public eventObject addEventObj = new eventObject ();

	public bool requiredEvent = false;
	public eventObject requiredEventObj = new eventObject ();

	public bool requiredPastEvent = false;
	public eventObject requiredPastEventObj = new eventObject ();

	public bool requiredItem = false;
	public inventoryObject invObject = new inventoryObject ();

	public bool subtitleOnly = true;
	public float subtitleDuration = 2.5f;
	[TextArea(3,10)]
	public string subtitle;

	public AllEvents eventsToDo = new AllEvents ();

}



[System.Serializable]
public class SoundManager : MonoBehaviour {

	public List<SoundObject> soundList = new List<SoundObject> ();



	private AudioSource audioSource;
	private GameObject eMng ;
	private GameObject iMng;
	private GameObject subtitle;
	private float timer;

	// Use this for initialization
	void Start () {
		audioSource = gameObject.GetComponent<AudioSource> ();
		eMng = GameObject.Find ("EventManager");
		iMng = GameObject.Find ("InventoryManager");
		subtitle = GameObject.Find ("Subtitles");
		timer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		playAudio ();
	}

	void handleAllEvents(SoundObject obj){
		if (obj.eventsToDo.selectedDoors.Count>0&&obj.eventsToDo.unlockSDoors) {
			obj.eventsToDo.unlockSelectedDoors ();
		}

		if(obj.eventsToDo.enableSelectedObjects&&obj.eventsToDo.objectsToActivate.Count>0) {
			for (int i = 0; i < obj.eventsToDo.objectsToActivate.Count; i++) {
				obj.eventsToDo.objectsToActivate [i].SetActive (true);
				obj.eventsToDo.objectsToActivate.Remove (obj.eventsToDo.objectsToActivate [i]);
			}
		}

		if (obj.eventsToDo.disableSelectedObjects&&obj.eventsToDo.objectsToDeactivate.Count>0) {
			for (int i = 0; i <obj.eventsToDo.objectsToDeactivate.Count; i++) {
				obj.eventsToDo.objectsToDeactivate [i].SetActive (false);
				obj.eventsToDo.objectsToDeactivate.Remove (obj.eventsToDo.objectsToDeactivate [i]);
			}
		}
		if (obj.eventsToDo.turnOffAll) {
			obj.eventsToDo.turnOffAllLamps ();
		}

		if (obj.eventsToDo.selectedDoors.Count>0&&obj.eventsToDo.unlockSDoors) {
			obj.eventsToDo.unlockSelectedDoors ();
			for (int i = 0; i < obj.eventsToDo.selectedDoors.Count; i++) {
				obj.eventsToDo.selectedDoors.Remove (obj.eventsToDo.selectedDoors [i]);
			}
		}

	}

	public void addLines(List<SoundObject> sList){
		for (int i = 0; i < sList.Count; i++) {
			soundList.Add (sList [i]);
		}
	}


	public void handleEvent(SoundObject obj){

		if (obj.addEvent) {
			if (!eMng.GetComponent<EventManager> ().doesEventExist (obj.addEventObj.eventName)) {
				if (obj.requiredEvent) {
					if (eMng.GetComponent<EventManager> ().doesEventExist (obj.requiredEventObj.eventName)) {
						if (obj.requiredItem) {
							if (iMng.GetComponent<InventoryManager> ().isObjectInInventory (obj.invObject.objectName)) {
								eMng.GetComponent<EventManager> ().removeEvent (obj.requiredEventObj.eventName);
								eMng.GetComponent<EventManager> ().addEvent (obj.addEventObj);
							}
						} else {
							eMng.GetComponent<EventManager> ().removeEvent (obj.requiredEventObj.eventName);
							eMng.GetComponent<EventManager> ().addEvent (obj.addEventObj);
						}
					}
				} else {
					if (obj.requiredItem) {
						if (iMng.GetComponent<InventoryManager> ().isObjectInInventory (obj.invObject.objectName)) {
							eMng.GetComponent<EventManager> ().removeEvent (obj.requiredEventObj.eventName);
							eMng.GetComponent<EventManager> ().addEvent (obj.addEventObj);
						}
					} else {
						eMng.GetComponent<EventManager> ().removeEvent (obj.requiredEventObj.eventName);
						eMng.GetComponent<EventManager> ().addEvent (obj.addEventObj);
					}
				}
			}
		}
	}


	public void playAudio(){
		timer -= Time.deltaTime;
		if (soundList.Count>0) {
			if (!canPlay(soundList[0])) {
				soundList.Remove (soundList [0]);
			}
		}


		if (audioSource.mute) {
			if (timer<=0) {
				if (soundList.Count > 0) {
					if (canPlay (soundList [0])) {
						subtitle.GetComponent<Text> ().text = soundList [0].subtitle;
						subtitle.GetComponent<Text> ().enabled = true;
						timer = soundList [0].subtitleDuration;
						handleEvent (soundList [0]);
						handleAllEvents (soundList [0]);
						soundList.Remove (soundList [0]);
					} else {
						subtitle.GetComponent<Text> ().text = "";
						subtitle.GetComponent<Text> ().enabled = false;
						soundList.Remove (soundList [0]);
					}
				} else {
					subtitle.GetComponent<Text> ().text = "";
					subtitle.GetComponent<Text> ().enabled = false;
				}


			}

		}
		if (!audioSource.isPlaying&&!audioSource.mute) {
			for (int i = 0; i < soundList.Count; i++) {

				if (!canPlay (soundList [i])) {
					
					return;
				} else {
					


					if (audioSource.clip == soundList [i].audioClip && !audioSource.isPlaying) {
						soundList.RemoveAt (i);
						subtitle.GetComponent<Text> ().enabled = false;
						i = 0;
					} else {
						if (canPlay(soundList[i]) && !audioSource.isPlaying) {
							audioSource.clip = soundList [i].audioClip;
							audioSource.Play ();
							subtitle.GetComponent<Text> ().text = soundList [i].subtitle;
							subtitle.GetComponent<Text> ().enabled = true;
							handleEvent (soundList[i]);
						}
					}

				}

			}
		}
	}





	public bool canPlay(SoundObject sObj){
		//List<eventObject> eventList = GameObject.Find ("EventManager").GetComponent<EventManager> ().eventList;
		//List<inventoryObject> invList = GameObject.Find ("InventoryManager").GetComponent<InventoryManager> ().invList;
		var eventMng = GameObject.Find ("EventManager").GetComponent<EventManager> ();
		var invMng = GameObject.Find ("InventoryManager").GetComponent<InventoryManager> ();


		if (sObj.addEvent) {
			if (eventMng.doesEventExist(sObj.addEventObj.eventName)) {
				return false;
			}
		}

		if (sObj.requiredEvent) {
			if (!eventMng.doesEventExist (sObj.requiredEventObj.eventName)) {
				return false;
			}
		}

		if (sObj.requiredItem) {
			if (!invMng.isObjectInInventory(sObj.invObject.objectName)) {
				return false;
			}
		}

	
		return true;
	}


}

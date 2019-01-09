using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BookObject{
	public string bookName;
	public string pageOne;
	public bool pageTwoImage = false;
	public string pageTwo;
	public Sprite pageImage;
}


public class Interactable : MonoBehaviour {

	public string interactionText = "Press F to interact";
	public Sprite objectSprite;
	public string objectName;
	public string descriptionText;

	public bool isPickup = true;
	public bool toShowDescription = false;

	private bool showingDesc = false;
	public float timeToShowDescription=2.0f;
	private float timer;

	private Text descriptionTextObject;
	private GameObject invObj;

	private bool isInInventory = false;
	public bool addLines = false;
	public List<SoundObject> soundList = new List<SoundObject> ();



	public bool isTarget = false;
	public bool leavesOpaque = false;
	private bool canInteractWithTarget = false;



	public bool addEvent = false;
	public eventObject addEventObject = new eventObject ();
	public bool needsCurrentEvent = true;
	public eventObject currentEvent = new eventObject();
	private GameObject eventManager;
	private bool canPickup = true;


	public AllEvents ev = new AllEvents();

	public bool isBook=false;
	public BookObject bookObject = new BookObject ();





	// Use this for initialization
	void Start () {
		timer = timeToShowDescription;
		descriptionTextObject = GameObject.Find ("DescriptionText").GetComponent<Text>();
		invObj = GameObject.Find ("InventoryManager");
		eventManager = GameObject.Find ("EventManager");
	}


	public void addLinesToMain(){

		if (addLines) {
			for (int i = 0; i < soundList.Count; i++) {
				GameObject.Find ("SoundManager").GetComponent<SoundManager> ().soundList.Add (soundList [i]);
			}
		}
	}


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


	}

	public void handleEvent(){

		if (addEvent) {
			canPickup = true;
			if (needsCurrentEvent) {
				if (eventManager.GetComponent<EventManager> ().doesEventExist (currentEvent.eventName)) {
					if (currentEvent.itemRequired) {
						if (GameObject.Find ("InventoryManager").GetComponent<InventoryManager> ().isObjectInInventory (currentEvent.requiredItemName)) {
							eventManager.GetComponent<EventManager> ().addEvent (addEventObject);
							eventManager.GetComponent<EventManager> ().removeEvent (currentEvent.eventName);

						} else {
							canPickup = false;
						}
					} else {
						eventManager.GetComponent<EventManager> ().removeEvent (currentEvent.eventName);
						eventManager.GetComponent<EventManager> ().addEvent (addEventObject);
					}
				} else {
					canPickup = false;
				}
			} else {
				if (currentEvent.itemRequired) {
					if (GameObject.Find ("InventoryManager").GetComponent<InventoryManager> ().isObjectInInventory (currentEvent.requiredItemName)) {
						eventManager.GetComponent<EventManager> ().removeEvent (currentEvent.eventName);
						eventManager.GetComponent<EventManager> ().addEvent (addEventObject);
					} else {
						canPickup = false;
					}
				} else {
					eventManager.GetComponent<EventManager> ().addEvent (addEventObject);

				}
			}
		}

	}



	public void doInteraction(){
		

		if (isTarget) {
			if (currentEvent.itemRequired) {
				if (invObj.GetComponent<InventoryManager> ().isObjectInInventory (currentEvent.requiredItemName)) {
					invObj.GetComponent<InventoryManager> ().removeFromInventory (currentEvent.requiredItemName);
					canInteractWithTarget = true;
				}
			}

			if (needsCurrentEvent) {
				if (eventManager.GetComponent<EventManager> ().doesEventExist (currentEvent.eventName)) {
					canInteractWithTarget = true;
				}
			}

			if (canInteractWithTarget) {
				if (leavesOpaque) {
					gameObject.transform.parent.Find ("Opaque").gameObject.SetActive (true);
					eventManager.GetComponent<EventManager> ().removeEvent (currentEvent.eventName);
					addLinesToMain ();
					gameObject.SetActive(false);
				} else {
					eventManager.GetComponent<EventManager> ().removeEvent (currentEvent.eventName);
					addLinesToMain ();
					gameObject.SetActive(false);
				}
			}

		}


		handleEvent ();
		if (isPickup&&canPickup) {
			addToInventory ();
			addLinesToMain ();
		}
		if (toShowDescription&&showingDesc!=true) {
			showingDesc = true;
			timer = timeToShowDescription;
			descriptionTextObject.text = descriptionText;
		}

	}

	void Update(){
		handleAllEvents ();

		if (showingDesc) {
			showDescription ();
		}
	
	}

	void showDescription(){
		timer -= Time.deltaTime;
		descriptionTextObject.enabled = true;

		if(timer<0){
			showingDesc = false;
			descriptionTextObject.text = "";
			descriptionTextObject.enabled = false;
			if (isPickup&&!isInInventory&&canPickup) {
				gameObject.SetActive (false);
			}
		}


	}


	void addToInventory(){
		if (!invObj.GetComponent<InventoryManager> ().isObjectInInventory (objectName)) {

			invObj.GetComponent<InventoryManager> ().addToInv (objectName, descriptionText);
			pickUp ();
		} else {
			isInInventory = true;
			toShowDescription = true;
			showingDesc = true;
			timer = timeToShowDescription;
			descriptionTextObject.text = descriptionText + " ... I already have it.";
		}
	}


	void pickUp(){



		if (gameObject.GetComponent<Collider> ()!=null) {
			gameObject.GetComponent<Collider> ().enabled = false;
		}

		if (gameObject.GetComponent<MeshRenderer> ()!=null) {
			gameObject.GetComponent<MeshRenderer> ().enabled = false;
		}


		if (gameObject.transform.childCount > 0) {
			foreach (Transform child in gameObject.transform) {
				if (child.gameObject.GetComponent<Collider> ()!=null) {
					child.gameObject.GetComponent<Collider> ().enabled = false;
				}

				if (child.gameObject.GetComponent<MeshRenderer> ()!=null) {
					child.gameObject.GetComponent<MeshRenderer> ().enabled = false;
				}
			}
		}

	}

}

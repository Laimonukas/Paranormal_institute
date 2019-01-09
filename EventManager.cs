using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class eventObject{

	public string eventName;
	[TextArea(3,10)]
	public string eventDescription;
	public Sprite eventIcon;
	public bool itemRequired;
	public string requiredItemName;

	
}


[System.Serializable]
public class EventManager : MonoBehaviour {



	public List<eventObject> completedEventsList = new List<eventObject> ();
	public List<eventObject> eventList = new List<eventObject> ();

		


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


	}



	public void enableTargetObjets(string eventName){
		GameObject[] arr = GameObject.FindGameObjectsWithTag ("InteractableParent");

		for (int i = 0; i < arr.Length; i++) {

			GameObject obj = arr [i].gameObject.transform.Find ("Transparent").gameObject;
		
			if (obj.GetComponent<Interactable>().currentEvent.eventName==eventName) {
				obj.SetActive (true);
			}





		}

		
	}


	public string returnStringEvents(){
		string str="";
		if (eventList.Count == 0) {
			return "Journal is empty";
		} else {
			foreach (var obj in eventList) {
				str += obj.eventDescription;
				str += "NEWLINE";
			}

			str = str.Replace("NEWLINE","\n");
			return str;
		}
	}



	public void removeEvent(string name){
		
		for (var i = 0; i < eventList.Count; i++) {
			if (eventList[i].eventName==name) {
				eventList.Remove (eventList[i]);
			}
		}
	}

	public bool doesEventExist(string name){
		foreach (var obj in eventList) {
			if (obj.eventName==name) {
				return true;
			}
		}
		return false; 
	}


	public void addEvent(eventObject obj){

		enableTargetObjets (obj.eventName);

		eventList.Add (obj);
	}
}

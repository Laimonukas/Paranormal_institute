using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class OptManager : MonoBehaviour {

	public bool typeOne = false;
	public List<GameObject> roomList =new List<GameObject>();
	public float distance;
	public bool lightsOn = true;
	public bool typeTwo = false;
	public List<GameObject> roomQuadList;


	public GameObject currentRoom = null;

	// Use this for initialization
	void Start () {
		roomQuadList = new List<GameObject> (GameObject.FindGameObjectsWithTag ("RoomQuad"));
		roomList = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Room"));
	}


	public bool isPlayerWithinDistance(Transform obj){
		float minDist=distance; 

		for (int i = 0; i < obj.childCount; i++) {
			if (obj.GetChild(i).tag=="RoomQuad") {
				var newDist = Vector3.Distance (transform.position, obj.GetChild (i).transform.position);
				if (newDist<minDist) {
					minDist = newDist;
				}
			}
		}
		if (minDist < distance) {
			return true;
		} else {
			return false;
		}
	}


	public bool hasMultipleQuads(Transform obj){
		int count = 0;
		for (int i = 0; i < obj.childCount; i++) {
			if (obj.GetChild(i).tag=="RoomQuad") {
				count++;
			}
		}
		if (count > 1) {
			return true;
		} else {
			return false;
		}
	}

	//Update is called once per frame
	void Update () {



		if (typeOne) {

			if (currentRoom!=null) {

				for (int i = 0; i < roomList.Count; i++) {
					if (currentRoom!=roomList[i]) {
						if (roomList[i] == true) {
							roomList[i].SetActive (false);
						}
					}
				}

			}
		}



		if (currentRoom==null) {
			if (typeTwo) {
				typeOne = false;
				for (int i = 0; i < roomQuadList.Count; i++) {
					if (isPlayerWithinDistance (roomQuadList [i].transform.parent.transform)) {
						if (roomQuadList [i].transform.parent.gameObject.activeSelf == false) {
							roomQuadList [i].transform.parent.gameObject.SetActive (true);
							if (!lightsOn) {
								GameObject.Find ("AllEvents").GetComponent<Events> ().ev.turnOffAllLamps ();

							}
						}
					} else {
						if (roomQuadList [i].transform.parent.gameObject.activeSelf == true) {
							roomQuadList [i].transform.parent.gameObject.SetActive (false);
						}

					}

					/*
					if (Vector3.Distance (transform.position, roomQuadList [i].transform.position) > distance) {
						if (hasMultipleQuads (roomQuadList [i].transform.parent.transform)) {
							
						} else {
							if (roomQuadList [i].transform.parent.gameObject.activeSelf == true) {
								roomQuadList [i].transform.parent.gameObject.SetActive (false);
							}
						}

					} else {
						if (roomQuadList [i].transform.parent.gameObject.activeSelf == false) {
							roomQuadList [i].transform.parent.gameObject.SetActive (true);
						}
					}
					*/
				}
			}
		}



	}

	
}

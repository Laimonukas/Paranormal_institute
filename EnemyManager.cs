using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class EnemyManager : MonoBehaviour {

	private NavMeshAgent agent;
	private GameObject playerObject;
	public float detectionRange = 5.0f;
	public float rayDistance = 25.0f;
	public float moveSpeed = 10.0f;
	public float noticeAngle = 25.0f;
	public float playerLookAngle = 25.0f;
	private bool movePlayerCloser = false;
	private List<Vector3> waypointList = new List<Vector3> ();
	private GameObject camObject;
	private float sensX;
	private float sensY;

	private Vector3 target;
	private bool playerFound = false;

	public bool debugMode = false;
	public GameObject debugLight;
	public GameObject eyes;

	public AllEvents ev = new AllEvents ();





	// Use this for initialization
	void Start () {
		agent = gameObject.GetComponent<NavMeshAgent> ();
		playerObject = GameObject.FindGameObjectWithTag ("Player");
		camObject = GameObject.Find ("Main Camera");
		sensX = camObject.GetComponent<MouseLook> ().sensitivityX;
		sensY = camObject.GetComponent<MouseLook> ().sensitivityY;
		for (int i = 0; i < GameObject.FindGameObjectsWithTag("Waypoint").Length; i++) {
			waypointList.Add (GameObject.FindGameObjectsWithTag ("Waypoint") [i].transform.position);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		RaycastHit hit;

		var direction = playerObject.transform.position - transform.position;

		if (Physics.Raycast (transform.position,  direction, out hit, rayDistance)) {
			movePlayerCloser = false;
			if (hit.collider.tag == "Player") {
				movePlayerCloser = true;




				if (Vector3.Angle (transform.forward, direction) < noticeAngle||Vector3.Distance (transform.position, playerObject.transform.position) <10) {
					Debug.DrawRay (transform.position, direction * hit.distance, Color.green);

					if (Vector3.Distance (transform.position, playerObject.transform.position) <5) {
						Debug.Log ("Caught");
						GameObject.Find ("BlackBackground").GetComponent<Animator> ().Play ("FadeFromBlack",0,0f);
						playerObject.transform.position = GameObject.FindGameObjectWithTag ("Checkpoint").transform.position;
						playerFound = false;
						eyes.SetActive (false);
						camObject.GetComponent<MouseLook> ().sensitivityX = sensX;
						camObject.GetComponent<MouseLook> ().sensitivityY = sensY;
						gameObject.transform.position = getFurthestWaypoint ();
						target = getTarget ();
					}
					if (debugMode&&debugLight!=null) {
						debugLight.GetComponent<Light> ().enabled = true;
						debugLight.GetComponent<Light> ().color = Color.green;
					}
					target = playerObject.transform.position;
					playerFound = true;
					eyes.SetActive (true);
					camObject.GetComponent<MouseLook> ().sensitivityX = -2.0f;
					camObject.GetComponent<MouseLook> ().sensitivityY = -2.0f;
				} else {
					Debug.DrawRay (transform.position, direction * hit.distance, Color.yellow);
					//target = getTarget ();
					if (debugMode&&debugLight!=null) {
						debugLight.GetComponent<Light> ().enabled = true;
						debugLight.GetComponent<Light> ().color = Color.yellow;
					}
					playerFound = false;
					eyes.SetActive (false);
					camObject.GetComponent<MouseLook> ().sensitivityX = sensX;
					camObject.GetComponent<MouseLook> ().sensitivityY = sensY;
				}




			} else {
				camObject.GetComponent<MouseLook> ().sensitivityX = sensX;
				camObject.GetComponent<MouseLook> ().sensitivityY = sensY;
				playerFound = false;
				eyes.SetActive (false);
			}

		} else {
			movePlayerCloser = false;
			playerFound = false;
			eyes.SetActive (false);
			if (debugMode&&debugLight!=null) {
				debugLight.GetComponent<Light> ().enabled = false;
			}
			camObject.GetComponent<MouseLook> ().sensitivityX = sensX;
			camObject.GetComponent<MouseLook> ().sensitivityY = sensY;
			Debug.DrawRay(transform.position, direction*rayDistance, Color.red);
		}

		moveToTarget ();
	}





	Vector3 getFurthestWaypoint(){
		float max = Vector3.Distance(waypointList[0],playerObject.transform.position);
		Vector3 furthest = waypointList [0];
		for (int i = 1; i < waypointList.Count; i++) {
			if (Vector3.Distance(waypointList[i],playerObject.transform.position)>max) {
				furthest = waypointList [i];
				max = Vector3.Distance (waypointList [i], playerObject.transform.position);
			}
		}
		return furthest;
	}

	void FixedUpdate(){
		var direction = transform.position - playerObject.transform.position;




		if (movePlayerCloser&&playerFound) {
			
			drawPlayerIn (direction);
		}
	}



	void moveToTarget(){
		if (target != null) {
			if (Vector3.Distance (transform.position, target) < 5 &&target!=playerObject.transform.position) {
				target = getTarget ();
			} else {
				agent.destination =target;
			}
		} else {
			target = getTarget ();
		}
	}



	Vector3 getTarget(){
		int randomInt = Random.Range (0, waypointList.Count);
		return waypointList [randomInt];
	}


	void drawPlayerIn(Vector3 dir){
		
		playerObject.transform.Translate (dir * moveSpeed * Time.deltaTime);

	}




}

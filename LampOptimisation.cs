using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampOptimisation : MonoBehaviour {
	public float distance = 25.0f;
	private RaycastHit hit;
	private int layerMask;
	private GameObject player;
	private AllEvents ev = new  AllEvents();

	// Use this for initialization
	void Start () {
		layerMask = 1 << 12;
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	// Update is called once per frame
	void Update () {
		if (player != null) {
			RaycastHit hit;
			Vector3 dir = player.transform.position - transform.position;
			if (player.GetComponent<OptManager> ().lightsOn) {
				if (Physics.Raycast (transform.position, dir, out hit, distance, layerMask)) {
					Debug.DrawRay (transform.position, dir * hit.distance, Color.yellow);
					for (int i = 0; i < GetComponentsInChildren<Light> ().Length; i++) {
						GetComponentsInChildren<Light> () [i].enabled = true;
					}
				} else {
					Debug.DrawRay (transform.position, dir * distance, Color.white);
					for (int i = 0; i < GetComponentsInChildren<Light> ().Length; i++) {

						GetComponentsInChildren<Light> () [i].enabled = false;
					}

				}
			} else {
				player.transform.Find ("AllEvents").GetComponent<Events> ().ev.turnOffAllLamps ();
				enabled = false;
			}

		} else {
			enabled = false;
		}

			
	}

}

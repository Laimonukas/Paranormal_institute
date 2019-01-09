using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InteractManager : MonoBehaviour {

	public float raycastDistance = 20.0f;

	private Text interactionText;

	private GameObject lastHitInteractObject;

	private bool canInteractWithObject=false;


	private bool invOpen = false;
	private bool journalOpen = false;

	private Text infoName;
	private Text infoDesc;
	public GameObject infoPad;


	private bool bookOpen = false;
	private bool deliveryComplete = false;
	AllEvents ev = new AllEvents();

	void Start () {



		interactionText = GameObject.Find ("InteractText").GetComponent<Text>();


	


		
	}
	
	// Update is called once per frame
	void Update () {
		int layerMask = 1 << 9;
		RaycastHit hit;

		Physics.Raycast (transform.position, transform.TransformDirection (Vector3.forward), out hit, raycastDistance, layerMask);
		if (hit.collider != null) {
			Debug.DrawRay (transform.position, transform.TransformDirection (Vector3.forward) * hit.distance, Color.yellow);

		} else {
			Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * raycastDistance, Color.white);
		}

		computerRaycast (hit);
		objectRaycast(hit);

		openBookToRead (hit);

		buttonPress ();
		openPad ();
	}


	void FixedUpdate(){
		closeBook ();
	}

	void closeBook(){
		if (bookOpen) {
			if (Input.anyKeyDown) {
				GameObject.Find ("PageTwo").GetComponent<Text> ().enabled = false;
				GameObject.Find ("PageImage").GetComponent<Image> ().enabled = false;
				GameObject.Find ("PageOne").GetComponent<Text> ().enabled = false;
				GameObject.Find ("BookName").GetComponent<Text> ().enabled = false;
				GameObject.Find ("BookImage").GetComponent<Image> ().enabled = false;
			}
		}

	}


	void openBookToRead(RaycastHit hit){
		if (hit.collider!=null) {
			

			if (hit.collider.tag=="InteractableObject") {
				GameObject obj = hit.collider.gameObject;

				if (obj.GetComponent<Interactable>().isBook) {
					if (Input.GetButtonDown("Interact")) {
						bookOpen = true;
						BookObject bObj = obj.GetComponent<Interactable> ().bookObject;

						GameObject.Find ("BookName").GetComponent<Text> ().text = bObj.bookName;
						GameObject.Find ("BookName").GetComponent<Text> ().enabled = true;

						GameObject.Find ("PageOne").GetComponent<Text> ().text = bObj.pageOne;
						GameObject.Find ("PageOne").GetComponent<Text> ().enabled = true;



						GameObject.Find ("BookImage").GetComponent<Image> ().enabled = true;

						if (bObj.pageTwoImage) {

							GameObject.Find ("PageImage").GetComponent<Image> ().sprite = bObj.pageImage;
							GameObject.Find ("PageImage").GetComponent<Image> ().enabled = true;
							GameObject.Find ("PageTwo").GetComponent<Text> ().enabled = false;


						} else {
							GameObject.Find ("PageTwo").GetComponent<Text> ().text = bObj.pageTwo;
							GameObject.Find ("PageTwo").GetComponent<Text> ().enabled = true;
							GameObject.Find ("PageImage").GetComponent<Image> ().enabled = false;
						}

					}


				}


			}
		}
	}


	void computerRaycast(RaycastHit hit){
		if (hit.collider!=null) {

			if (hit.transform.tag == "Computer") {


				GameObject obj = hit.transform.parent.gameObject;
				screenObject sObj;
				if (hit.collider.name.Contains("Icon")) {
					if (hit.collider.GetComponentInChildren<Image> () != null) {
						hit.collider.GetComponentInChildren<Image> ().color = obj.GetComponent<ComputerInteraction>().iconColorToSet;
						if (Input.GetButtonDown("MouseLeft")) {
							for (int i = 0; i < hit.collider.transform.parent.childCount; i++) {
								if (hit.collider.transform.parent.GetChild (i).name.Contains("Icon")) {
									hit.collider.transform.parent.GetChild (i).gameObject.SetActive (false);
								}
							}
							hit.collider.transform.parent.Find ("Exit").gameObject.SetActive (true);
							sObj = obj.GetComponent<ComputerInteraction> ().returnObjectData (hit.collider.name);
							switch (sObj.type) {
							case "text":
								hit.collider.transform.parent.Find ("FileText").GetComponent<Text> ().text = sObj.objText;
								hit.collider.transform.parent.Find ("FileText").gameObject.SetActive (true);
								hit.collider.transform.parent.Find ("Image").gameObject.SetActive (false);
								break;
							case "image":
								hit.collider.transform.parent.Find ("Image").GetComponent<Image> ().sprite = sObj.objSprite;
								hit.collider.transform.parent.Find ("Image").gameObject.SetActive (true);
								hit.collider.transform.parent.Find ("FileText").gameObject.SetActive (false);
								break;
							case "event":
									if (!ev.elevatorDown && !deliveryComplete) {
										ev.callDownElevator ();
										deliveryComplete = true;
										hit.collider.transform.parent.Find ("FileText").GetComponent<Text> ().text = sObj.objText;
										hit.collider.transform.parent.Find ("FileText").gameObject.SetActive (true);
										hit.collider.transform.parent.Find ("Image").gameObject.SetActive (false);
									} else {
										if (deliveryComplete) {
											ev.callUpElevator ();
											hit.collider.transform.parent.Find ("FileText").GetComponent<Text> ().text = sObj.objText;
											hit.collider.transform.parent.Find ("FileText").gameObject.SetActive (false);
											hit.collider.transform.parent.Find ("Image").gameObject.SetActive (false);
											hit.collider.gameObject.SetActive (false);
										}
									}

							

								break;
							case "error":
								hit.collider.transform.parent.Find ("FileText").GetComponent<Text> ().text = sObj.objText;
								hit.collider.transform.parent.Find ("FileText").gameObject.SetActive (true);
								hit.collider.transform.parent.Find ("Image").gameObject.SetActive (false);
								break;
							}
							obj.GetComponent<ComputerInteraction> ().handleEvent (sObj);

						}
					}
				}

				if (hit.collider.name.Contains("Exit")) {
					hit.collider.GetComponentInChildren<Image> ().color = obj.GetComponent<ComputerInteraction>().exitColorToSet;
					if (Input.GetButtonDown("MouseLeft")) {
						hit.collider.transform.parent.Find ("Image").gameObject.SetActive (false);
						hit.collider.transform.parent.Find ("FileText").gameObject.SetActive (false);
						for (int i = 0; i < hit.collider.transform.parent.childCount; i++) {
							if (hit.collider.transform.parent.GetChild (i).name.Contains("Icon")) {
								hit.collider.transform.parent.GetChild (i).gameObject.SetActive (true);
							}

						}
						hit.collider.gameObject.SetActive (false);
					}

				}


			} else {

				for (int i = 0; i < hit.collider.GetComponentsInChildren<Image> ().Length; i++) {
					hit.collider.GetComponentsInChildren<Image> ()[i].color = Color.white;
				}


			}
		}
	}



	void openPad(){
		if (invOpen) {
			infoName = GameObject.Find ("InfoNameText").GetComponent<Text>();
			infoDesc = GameObject.Find ("InfoDescription").GetComponent<Text>();
			infoName.text="Inventory";
			infoDesc.text = GameObject.Find ("InventoryManager").GetComponent<InventoryManager>().returnStringInventory();
		}


		if (journalOpen) {
			
			infoName = GameObject.Find ("InfoNameText").GetComponent<Text>();
			infoDesc = GameObject.Find ("InfoDescription").GetComponent<Text>();
			infoName.text="Journal";
			infoDesc.text = GameObject.Find ("EventManager").GetComponent<EventManager> ().returnStringEvents ();
		}

		if (Input.GetButtonDown ("Inventory")) {
			if (invOpen) {
				invOpen = false;
				journalOpen = false;
				infoPad.SetActive (false);
			} else {
				invOpen = true;
				journalOpen = false;
				infoPad.SetActive (true);
				infoName = GameObject.Find ("InfoNameText").GetComponent<Text>();
				infoDesc = GameObject.Find ("InfoDescription").GetComponent<Text>();
				infoName.text="Inventory";
				infoDesc.text = GameObject.Find ("InventoryManager").GetComponent<InventoryManager>().returnStringInventory();
			}
		}


		if (Input.GetButtonDown ("Journal")) {
			if (journalOpen) {
				journalOpen = false;
				invOpen = false;
				infoPad.SetActive (false);
			} else {
				
				journalOpen = true;
				invOpen = false;
				infoPad.SetActive (true);
				infoName = GameObject.Find ("InfoNameText").GetComponent<Text>();
				infoDesc = GameObject.Find ("InfoDescription").GetComponent<Text>();
				infoName.text="Journal";
				infoDesc.text = GameObject.Find ("EventManager").GetComponent<EventManager> ().returnStringEvents ();
			}


		}

	}


	void buttonPress(){

		if (canInteractWithObject && Input.GetButtonDown("Interact")) {
		
			lastHitInteractObject.GetComponent<Interactable> ().doInteraction();
		
		}

	
	}



	void objectRaycast(RaycastHit hit){
		if (hit.collider != null) {
			if (hit.collider.tag == "InteractableObject") {

				if (lastHitInteractObject != null) {

					if (lastHitInteractObject != GameObject.Find (hit.collider.name)) {
						lastHitInteractObject.GetComponent<ManipulateColor> ().setDefaultColor ();
					}
				}


				lastHitInteractObject = GameObject.Find (hit.collider.name);
				lastHitInteractObject.GetComponent<ManipulateColor> ().setColor ();


				interactionText.text = lastHitInteractObject.GetComponent<Interactable> ().interactionText;
				interactionText.enabled = true;

				canInteractWithObject = true;

			} else {


				if (lastHitInteractObject != null) {
					lastHitInteractObject.GetComponent<ManipulateColor> ().setDefaultColor ();
				}

				interactionText.enabled = false;
				canInteractWithObject = false;
			}
		} else {
			if (lastHitInteractObject != null) {
				lastHitInteractObject.GetComponent<ManipulateColor> ().setDefaultColor ();
			}

			interactionText.enabled = false;
			canInteractWithObject = false;
		}



	}





}




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulateColor : MonoBehaviour {

	public Color colorToSet = Color.green;

	private Color defaultColor;

	private List<Color> childrenColors = new List<Color>();
	private List<Color> colorListToUse = new List<Color> ();

	private Renderer rend;

	private bool hasChildren = false;
	private bool parentHasRender = false;

	// Use this for initialization
	void Start () {


		if (gameObject.transform.childCount > 0) {
			hasChildren = true;
			if (gameObject.GetComponent<Renderer> ()!=null) {
				rend = gameObject.GetComponent<Renderer> ();
				childrenColors.Add (rend.materials [0].color);
				parentHasRender = true;
			} 

			foreach (Transform child in gameObject.transform) {


				if (child.gameObject.GetComponent<Renderer> ()!=null) {

					rend = child.gameObject.GetComponent<Renderer> ();
					childrenColors.Add (rend.materials [0].color);
				}

			}

		} else {
			rend = gameObject.GetComponent<Renderer> ();
			defaultColor = rend.materials[0].color;
		}


		colorListToUse = childrenColors;

	}

	public void setColor(){




		if (hasChildren) {
			if (parentHasRender) {
				rend = gameObject.GetComponent<Renderer> ();
				rend.materials [0].color = colorToSet;
			}


			foreach (Transform child in gameObject.transform) {
				if (child.gameObject.GetComponent<Renderer> ()!=null) {
					rend = child.gameObject.GetComponent<Renderer> ();
					rend.materials [0].color = colorToSet;
				}


				
			}
		
		} else {
			rend.materials [0].color = colorToSet;
		}
	
	}

	public void setDefaultColor(){



		if (hasChildren) {
			if (parentHasRender) {
			
				rend.materials [0].color = colorListToUse [0];
				colorListToUse.RemoveAt (0);
			}


			foreach (Transform child in gameObject.transform) {
				if (child.gameObject.GetComponent<Renderer> ()!=null) {
					rend = child.gameObject.GetComponent<Renderer> ();
					rend.materials [0].color = colorListToUse [0];
				}
			}
		
		
		} else {
			rend = gameObject.GetComponent<Renderer> ();
			rend.materials [0].color = defaultColor;
		
		}
	

		colorListToUse = childrenColors;
	}

}

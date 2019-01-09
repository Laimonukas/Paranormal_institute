using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class inventoryObject{
	public string objectName;
	public Sprite objectSprite;
	public string objectDescription;

}

[System.Serializable]
public class InventoryManager : MonoBehaviour {



	//inventoryObject invObj = new inventoryObject();

	public List<inventoryObject> invList = new List<inventoryObject>();


	// Use this for initialization
	void Start () {
		

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	public string returnStringInventory(){
		string str="";
		if (invList.Count == 0) {
			return "Inventory is empty";
		} else {
			foreach (var obj in invList) {
				str += obj.objectName;
				str += "NEWLINE";
			}

			str = str.Replace("NEWLINE","\n");
			return str;
		}



	}

	public bool isObjectInInventory(string name){
		foreach (var obj in invList) {
			if (obj.objectName==name) {
				return true;
			}
		}


		return false;
	}



	public void removeFromInventory(string name){

		for (var i = 0; i < invList.Count; i++) {
			if (invList[i].objectName==name) {
				invList.Remove (invList[i]);
			}
		}

	}


	public void addToInv(string name,string description,Sprite img=null){
		inventoryObject obj = new inventoryObject ();
		obj.objectName = name;
		obj.objectDescription = description;
		obj.objectSprite = img;

		invList.Add (obj);
	
	}
}

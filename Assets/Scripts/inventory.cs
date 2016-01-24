using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class inventory : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Heart display
		for(int i = 0; i < PlayerControl.instance.Inventory.Count; i++)
		{
			print (this.gameObject.transform.position);
			PlayerControl.instance.Inventory[i].transform.position = this.gameObject.transform.position;
			//PlayerControl.instance.Inventory[i].layer = 5;
				
				

		}		
		
	}
	
	
}

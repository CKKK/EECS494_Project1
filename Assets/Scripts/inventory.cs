using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class inventory : MonoBehaviour {
	public GameObject[] Inventory;
	public bool [] Inventory_bool;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		int counter = 0;
		print (Inventory.Length);
		for (int i =0; i < Inventory.Length; i++) 
		{
			if(Inventory[i].gameObject!= null && Inventory_bool[i] == true)
			{
				Inventory[i].gameObject.transform.position = new Vector3(-117f,-18f,0f);
				//S.Inventory[i].gameObject.transform.position = new Vector3(20f+counter * 20,4.8f,0);
				//S.Inventory[i].gameObject.layer = 5;
				counter++;
			}
		}
		
		
	}
	
	
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class inventory : MonoBehaviour {
	public static inventory inventory_instance;
	public GameObject[] Inventory;
	public GameObject[] Inventory_Selected;
	public GameObject[] selected;
	public static bool[] Inventory_bool = {false,false,false,false,false,false,false,false};
	public static int active_Max;
	public static int movemen_counter;
	public static int active_counter = -1;
	// Use this for initialization
	void Start () {
		inventory_instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		int counter = 0;
		if (PlayerControl.instance.boom_Count == 0)
			Inventory_bool [0] = false;
		if (movemen_counter < 0)
			movemen_counter = 0;
		if (movemen_counter > active_Max-1)
			movemen_counter = active_Max-1;
		for (int i =0; i < Inventory.Length; i++) 
		{
			if(Inventory[i].gameObject!= null)
			{
				if(Inventory_bool[i] == true)
				{
					if(movemen_counter == counter)
					{
						Inventory_Selected[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(-100,0,0);
						active_counter = i;
					}
					else
						Inventory_Selected[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(-500,0,0);
					Inventory[i].GetComponent<RectTransform>(). anchoredPosition = new Vector3(30+counter*40,0,0);
					counter++;
				}
				else
				{
					Inventory_Selected[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(-500,0,0);
					Inventory[i].GetComponent<RectTransform>(). anchoredPosition = new Vector3(-1000,0,0);
				}

			}
		}

		
		
	}
	
	
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class inventory : MonoBehaviour {
	public static inventory inventory_instance;
	public GameObject[] Inventory;
	public GameObject[] Inventory_Selected;
	public GameObject[] selected;
	public static bool[] Inventory_bool = {false,false,false,false,false,false,false,false};//0,boom 1,bow 2,boomerage
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
						Inventory_Selected[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(-80,-16,0);

						if (inventory.inventory_instance.Inventory [i].name == "boom_pic_inventory")
						{
							PlayerControl.instance.selected_weapon_prefab1 = PlayerControl.instance.weapon_Inventory [0];
							inventory_instance.selected[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(-50f,-31f,0f);
							inventory_instance.selected[1].GetComponent<RectTransform>().anchoredPosition = new Vector3(-277f,-31f,0f);
							inventory_instance.selected[2].GetComponent<RectTransform>().anchoredPosition = new Vector3(-277f,-31f,0f);
							
						}
						else if (inventory.inventory_instance.Inventory [i].name == "Bow") 
						{
							PlayerControl.instance.selected_weapon_prefab1 = PlayerControl.instance.weapon_Inventory [1];
							inventory_instance.selected[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(-277f,-31f,0f);
							inventory_instance.selected[1].GetComponent<RectTransform>().anchoredPosition = new Vector3(-50f,-31f,0f);
							inventory_instance.selected[2].GetComponent<RectTransform>().anchoredPosition = new Vector3(-277f,-31f,0f);
							
						}
						else if (inventory.inventory_instance.Inventory [i].name == "Boomerage_Inventory") 
						{
							PlayerControl.instance.selected_weapon_prefab1 = PlayerControl.instance.weapon_Inventory [2];
							inventory_instance.selected[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(-277f,-31f,0f);
							inventory_instance.selected[1].GetComponent<RectTransform>().anchoredPosition = new Vector3(-277f,-31f,0f);
							inventory_instance.selected[2].GetComponent<RectTransform>().anchoredPosition = new Vector3(-50f,-31f,0f);
							
							
						}

					}
					else
						Inventory_Selected[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(-500,0,0);
					Inventory[i].GetComponent<RectTransform>(). anchoredPosition = new Vector3(0+counter*30,-16,0);
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

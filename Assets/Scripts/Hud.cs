using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Hud : MonoBehaviour {

	public Text rupee_Text;
	public Text health_Text;
	public Text key_Text;
	// Use this for initialization



	

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		int num_Player_Rupees = PlayerControl.instance.rupee_Count;
		rupee_Text.text = ": " + num_Player_Rupees.ToString ();
		int num_Player_Key = PlayerControl.instance.key;
		key_Text.text = ": " + num_Player_Key.ToString ();
		//credit to Austin Ya

	
	}
}

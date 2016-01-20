using UnityEngine;
using System.Collections;

public class heart : MonoBehaviour {
	Sprite new_Sprite;
	public Sprite []Sprites ;
	int counter = 0;
	// Use this for initialization
	void Start () {
		new_Sprite = GetComponent<SpriteRenderer> ().sprite;
	}
	
	// Update is called once per frame
	void Update () {
		counter++;
		if (counter < 10) {
			GetComponent<SpriteRenderer> ().sprite = Sprites [0];
		} else if (counter >= 10 && counter < 20)
			GetComponent<SpriteRenderer> ().sprite = Sprites [1];
		else 
			counter = 0;
	}
}

using UnityEngine;
using System.Collections;

public class explosion : MonoBehaviour {
	int counter = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (this != null)
		{
			counter ++;
			if(counter == 20)
			{
				Destroy(this.gameObject);
			}
		}
	
	}
}

using UnityEngine;
using System.Collections;

public class bomb : MonoBehaviour {
	private int counter = 0 ;
	public GameObject explosion;
	public bool inventory_or_not;
	GameObject explosion1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (this != null && inventory_or_not == false) {
			counter ++;
			if(counter == 30)
			{
				Destroy(this.gameObject);
				explosion1 = MonoBehaviour.Instantiate(explosion,this.transform.position,Quaternion.identity) as GameObject;
			}

		}
	
	}
}

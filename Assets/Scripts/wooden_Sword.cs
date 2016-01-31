using UnityEngine;
using System.Collections;

public class wooden_Sword : MonoBehaviour {
	// Use this for initialization
	public static wooden_Sword sword_instance;
	public bool inventory_or_not;
	void Start () {
		sword_instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (this != null && inventory_or_not == false) {

			if (Camera.main.transform.position.y + 3 <= this.transform.position.y) 
			{
				PlayerControl.instance.sword_fire = false;
				Destroy(this.gameObject);
			}
			else if(Camera.main.transform.position.y - 6 >= this.transform.position.y)
			{
				PlayerControl.instance.sword_fire = false;
				Destroy(this.gameObject);
			}
			else if(Camera.main.transform.position.x - 6 >= this.transform.position.x)
			{
				PlayerControl.instance.sword_fire = false;
				Destroy(this.gameObject);
			}
			else if(Camera.main.transform.position.x + 6 <= this.transform.position.x)
			{
				PlayerControl.instance.sword_fire = false;
				Destroy(this.gameObject);
			}

		}
	
	}

	void OnTriggerEnter(Collider coll)
	{
		print (coll.gameObject.tag);
		if(coll.gameObject.tag == "Enemy")
		{
			Destroy(this.gameObject);
			coll.gameObject.GetComponent<Enemy> ().beAttacked (1, gameObject);
			PlayerControl.instance.sword_fire = false;
		}
	}
}

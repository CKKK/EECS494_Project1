using UnityEngine;
using System.Collections;

public class BossProjectile : EnemyProjectile {
	public Vector3 direction;

	public BossProjectile(): base(1){
	}

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Rigidbody> ().velocity = direction;
	}
	
	// Update is called once per frame
	void Update () {
		if (this != null) {
			if (PlayerControl.instance.health_Max == PlayerControl.instance.health_Count) 
			{

				if (Camera.main.transform.position.y + 3 <= this.transform.position.y) 
				{
					Destroy(this.gameObject);
				}
				else if(Camera.main.transform.position.y -6 >= this.transform.position.y)
				{
					Destroy(this.gameObject);
				}
				else if(Camera.main.transform.position.x-6 >= this.transform.position.x)
				{
					Destroy(this.gameObject);
				}
				else if(Camera.main.transform.position.x +6 <= this.transform.position.x)
				{
					Destroy(this.gameObject);
				}
			}
		}
	}


}

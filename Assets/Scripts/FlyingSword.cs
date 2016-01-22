using UnityEngine;
using System.Collections;

public class FlyingSword : Weapon {

	public override void AttackTimeOut () {}

	public virtual void Init (Direction direction) {
		base.Init (direction);
		if (direction == Direction.NORTH) {
			this.GetComponent<Rigidbody>().velocity = new Vector3(0,10,0);
		} else if (direction == Direction.EAST) {
			this.GetComponent<Rigidbody>().velocity = new Vector3(0,10,0);
		} else if (direction == Direction.SOUTH) {
			this.GetComponent<Rigidbody>().velocity = new Vector3(0,10,0);
		} else if (direction == Direction. WEST) 
		{
			this.GetComponent<Rigidbody>().velocity = new Vector3(0,10,0);
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

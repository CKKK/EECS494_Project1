using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public virtual void AttackTimeOut () {
		Destroy(this);
	}

	public virtual void Init (Direction direction) {
		Vector3 direction_Offset = Vector3.zero;
		Vector3 direction_Eulerangle = Vector3.zero;
		if (direction == Direction.NORTH) {
			direction_Offset = new Vector3 (0, 1, 0);
			direction_Eulerangle = new Vector3 (0, 0, 90);
		} else if (direction == Direction.EAST) {
			direction_Offset = new Vector3 (1, 0, 0);
			direction_Eulerangle = new Vector3 (0, 0, 0);
		} else if (direction == Direction.SOUTH) {
			direction_Offset = new Vector3 (0, -1, 0);
			direction_Eulerangle = new Vector3 (0, 0, 270);
		} else if (direction == Direction. WEST) 
		{
			direction_Offset = new Vector3 (-1, 0, 0);
			direction_Eulerangle = new Vector3 (0, 0, 180);
		}
		this.transform.position += direction_Offset;
		Quaternion new_Weapon_Rotaion = new Quaternion ();
		new_Weapon_Rotaion = Quaternion.Euler (direction_Eulerangle.x, direction_Eulerangle.y, direction_Eulerangle.z);
		this.transform.rotation = new_Weapon_Rotaion;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}

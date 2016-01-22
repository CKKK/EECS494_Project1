using UnityEngine;
using System.Collections;

public class GridBasedMovement{
	GameObject moving_obj;
	Direction current_direction;
	float current_speed;
	float gridMultiplyer;
	public GridBasedMovement(GameObject moving_obj_, bool halfGrid){
		this.moving_obj = moving_obj_;
		if (halfGrid) {
			gridMultiplyer = 2.0f;
		} else {
			gridMultiplyer = 1.0f;
		}
	}

	public Direction getCurrentDirection(){
		return current_direction;
	}

	public void SetDirection (Direction direction) {
		current_direction = direction;
		float x = moving_obj.transform.position.x;
		float y = moving_obj.transform.position.y;
		float rounded_x = Mathf.Round ( x * gridMultiplyer) / gridMultiplyer;
		float rounded_y = Mathf.Round ( y * gridMultiplyer) / gridMultiplyer;
		if (direction == Direction.EAST || direction == Direction.WEST) {
			moving_obj.transform.position = new Vector3 (x, rounded_y, 0);
		} else {
			moving_obj.transform.position = new Vector3 (rounded_x, y, 0);
		}
		updateVelocity ();
	}

	public void SetSpeed (float speed) {
		current_speed = speed;
		updateVelocity ();
	}

	public void Stop(){
		current_speed = 0;
		moving_obj.GetComponent<Rigidbody> ().velocity = Vector3.zero;
	}

	public void onUpdate () {
		updateVelocity ();
	}



	void updateVelocity() {
		if (current_direction == Direction.NORTH) {
			moving_obj.GetComponent<Rigidbody>().velocity = new Vector3(0,current_speed,0);
		} else if (current_direction == Direction.EAST) {
			moving_obj.GetComponent<Rigidbody>().velocity = new Vector3(current_speed,0,0);
		} else if (current_direction == Direction.SOUTH) {
			moving_obj.GetComponent<Rigidbody>().velocity = new Vector3(0,-current_speed,0);
		} else if (current_direction == Direction. WEST) {
			moving_obj.GetComponent<Rigidbody>().velocity = new Vector3(-current_speed,0,0);
		}
	}
}

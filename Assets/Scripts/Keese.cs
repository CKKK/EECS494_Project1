using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Keese : Enemy {
	public Vector3[] points;
	float timeStart = 0;
	float duration = 4;
	public int counter;
	public int stop_counter;
	enum BatState {fast, slow, stop};
	BatState bat_state;
	int animation_inverval = 10;
	int animation_counter = 0;
	public Sprite[] Sprites;

	// Use this for initialization
	public Keese(): base(1,1) {
		
	}

	protected override void Start () {
		points = new Vector3[2];
		// There is already an initial position chosen by Main.SpawnEnemy()
		//   so add it to points as the initial p0 & p1
		InitMovement(2);
		InitMovement(2);
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();

		animation_counter++;
		animation_counter %= 2*animation_inverval;
		GetComponent<SpriteRenderer> ().sprite = Sprites [animation_counter / animation_inverval];
		if (bat_state == BatState.fast) {
			float u = (Time.time - timeStart) / duration;
			if (u >= 1) {  // if u >=1...

				InitMovement (1);  // ...then initialize movement to a new point
				u = 0;
				bat_state = BatState.slow;
				animation_inverval = 20;
			}

			//		u = 1 - Mathf.Pow( 1-u, 2 );         // Apply Ease Out easing to u

			this.GetComponent<Transform> ().position = (1 - u) * points [0] + u * points [1]; // Simple linear interpolation
		} else if (bat_state == BatState.slow) {
			float u = (Time.time - timeStart) / duration;
			if (u >= 1) {  // if u >=1...

				InitMovement (0);  // ...then initialize movement to a new point
				u = 0;
				bat_state = BatState.stop;
				animation_inverval = 1000000000;
			}

			//		u = 1 - Mathf.Pow( 1-u, 2 );         // Apply Ease Out easing to u

			this.GetComponent<Transform> ().position = (1 - u) * points [0] + u * points [1]; // Simple linear interpolation
		} else {
			float u = (Time.time - timeStart) / duration;
			if (u >= 1) {  // if u >=1...

				InitMovement (2);  // ...then initialize movement to a new point
				u = 0;
				bat_state = BatState.fast;
				animation_inverval = 10;
			}

			//		u = 1 - Mathf.Pow( 1-u, 2 );         // Apply Ease Out easing to u

			this.GetComponent<Transform> ().position = (1 - u) * points [0] + u * points [1]; // Simple linear interpolation
		}
	}

	void InitMovement(int speed){
		if (speed != 0) {
			Vector3 p1 = Vector3.zero;
			float esp = 0;
			float c_bounds_min_x = Camera.main.transform.position.x - 3;
			float c_bounds_max_x = Camera.main.transform.position.x + 3;
			float c_bounds_min_y = Camera.main.transform.position.y - 4;
			float c_bounds_max_y = Camera.main.transform.position.y + 2;
			p1.x = Random.Range (c_bounds_min_x + esp, c_bounds_max_x - esp);
			p1.y = Random.Range (c_bounds_min_y + esp, c_bounds_max_y - esp);
			points [0] = points [1];  // Shift points[1] to points[0]
			points [1] = p1;         // Add p1 as points[1]
			duration = Vector3.Distance (points [0], points [1]) / speed;
			// Reset the time
			timeStart = Time.time;
		} else {
			points [0] = points [1];  // Shift points[1] to points[0]
			duration = 5;
			// Reset the time
			timeStart = Time.time;
		}
	}
}
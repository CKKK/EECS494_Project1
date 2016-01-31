using UnityEngine;
using System.Collections;

public class Keese : Enemy {
	public Vector3[] points;
	public float speed = 1;
	float timeStart = 0;
	float duration = 4;
	public int counter;
	public int stop_counter;
	public Sprite[] animation_Sprites;
	// Use this for initialization
	public Keese(): base(1,1) {
		
	}

	protected override void Start () {
		points = new Vector3[2];
		// There is already an initial position chosen by Main.SpawnEnemy()
		//   so add it to points as the initial p0 & p1
		InitMovement();
		InitMovement();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		float u = (Time.time - timeStart) / duration;
		if (u>=1) {  // if u >=1...

				InitMovement();  // ...then initialize movement to a new point
				u=0;
		}

//		u = 1 - Mathf.Pow( 1-u, 2 );         // Apply Ease Out easing to u

		this.GetComponent<Transform>().position = (1-u)*points[0] + u*points[1]; // Simple linear interpolation
	}

	void InitMovement(){
		Vector3 p1 = Vector3.zero;
		float esp = 1;
		float c_bounds_min_x = Camera.main.transform.position.x - 3;
		float c_bounds_max_x = Camera.main.transform.position.x + 3;
		float c_bounds_min_y = Camera.main.transform.position.y - 4;
		float c_bounds_max_y = Camera.main.transform.position.y + 2;
		p1.x = Random.Range(c_bounds_min_x + esp, c_bounds_max_x - esp);
		p1.y = Random.Range(c_bounds_min_y + esp, c_bounds_max_y - esp);
		points[0] = points[1];  // Shift points[1] to points[0]
		points[1] = p1;         // Add p1 as points[1]
		duration = Vector3.Distance(points[0], points[1])/speed;
		// Reset the time
		timeStart = Time.time;
	}
}
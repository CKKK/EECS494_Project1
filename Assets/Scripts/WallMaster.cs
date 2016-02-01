using UnityEngine;
using System.Collections;

public class WallMaster : Enemy {
	public float spawn_interval = 3f;
	public Sprite[] sprites;
	static float last_spawn = 0f;
	Vector3[] pos = new Vector3[4];
	int animation_interval = 5;
	int animation_dir = 0; // 0 for west 1 for east;
	int phase = 0;
	int counter = 0;
	float current_phase_start_time;
	bool catching_Link;
	bool prev_invince;
	enum WMState {catching, waiting, stop};
	WMState state;
	WMState state_mem;
	float stun_start_time;

	public WallMaster(): base(1,1) {
	}
	public override void hittenByBoomerange (GameObject collider)
	{
		if (state != WMState.stop) {
			state_mem = state;
		}
		state = WMState.stop;
		stun_start_time = Time.time;
	}
	// Use this for initialization
	protected override void Start () {
		base.Start ();
		state = WMState.waiting;
		transform.position = new Vector3 (Camera.main.transform.position.x + 10, Camera.main.transform.position.y + 10, 0);
		catching_Link = false;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if (state != WMState.stop) {
			counter++;
			counter %= animation_interval * 2;
			GetComponent<SpriteRenderer> ().sprite = sprites [animation_dir * 2 + counter / animation_interval];
			float boundYMax = Mathf.Round ((Camera.main.transform.position.y + 1) * 2) / 2f;
			float boundYMin = Mathf.Round ((Camera.main.transform.position.y - 4) * 2) / 2f;
			float boundXMax = Mathf.Round ((Camera.main.transform.position.x + 5) * 2) / 2f;
			float boundXMin = Mathf.Round ((Camera.main.transform.position.x - 5) * 2) / 2f;
			if (state == WMState.waiting && Time.time - last_spawn > spawn_interval) {
				if (PlayerControl.instance.transform.position.y > boundYMax) {
					transform.eulerAngles = new Vector3 (0, 0, 180);
					if (PlayerControl.instance.current_direction == Direction.WEST) {
						animation_dir = 1;
						startCatching (new Vector3 (PlayerControl.instance.transform.position.x - 3, Mathf.Round (PlayerControl.instance.transform.position.y) + 2, 0), Direction.SOUTH, Direction.EAST);
					} else {
						animation_dir = 0;
						startCatching (new Vector3 (PlayerControl.instance.transform.position.x + 3, Mathf.Round (PlayerControl.instance.transform.position.y) + 2, 0), Direction.SOUTH, Direction.WEST);
					}
				} else if (PlayerControl.instance.transform.position.y < boundYMin) {
					transform.eulerAngles = new Vector3 (0, 0, 0);
					if (PlayerControl.instance.current_direction == Direction.WEST) {
						animation_dir = 0;
						startCatching (new Vector3 (PlayerControl.instance.transform.position.x - 3, Mathf.Round (PlayerControl.instance.transform.position.y) - 2, 0), Direction.NORTH, Direction.EAST);
					} else {
						animation_dir = 1;
						startCatching (new Vector3 (PlayerControl.instance.transform.position.x + 3, Mathf.Round (PlayerControl.instance.transform.position.y) - 2, 0), Direction.NORTH, Direction.WEST);
					}
				} else if (PlayerControl.instance.transform.position.x > boundXMax) {
					if (PlayerControl.instance.current_direction == Direction.NORTH) {
						animation_dir = 1;
						transform.eulerAngles = new Vector3 (0, 0, 90);
						startCatching (new Vector3 (PlayerControl.instance.transform.position.x + 2, Mathf.Round (PlayerControl.instance.transform.position.y) + 3, 0), Direction.WEST, Direction.SOUTH);
					} else {
						animation_dir = 0;
						transform.eulerAngles = new Vector3 (0, 0, 270);
						startCatching (new Vector3 (PlayerControl.instance.transform.position.x + 2, Mathf.Round (PlayerControl.instance.transform.position.y) - 3, 0), Direction.WEST, Direction.NORTH);
					}
				} else if (PlayerControl.instance.transform.position.x < boundXMin) {
				

					if (PlayerControl.instance.current_direction == Direction.NORTH) {
						animation_dir = 0;
						transform.eulerAngles = new Vector3 (0, 0, 90);
						startCatching (new Vector3 (PlayerControl.instance.transform.position.x - 2, Mathf.Round (PlayerControl.instance.transform.position.y) + 3, 0), Direction.EAST, Direction.SOUTH);
					} else {
						animation_dir = 1;
						transform.eulerAngles = new Vector3 (0, 0, 270);
						startCatching (new Vector3 (PlayerControl.instance.transform.position.x - 2, Mathf.Round (PlayerControl.instance.transform.position.y) - 3, 0), Direction.EAST, Direction.NORTH);
					}
				}
			} else if (state == WMState.catching) {
				if (catching_Link) {
					PlayerControl.instance.transform.position = transform.position;
				}
				float distCovered = (Time.time - current_phase_start_time) * 1;
				float fracJourney = distCovered / (pos [phase] - pos [phase + 1]).magnitude;
				transform.position = Vector3.Lerp (pos [phase], pos [phase + 1], fracJourney);
				if (fracJourney >= 1) {
					phase++;
					current_phase_start_time = Time.time;
					if (phase >= 3) {
						phase = 0;
						if (catching_Link) {
							catching_Link = false;
							PlayerControl.instance.transform.position = new Vector3 (39.654f, 2.904f, 0f);
							Camera.main.transform.position = new Vector3 (39.51f, 6.41f, -10f);
							PlayerControl.instance.invince = prev_invince;
							PlayerControl.instance.gameObject.GetComponent<Rigidbody> ().detectCollisions = true;
							PlayerControl.instance.gameObject.GetComponent<Rigidbody> ().isKinematic = false;
						}
						state = WMState.waiting;
						transform.position = new Vector3 (Camera.main.transform.position.x + 10, Camera.main.transform.position.y + 10, 0);
					}

				}
			}
		} else {
			if (Time.time - stun_start_time > 5) {
				state = state_mem;
				current_phase_start_time += Time.time - stun_start_time;
			}
		}
	}

	void startCatching(Vector3 position, Direction first_direction, Direction second_direction){
		pos [0] = position;
		Vector3 vector1 = Vector3.zero;
		Vector3 vector2 = Vector3.zero;
		state = WMState.catching;
		last_spawn = Time.time;
		current_phase_start_time = Time.time;
		phase = 0;
		switch (first_direction) {
		case Direction.EAST:
			vector1 = new Vector3 (2, 0, 0);
			break;
		case Direction.SOUTH:
			vector1 = new Vector3 (0, -2, 0);
			break;
		case Direction.WEST:
			vector1 = new Vector3 (-2, 0, 0);
			break;
		case Direction.NORTH:
			vector1 = new Vector3 (0, 2, 0);
			break;
		}

		switch (second_direction) {
		case Direction.EAST:
			vector2 = new Vector3 (3, 0, 0);
			break;
		case Direction.SOUTH:
			vector2 = new Vector3 (0, -3, 0);
			break;
		case Direction.WEST:
			vector2 = new Vector3 (-3, 0, 0);
			break;
		case Direction.NORTH:
			vector2 = new Vector3 (0, 3, 0);
			break;
		}

		pos [1] = pos [0] + vector1;
		pos [2] = pos [1] + vector2;
		pos [3] = pos [0] + vector2;

	}
	void OnCollisionEnter(Collision coll) {
		
		if (coll.gameObject.tag == "Player") {
			PlayerControl.instance.transform.position = transform.position;
			coll.gameObject.GetComponent<Rigidbody> ().detectCollisions = false;
			coll.gameObject.GetComponent<Rigidbody> ().isKinematic = true;
			catching_Link = true;
			prev_invince = PlayerControl.instance.invince;
			PlayerControl.instance.invince = true;
		}
	}
}

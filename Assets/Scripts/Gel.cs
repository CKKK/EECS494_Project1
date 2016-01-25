using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gel : Enemy {
	public double changeDirectionProb = 0.5;
	public float speed = 0.5f;
	enum GelState {move, stop};
	GelState movingState;
	public GameObject[] detectors;

	GameObject currentMovingTowardDetector;

	public Gel(): base(1, 1) {
	}

	// Use this for initialization
	protected override void Start () {
		movingState = GelState.move;
		State normalMovementState = new EnemyMovementState (this, randomTakeStep(), speed);
		base.BehaviorStateMathine.ChangeState (normalMovementState);
	}

	// Update is called once per frame
	protected override void Update ()
	{
		//		float time_delta_fraction = Time.deltaTime / (1.0f / Application.targetFrameRate);
		base.Update ();
		base.BehaviorStateMathine.Update ();
		if (base.BehaviorStateMathine.IsFinished()) {
			if (movingState == GelState.move) {
				movingState = GelState.stop;
				State normalMovementState = new EnemyMovementState (this, this.transform.position, speed);
				base.BehaviorStateMathine.ChangeState (normalMovementState);
			} else {
				movingState = GelState.move;
				int randNum = Random.Range (0, 100);
				if (!currentMovingTowardDetector.GetComponent<Detector> ().CollideWithTile () && randNum >= changeDirectionProb * 100) {
					State normalMovementState = new EnemyMovementState (this, currentMovingTowardDetector.transform.position, speed);
					base.BehaviorStateMathine.ChangeState (normalMovementState);
				} else {
					State normalMovementState = new EnemyMovementState (this, randomTakeStep (), speed);
					base.BehaviorStateMathine.ChangeState (normalMovementState);
				}
			}
		}
	}

	Vector3 randomTakeStep(){
		List<int> avaliable_directions = new List<int>();
		for (int i = 0; i < 4; i++) {
			if (!detectors [i].GetComponent<Detector> ().CollideWithTile()) {
				avaliable_directions.Add (i);
			}
		}
		if (avaliable_directions.Count > 0) {
			int randDirection = (int)Random.Range (0, avaliable_directions.Count);
			currentMovingTowardDetector = detectors [avaliable_directions [randDirection]];
			return currentMovingTowardDetector.transform.position;
		} else {
			return transform.position;
		}


	}

	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag == "Tiles") {
			State normalMovementState = new EnemyMovementState (this, randomTakeStep (), speed);
			base.BehaviorStateMathine.ChangeState (normalMovementState);
		}
	}

	protected override void OnTriggerEnter(Collider coll) {
		base.OnTriggerEnter (coll);
		if (coll.gameObject.tag == "Weapon") {
			if (!base.invincible) {
				State stunState = new EnemyStunState (this, 15, coll.gameObject);
				BehaviorStateMathine.ChangeState (stunState);
			}
		}
	}
}

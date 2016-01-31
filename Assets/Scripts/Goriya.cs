using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Goriya : Enemy {
	public GameObject boomerang;
	public double changeDirectionProb = 0.5;
	public float attackProb = 0.1f;
	public float speed = 0.5f;
	enum GoriyaState {attack, move};
	GoriyaState state;
	public GameObject[] detectors;

	GameObject currentMovingTowardDetector;

	public Goriya(): base(2, 1) {
	}

	// Use this for initialization
	protected override void Start () {
		state = GoriyaState.move;
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
			int randNum = Random.Range (0, 100);
			if (state == GoriyaState.move && randNum < attackProb * 100) {
				state = GoriyaState.attack;
				Vector3 velocity = (currentMovingTowardDetector.transform.position - transform.position).normalized * 5;
				State attackState = new GoriyaAttackState (this, boomerang, velocity);
				base.BehaviorStateMathine.ChangeState (attackState);
			} else {
				state = GoriyaState.move;
				randNum = Random.Range (0, 100);
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
			state = GoriyaState.move;
			State normalMovementState = new EnemyMovementState (this, randomTakeStep(), speed);
			base.BehaviorStateMathine.ChangeState (normalMovementState);
		}
	}

	public override void beAttacked(int damage, GameObject collider) {
		base.beAttacked (damage, collider);
		if (!base.invincible) {
			State stunState = new EnemyStunState (this, 15, collider);
			BehaviorStateMathine.ChangeState (stunState);
		}

	}
}

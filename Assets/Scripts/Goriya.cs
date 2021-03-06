﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Goriya : Enemy {
	public GameObject boomerang;
	public double changeDirectionProb = 0.5;
	public float attackProb = 0.1f;
	public float speed = 0.5f;
	enum GoriyaState {attack, move, stop};
	GoriyaState state;
	float stun_start_time;
	GoriyaState state_mem;
	public GameObject[] detectors;

	public Sprite[] Sprites;
	GameObject currentMovingTowardDetector;
	int currentMovingTowardDetectorInd;
	int animationCounter = 0;


	public Goriya(): base(2, 1) {
	}
	public override void hittenByBoomerange (GameObject collider)
	{
		if (state != GoriyaState.stop) {
			state_mem = state;
		}
		state = GoriyaState.stop;
		stun_start_time = Time.time;
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
		if (state != GoriyaState.stop) {
			base.BehaviorStateMathine.Update ();
			animationCounter++;
			animationCounter %= 30;
			GetComponent<SpriteRenderer> ().sprite = Sprites [2 * currentMovingTowardDetectorInd + animationCounter / 15];


			if (base.BehaviorStateMathine.IsFinished ()) {
				int randNum = Random.Range (0, 100);
				if (state == GoriyaState.move && randNum < attackProb * 100) {
					state = GoriyaState.attack;
					Vector3 velocity = (currentMovingTowardDetector.transform.position - transform.position).normalized * 5;
					velocity.z = 0;
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
		} else {
			if (Time.time - stun_start_time > 5) {
				state = state_mem;
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
			currentMovingTowardDetectorInd = avaliable_directions [randDirection];
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
			if (state != GoriyaState.stop) {
				State stunState = new EnemyStunState (this, 15, collider);
				BehaviorStateMathine.ChangeState (stunState);
			}
		}

	}
}

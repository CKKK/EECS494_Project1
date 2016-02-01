using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Stalfos : Enemy {
	public double changeDirectionProb = 0.5;
	public float speed = 0.5f;
	int counter = 0;
	public GameObject[] detectors;
	public Sprite[] Stal_sprites;
	public List<GameObject> drop_item;
	GameObject currentMovingTowardDetector;
	bool stoped;
	float stun_start_time;

	public Stalfos(): base(2, 1) {
		
	}
	public override void hittenByBoomerange (GameObject collider)
	{
		stoped = true;
		stun_start_time = Time.time;
	}
	// Use this for initialization
	protected override void Start () {
		stoped = false;
		State normalMovementState = new EnemyMovementState (this, randomTakeStep(), speed);
		base.BehaviorStateMathine.ChangeState (normalMovementState);
	}

	// Update is called once per frame
	protected override void Update ()
	{
		if (!stoped) {
			counter++;
			if (counter < 5) {
				this.gameObject.GetComponent<SpriteRenderer> ().sprite = Stal_sprites [0];

			} else if (counter >= 5 && counter < 10) {
				this.gameObject.GetComponent<SpriteRenderer> ().sprite = Stal_sprites [1];

			}
			if (counter == 10)
				counter = 0;
//		float time_delta_fraction = Time.deltaTime / (1.0f / Application.targetFrameRate);
			base.Update ();
			base.BehaviorStateMathine.Update ();
			if (base.BehaviorStateMathine.IsFinished ()) {
				int randNum = Random.Range (0, 100);
				if (!currentMovingTowardDetector.GetComponent<Detector> ().CollideWithTile () && randNum >= changeDirectionProb * 100) {
				
					State normalMovementState = new EnemyMovementState (this, currentMovingTowardDetector.transform.position, speed);
					base.BehaviorStateMathine.ChangeState (normalMovementState);
				 
				} else {
					State normalMovementState = new EnemyMovementState (this, randomTakeStep (), speed);
					base.BehaviorStateMathine.ChangeState (normalMovementState);
				}


			}
		} else {
			if (Time.time - stun_start_time > 5) {
				stoped = false;
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

	public override void beAttacked(int damage, GameObject collider) {
		base.beAttacked(damage, collider);
		if (!base.invincible) {
			State stunState = new EnemyStunState (this, 15, collider);
			BehaviorStateMathine.ChangeState (stunState);
		}
	}

}

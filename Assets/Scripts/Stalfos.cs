using UnityEngine;
using System.Collections;


public class Stalfos : Enemy {
	public double changeDirectionProb = 0.5;
	public Direction StartDirection = Direction.EAST;

	float changeDirTime = 0;
	GridBasedMovement MovementController;

	public Stalfos(): base(2, 1) {
	}

	// Use this for initialization
	protected override void Start () {
		MovementController = new GridBasedMovement (gameObject);
		MovementController.SetDirection (StartDirection);
		MovementController.SetSpeed (1);
	}

	// Update is called once per frame
	protected override void Update () {
		float time_delta_fraction = Time.deltaTime / (1.0f / Application.targetFrameRate);
		base.Update ();
		MovementController.onUpdate ();
		changeDirTime += time_delta_fraction;
		if (changeDirTime >= 100) {
			changeDirTime = 0;
			if (Random.Range (0, 100) <= changeDirectionProb*100) {
				randomChangeDirection (MovementController.getCurrentDirection());
			};
		}
	}

	void randomChangeDirection(Direction currentDirection){
		Direction new_direction = currentDirection;
		while (new_direction == currentDirection) {
			double randDirection = Random.Range (0, 4);
			if (randDirection < 1) {
				new_direction = Direction.NORTH;
			} else if (randDirection < 2) {
				new_direction = Direction.EAST;
			} else if (randDirection < 3) {
				new_direction = Direction.SOUTH;
			} else {
				new_direction = Direction.WEST;
			}
		}
		MovementController.SetDirection (new_direction);

	}

	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag == "Tile") {
			randomChangeDirection (MovementController.getCurrentDirection());
		}

	}
}

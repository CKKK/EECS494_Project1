using UnityEngine;
using System.Collections;

public enum Direction {NORTH, EAST, SOUTH, WEST};
public enum EntityState {NORMAL, ATTACKING};

public class PlayerControl : MonoBehaviour {
	public float walking_Velocity = 4.0f;
	public int rupee_Count = 0;
	public int health_Count = 3;
	public int health_Max = 3;


	public Sprite[] link_run_down;
	public Sprite[] link_run_up;
	public Sprite[] link_run_right;
	public Sprite[] link_run_left;

	StateMachine animation_state_machine;
	StateMachine control_state_machine;
	
	public EntityState current_state = EntityState.NORMAL;
	public Direction current_direction = Direction.SOUTH;

	public GameObject selected_weapon_prefab;

	public static PlayerControl instance;
	// Use this for initialization
	void Start () {
		if(instance != null)
			Debug.LogError("Multiple link objects detected");
		instance = this;
		
		animation_state_machine = new StateMachine ();
		animation_state_machine.ChangeState (new StateIdleWithSprite (this, GetComponent<SpriteRenderer> (), link_run_down [0]));

		control_state_machine = new StateMachine ();
		control_state_machine.ChangeState (new StateLinkNormalMovement (this));
	}
	
	// Update is called once per frame
	void Update () {
		animation_state_machine.Update ();
		control_state_machine.Update ();
		if (control_state_machine.IsFinished ()) {
			control_state_machine.ChangeState (new StateLinkNormalMovement (this));
		}
	}
	
	void OnTriggerEnter(Collider coll)
	{
		if (coll.tag == "Rupee") {
			rupee_Count++;
			Destroy (coll.gameObject);
		} else if (coll.tag == "heart_1") { // add health
			health_Count++;
			if (health_Count > health_Max) {
				health_Count = health_Max;
			}
			Destroy (coll.gameObject);
		} else if (coll.tag == "heart_2") {
			health_Max++;//add health max
			health_Count++;
			Destroy (coll.gameObject);
		} else if (coll.tag == "rupee_2") {
			rupee_Count = rupee_Count +2;
			Destroy(coll.gameObject);
		}
	}

	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag == "Enemy") {
			Enemy enemyObj = coll.gameObject.GetComponent<Enemy>();
			health_Count -= enemyObj.getDamage ();
		}
	}
}

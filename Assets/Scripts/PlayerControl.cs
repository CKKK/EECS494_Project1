using UnityEngine;
using System.Collections;

public enum Direction {NORTH, EAST, SOUTH, WEST};
public enum EntityState {NORMAL, ATTACKING, STUNNING,DEAD,CANVAS};

public class PlayerControl : MonoBehaviour {

	public float walking_Velocity = 4.0f;
	public int rupee_Count = 0;
	public int health_Count = 3;
	public int health_Max = 3;
	public int key = 0;
	public Sprite [] sprites;
	public Sprite[] spritesfordead;
	public Sprite[] link_run_down;
	public Sprite[] link_run_up;
	public Sprite[] link_run_right;
	public Sprite[] link_run_left;
	public GridBasedMovement movement_controller;
	public Sprite[] doors;

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
		movement_controller = new GridBasedMovement (gameObject, true);
	}
	
	// Update is called once per frame
	void Update () {
		animation_state_machine.Update ();
		control_state_machine.Update ();
		if (control_state_machine.IsFinished ()) {
			control_state_machine.ChangeState (new StateLinkNormalMovement (this));

		}
		if (this.transform.position.x >= 46 && this.transform.position.x < 47 && this.transform.position.y >= 5 && this.transform.position.y < 6) 
		{
			this.transform.position = new Vector3(this.transform.position.x+3,this.transform.position.y,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x+15,Camera.main.transform.position.y,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 48 && this.transform.position.x < 49 && this.transform.position.y >= 5 && this.transform.position.y < 6) 
		{
			this.transform.position = new Vector3(this.transform.position.x-3,this.transform.position.y,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x-15,Camera.main.transform.position.y,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 32 && this.transform.position.x < 33 && this.transform.position.y >= 5 && this.transform.position.y < 6) 
		{
			this.transform.position = new Vector3(this.transform.position.x-3,this.transform.position.y,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x-15,Camera.main.transform.position.y,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 30 && this.transform.position.x < 31 && this.transform.position.y >= 5 && this.transform.position.y < 6) 
		{
			this.transform.position = new Vector3(this.transform.position.x+3,this.transform.position.y,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x+15,Camera.main.transform.position.y,Camera.main.transform.position.z);
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
			rupee_Count = rupee_Count + 2;
			Destroy (coll.gameObject);
		} else if (coll.tag == "key") {
			key++;
			Destroy (coll.gameObject);
		} else if (coll.tag == "locked") {
			if(coll.name == "039x009" || coll.name == "040x009")
			{
				GameObject.Find("039x009").GetComponent<SpriteRenderer>().sprite = doors[0];
				GameObject.Find("040x009").GetComponent<SpriteRenderer>().sprite = doors[1];

			}
		} else if (coll.gameObject.tag == "EnemyProjectile") {
			EnemyProjectile enemyProjObj = coll.gameObject.GetComponent<EnemyProjectile> ();
			health_Count -= enemyProjObj.getDamage ();
			if (health_Count > 0)
				control_state_machine.ChangeState (new LinkStunning (this, sprites, 15, coll.gameObject));
			else
				control_state_machine.ChangeState (new LinkDead (this, spritesfordead, 47));
		}
	}

	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag == "Enemy") {
			Enemy enemyObj = coll.gameObject.GetComponent<Enemy> ();
			health_Count -= enemyObj.getDamage ();
			if (health_Count > 0)
				control_state_machine.ChangeState (new LinkStunning (this, sprites, 15, coll.gameObject));
			else
				control_state_machine.ChangeState (new LinkDead (this, spritesfordead, 47));


		}

	}

}

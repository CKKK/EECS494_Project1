using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Direction {NORTH, EAST, SOUTH, WEST};
public enum EntityState {NORMAL, ATTACKING, STUNNING,DEAD,PANEL,CANVAS};
public class PlayerControl : MonoBehaviour {
	public bool invince = false;
	public float walking_Velocity = 4.0f;
	public int rupee_Count = 0;
	public int health_Count = 3;
	public int health_Max = 3;
	public int boom_Count = 0;
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
	public GameObject selected_weapon_prefab1;
	public GameObject Arrow;
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
		if (this.transform.position.x >= 38 && this.transform.position.x < 40 && this.transform.position.y >= 9 && this.transform.position.y < 10) 
		{
			this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y+2.7f,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y+12,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 38 && this.transform.position.x < 40 && this.transform.position.y >= 11 && this.transform.position.y < 12.2) 
		{
			this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y-2.7f,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y-12,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 39 && this.transform.position.x < 40 && this.transform.position.y >= 20 && this.transform.position.y < 21) 
		{
			this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y+2.7f,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y+10,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 39 && this.transform.position.x < 40 && this.transform.position.y >= 22 && this.transform.position.y < 23.1) 
		{
			this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y-2.7f,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y-10,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 32 && this.transform.position.x < 33 && this.transform.position.y >= 27 && this.transform.position.y < 27.1) 
		{
			this.transform.position = new Vector3(this.transform.position.x-3,this.transform.position.y,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x-15,Camera.main.transform.position.y,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 30 && this.transform.position.x < 31 && this.transform.position.y >= 27 && this.transform.position.y < 27.1) 
		{
			this.transform.position = new Vector3(this.transform.position.x+3,this.transform.position.y,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x+15,Camera.main.transform.position.y,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 23 && this.transform.position.x < 24 && this.transform.position.y >= 31 && this.transform.position.y < 32) 
		{
			this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y+2.7f,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y+10,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 23 && this.transform.position.x < 24 && this.transform.position.y >= 33 && this.transform.position.y < 34) 
		{
			this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y-2.7f,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y-10,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 24 && this.transform.position.x < 25 && this.transform.position.y >= 38 && this.transform.position.y < 39) 
		{
			this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y-2.7f,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y-10,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 16 && this.transform.position.x < 17 && this.transform.position.y >= 38 && this.transform.position.y < 38.1) 
		{
			this.transform.position = new Vector3(this.transform.position.x-3,this.transform.position.y,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x-15,Camera.main.transform.position.y,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 14 && this.transform.position.x < 15 && this.transform.position.y >= 38 && this.transform.position.y < 38.1) 
		{
			this.transform.position = new Vector3(this.transform.position.x+3,this.transform.position.y,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x+15,Camera.main.transform.position.y,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 30 && this.transform.position.x < 31 && this.transform.position.y >= 38 && this.transform.position.y < 38.1) 
		{
			this.transform.position = new Vector3(this.transform.position.x+3,this.transform.position.y,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x+15,Camera.main.transform.position.y,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 32 && this.transform.position.x < 33 && this.transform.position.y >= 38 && this.transform.position.y < 38.1) 
		{
			this.transform.position = new Vector3(this.transform.position.x-3,this.transform.position.y,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x-15,Camera.main.transform.position.y,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 39 && this.transform.position.x < 40 && this.transform.position.y >= 42 && this.transform.position.y < 43) 
		{
			this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y+2.7f,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y+12,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 39 && this.transform.position.x < 40 && this.transform.position.y >= 44 && this.transform.position.y < 45) 
		{
			this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y-2.7f,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y-12,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 39 && this.transform.position.x < 40 && this.transform.position.y >= 53 && this.transform.position.y < 54) 
		{
			this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y+2.7f,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y+10,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 39 && this.transform.position.x < 40 && this.transform.position.y >= 55 && this.transform.position.y < 56) 
		{
			this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y-2.7f,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y-10,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 32 && this.transform.position.x < 33 && this.transform.position.y >= 60 && this.transform.position.y < 60.1) 
		{
			this.transform.position = new Vector3(this.transform.position.x-3,this.transform.position.y,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x-15,Camera.main.transform.position.y,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 30 && this.transform.position.x < 31 && this.transform.position.y >= 60 && this.transform.position.y < 60.1) 
		{
			this.transform.position = new Vector3(this.transform.position.x+3,this.transform.position.y,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x+15,Camera.main.transform.position.y,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 46 && this.transform.position.x < 47 && this.transform.position.y >=27 && this.transform.position.y < 28) 
		{
			this.transform.position = new Vector3(this.transform.position.x+3,this.transform.position.y,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x+15,Camera.main.transform.position.y,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 48 && this.transform.position.x < 49 && this.transform.position.y >=27 && this.transform.position.y < 28) 
		{
			this.transform.position = new Vector3(this.transform.position.x-3,this.transform.position.y,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x-15,Camera.main.transform.position.y,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 46 && this.transform.position.x < 47 && this.transform.position.y >=38 && this.transform.position.y < 39) 
		{
			this.transform.position = new Vector3(this.transform.position.x+3,this.transform.position.y,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x+15,Camera.main.transform.position.y,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 48 && this.transform.position.x < 49 && this.transform.position.y >=38 && this.transform.position.y < 39) 
		{
			this.transform.position = new Vector3(this.transform.position.x-3,this.transform.position.y,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x-15,Camera.main.transform.position.y,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 62 && this.transform.position.x < 63 && this.transform.position.y >=38 && this.transform.position.y < 39) 
		{
			this.transform.position = new Vector3(this.transform.position.x+3,this.transform.position.y,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x+15,Camera.main.transform.position.y,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 64 && this.transform.position.x < 65 && this.transform.position.y >=38 && this.transform.position.y < 39) 
		{
			this.transform.position = new Vector3(this.transform.position.x-3,this.transform.position.y,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x-15,Camera.main.transform.position.y,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 71 && this.transform.position.x < 72 && this.transform.position.y >= 42 && this.transform.position.y < 43) 
		{
			this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y+2.7f,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y+12,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 71 && this.transform.position.x < 72 && this.transform.position.y >= 44 && this.transform.position.y < 45) 
		{
			this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y-2.7f,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y-12,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 78 && this.transform.position.x < 79 && this.transform.position.y >=49 && this.transform.position.y < 50) 
		{
			this.transform.position = new Vector3(this.transform.position.x+3,this.transform.position.y,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x+15,Camera.main.transform.position.y,Camera.main.transform.position.z);
		}
		if (this.transform.position.x >= 80 && this.transform.position.x < 81 && this.transform.position.y >=49 && this.transform.position.y < 50) 
		{
			this.transform.position = new Vector3(this.transform.position.x-3,this.transform.position.y,this.transform.position.z);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x-15,Camera.main.transform.position.y,Camera.main.transform.position.z);
		}

	}
	
	void OnTriggerEnter(Collider coll)
	{
		if (coll.tag == "Weapon") 
		{
			if(coll.name == "bow")
			{
				print ("Succesful added");
				Destroy(coll.gameObject);
			}
		}
		else if (coll.tag == "boom") 
		{
			Destroy(coll.gameObject);
			boom_Count++;
		}
		else if (coll.tag == "Rupee") {
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
			if (coll.name == "039x009" || coll.name == "040x009") {
				GameObject.Find ("039x009").GetComponent<SpriteRenderer> ().sprite = doors [0];
				GameObject.Find ("040x009").GetComponent<SpriteRenderer> ().sprite = doors [1];

			}
		} else if (coll.gameObject.tag == "EnemyProjectile") {
			EnemyProjectile enemyProjObj = coll.gameObject.GetComponent<EnemyProjectile> ();
			if(invince != true)
				health_Count -= enemyProjObj.getDamage ();
			if (health_Count > 0)
				control_state_machine.ChangeState (new LinkStunning (this, sprites, 15, coll.gameObject));
			else
				control_state_machine.ChangeState (new LinkDead (this, spritesfordead, 47));
		} else if (coll.tag == "locked") {
			
		}
	}

	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag == "Enemy") {
			Enemy enemyObj = coll.gameObject.GetComponent<Enemy> ();
			if(invince != true)
				health_Count -= enemyObj.getDamage ();
			if (health_Count > 0)
				control_state_machine.ChangeState (new LinkStunning (this, sprites, 15, coll.gameObject));
			else
				control_state_machine.ChangeState (new LinkDead (this, spritesfordead, 47));

		}
		if (coll.gameObject.tag == "locked") 
		{
			if(key >=1)
			{
				if(coll.gameObject.name == "039x009" || coll.gameObject.name == "040x009")
				{
					
					GameObject.Find("039x009").GetComponent<SpriteRenderer>().sprite = doors[0];
					GameObject.Find("040x009").GetComponent<SpriteRenderer>().sprite = doors[1];
					GameObject.Find("039x009").GetComponent<BoxCollider>().isTrigger = true;
					GameObject.Find("040x009").GetComponent<BoxCollider>().isTrigger = true;
					
				}
				if(coll.gameObject.name == "023x031"|| coll.gameObject.name == "024x031")
				{
					GameObject.Find("023x031").GetComponent<SpriteRenderer>().sprite = doors[0];
					GameObject.Find("024x031").GetComponent<SpriteRenderer>().sprite = doors[1];
					GameObject.Find("023x031").GetComponent<BoxCollider>().isTrigger = true;
					GameObject.Find("024x031").GetComponent<BoxCollider>().isTrigger = true;
				}
				if(coll.gameObject.name == "039x053"|| coll.gameObject.name == "040x053")
				{
					GameObject.Find("039x053").GetComponent<SpriteRenderer>().sprite = doors[0];
					GameObject.Find("040x053").GetComponent<SpriteRenderer>().sprite = doors[1];
					GameObject.Find("039x053").GetComponent<BoxCollider>().isTrigger = true;
					GameObject.Find("040x053").GetComponent<BoxCollider>().isTrigger = true;
				}
				if(coll.gameObject.name == "071x042"|| coll.gameObject.name == "072x042")
				{
					GameObject.Find("071x042").GetComponent<SpriteRenderer>().sprite = doors[0];
					GameObject.Find("072x042").GetComponent<SpriteRenderer>().sprite = doors[1];
					GameObject.Find("071x042").GetComponent<BoxCollider>().isTrigger = true;
					GameObject.Find("072x042").GetComponent<BoxCollider>().isTrigger = true;
				}


				if(coll.gameObject.name == "017x038")
				{
					GameObject.Find("017x038").GetComponent<SpriteRenderer>().sprite = doors[2];
					GameObject.Find("017x038").GetComponent<BoxCollider>().isTrigger = true;
					
				}
				if(coll.gameObject.name == "033x060")
				{
					GameObject.Find("033x060").GetComponent<SpriteRenderer>().sprite = doors[2];
					GameObject.Find("033x060").GetComponent<BoxCollider>().isTrigger = true;

				}
				if(coll.gameObject.name == "046x038")
				{
					GameObject.Find("046x038").GetComponent<SpriteRenderer>().sprite = doors[3];
					GameObject.Find("046x038").GetComponent<BoxCollider>().isTrigger = true;
					
				}
				key--;
				
			}
		}

	}

}

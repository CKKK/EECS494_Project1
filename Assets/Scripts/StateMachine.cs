using UnityEngine;
using System.Collections;

// State Machines are responsible for processing states, notifying them when they're about to begin or conclude, etc.
public class StateMachine
{
	private State _current_state;
	
	public void ChangeState(State new_state)
	{
		if(_current_state != null)
		{
			_current_state.OnFinish();
		}
		
		_current_state = new_state;
		// States sometimes need to reset their machine. 
		// This reference makes that possible.
		_current_state.state_machine = this;
		_current_state.OnStart();
	}
	
	public void Reset()
	{
		if(_current_state != null)
			_current_state.OnFinish();
		_current_state = null;
	}
	
	public void Update()
	{
		if(_current_state != null)
		{
			float time_delta_fraction = Time.deltaTime / (1.0f / Application.targetFrameRate);
			_current_state.OnUpdate(time_delta_fraction);
		}
	}

	public bool IsFinished()
	{
		return _current_state == null;
	}
}

// A State is merely a bundle of behavior listening to specific events, such as...
// OnUpdate -- Fired every frame of the game.
// OnStart -- Fired once when the state is transitioned to.
// OnFinish -- Fired as the state concludes.
// State Constructors often store data that will be used during the execution of the State.
public class State
{
	// A reference to the State Machine processing the state.
	public StateMachine state_machine;
	
	public virtual void OnStart() {}
	public virtual void OnUpdate(float time_delta_fraction) {} // time_delta_fraction is a float near 1.0 indicating how much more / less time this frame took than expected.
	public virtual void OnFinish() {}
	
	// States may call ConcludeState on themselves to end their processing.
	public void ConcludeState() { state_machine.Reset(); }
}

// A State that takes a renderer and a sprite, and implements idling behavior.
// The state is capable of transitioning to a walking state upon key press.
public class StateIdleWithSprite : State
{
	PlayerControl pc;
	SpriteRenderer renderer;
	Sprite sprite;
	
	public StateIdleWithSprite(PlayerControl pc, SpriteRenderer renderer, Sprite sprite)
	{
		this.pc = pc;
		this.renderer = renderer;
		this.sprite = sprite;
	}
	
	public override void OnStart()
	{
		renderer.sprite = sprite;
	}
	
	public override void OnUpdate(float time_delta_fraction)
	{
		if(pc.current_state == EntityState.ATTACKING)
			return;

		// Transition to walking animations on key press.
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.link_run_down, 6, KeyCode.DownArrow));
		}
		if (Input.GetKeyDown (KeyCode.UpArrow)) 
		{
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.link_run_up, 6, KeyCode.UpArrow));
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) 
		{
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.link_run_right, 6, KeyCode.RightArrow));

		}
		if (Input.GetKeyDown (KeyCode.LeftArrow))
		{
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.link_run_left, 6, KeyCode.LeftArrow));
		}
	}
}

// A State for playing an animation until a particular key is released.
// Good for animations such as walking.
public class StatePlayAnimationForHeldKey : State
{
	PlayerControl pc;
	SpriteRenderer renderer;
	KeyCode key;
	Sprite[] animation;
	int animation_length;
	float animation_progression;
	float animation_start_time;
	int fps;
	
	public StatePlayAnimationForHeldKey(PlayerControl pc, SpriteRenderer renderer, Sprite[] animation, int fps, KeyCode key)
	{
		this.pc = pc;
		this.renderer = renderer;
		this.key = key;
		this.animation = animation;
		this.animation_length = animation.Length;
		this.fps = fps;
		
		if(this.animation_length <= 0)
			Debug.LogError("Empty animation submitted to state machine!");
	}
	
	public override void OnStart()
	{
		animation_start_time = Time.time;
	}
	
	public override void OnUpdate(float time_delta_fraction)
	{
		if(pc.current_state == EntityState.ATTACKING)
			return;

		if(this.animation_length <= 0)
		{
			Debug.LogError("Empty animation submitted to state machine!");
			return;
		}
		
		// Modulus is necessary so we don't overshoot the length of the animation.
		int current_frame_index = ((int)((Time.time - animation_start_time) / (1.0 / fps)) % animation_length);
		renderer.sprite = animation[current_frame_index];
		
		// If another key is pressed, we need to transition to a different walking animation.
		if(Input.GetKeyDown(KeyCode.DownArrow))
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.link_run_down, 6, KeyCode.DownArrow));
		else if(Input.GetKeyDown(KeyCode.UpArrow))
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.link_run_up, 6, KeyCode.UpArrow));
		else if(Input.GetKeyDown(KeyCode.RightArrow))
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.link_run_right, 6, KeyCode.RightArrow));
		else if(Input.GetKeyDown(KeyCode.LeftArrow))
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.link_run_left, 6, KeyCode.LeftArrow));
		
		// If we detect the specified key has been released, return to the idle state.
		else if(!Input.GetKey(key))
			state_machine.ChangeState(new StateIdleWithSprite(pc, renderer, animation[1]));
	}
}


public class StateLinkNormalMovement: State
{
	PlayerControl pc;
	public StateLinkNormalMovement(PlayerControl pc)
	{
		this.pc = pc;
	}
	public override void OnUpdate (float time_delta_fraction)
	{
		float horizontal_Input = Input.GetAxis("Horizontal");
		float vertical_Input = Input.GetAxis("Vertical");
		if (horizontal_Input != 0.0f)
			vertical_Input = 0.0f;
		
		if (horizontal_Input > 0.0f)
			pc.current_direction = Direction.EAST;
		else if (horizontal_Input < 0.0f)
			pc.current_direction = Direction.WEST;
		else if (vertical_Input > 0.0f)
			pc.current_direction = Direction.NORTH;
		else if (vertical_Input < 0.0f)
			pc.current_direction = Direction.SOUTH;
		pc.movement_controller.SetSpeed (Mathf.Abs(horizontal_Input + vertical_Input) * pc.walking_Velocity * time_delta_fraction);
		pc.movement_controller.SetDirection (pc.current_direction);
		if (Input.GetKeyDown (KeyCode.S))
			state_machine.ChangeState (new StateLinkAttack (pc, pc.selected_weapon_prefab, 15));
		if (Input.GetKeyDown (KeyCode.A) && pc.rupee_Count>=1 && pc.selected_weapon_prefab1.name == "bow")
			state_machine.ChangeState (new StateLinkAttack (pc, pc.selected_weapon_prefab1, 15));
		if (Input.GetKeyDown (KeyCode.Z)) 
		{
			Time.timeScale = 0;
			state_machine.ChangeState (new LinkInventory (pc, panel.panel_instance));
		}
		if (Input.GetKeyDown (KeyCode.A) && pc.boom_Count >= 1 && pc.selected_weapon_prefab1.name == "Boom_Bomb") {
			pc.boom_Count--;
			state_machine.ChangeState (new StateLinkAttack (pc, pc.selected_weapon_prefab1, 0));
			if(pc.boom_Count == 0)
				inventory.Inventory_bool[0] = true;
		}
		if (Input.GetKeyDown (KeyCode.I)) 
		{
			if(pc.invince == false)
				pc.invince = true;
			else
				pc.invince = false;
		}

	}
}

public class StateLinkAttack : State
{
	PlayerControl pc;
	GameObject  weapon_Prefab;
	GameObject weapon_Instance;
	GameObject arrow_Instance;
	float cooldown = 0.0f;
	public StateLinkAttack(PlayerControl pc, GameObject weapon_Prefab, int cooldown)
	{
		this.pc = pc;
		this.weapon_Prefab = weapon_Prefab;
		this.cooldown = cooldown;
	}

	public override void OnStart ()
	{
		pc.current_state = EntityState.ATTACKING;
		pc.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		weapon_Instance = MonoBehaviour.Instantiate (weapon_Prefab, pc.transform.position, Quaternion.identity) as GameObject;
		arrow_Instance = MonoBehaviour.Instantiate (pc.Arrow, pc.transform.position, Quaternion.identity) as GameObject;
		Vector3 direction_Offset = Vector3.zero;
		Vector3 direction_Eulerangle = Vector3.zero;

		if (pc.current_direction == Direction.NORTH) {
			direction_Offset = new Vector3 (0, 1, 0);
			direction_Eulerangle = new Vector3 (0, 0, 90);
			if(pc.health_Count == pc.health_Max && weapon_Prefab.name == "wooden sword" && pc.sword_fire == false)
			{
				pc.sword_fire = true;
				Debug.Log (pc.sword_fire);
				weapon_Instance.GetComponent<Rigidbody>().velocity = new Vector3(0,10,0);
			}
			if(weapon_Prefab.name == "bow")
			{
				arrow_Instance.GetComponent<Rigidbody>().velocity = new Vector3(0,10,0);

			}
		} else if (pc.current_direction == Direction.EAST) {
			direction_Offset = new Vector3 (1, 0, 0);
			direction_Eulerangle = new Vector3 (0, 0, 0);
			if(pc.health_Count == pc.health_Max&& weapon_Prefab.name == "wooden sword" && pc.sword_fire == false)
			{
				pc.sword_fire = true;
				weapon_Instance.GetComponent<Rigidbody>().velocity = new Vector3(10,0,0);
			}
			if(weapon_Prefab.name == "bow")
			{
				arrow_Instance.GetComponent<Rigidbody>().velocity = new Vector3(10,0,0);
				
			}
		} else if (pc.current_direction == Direction.SOUTH) {
			direction_Offset = new Vector3 (0, -1, 0);
			direction_Eulerangle = new Vector3 (0, 0, 270);
			if(pc.health_Count == pc.health_Max&& weapon_Prefab.name == "wooden sword"&& pc.sword_fire == false)
			{
				pc.sword_fire = true;
				weapon_Instance.GetComponent<Rigidbody>().velocity = new Vector3(0,-10,0);
			}
			if(weapon_Prefab.name == "bow")
			{
				arrow_Instance.GetComponent<Rigidbody>().velocity = new Vector3(0,-10,0);
				
			}
		} else if (pc.current_direction == Direction. WEST) 
		{
			direction_Offset = new Vector3 (-1, 0, 0);
			direction_Eulerangle = new Vector3 (0, 0, 180);
			if(pc.health_Count == pc.health_Max&& weapon_Prefab.name == "wooden sword"&& pc.sword_fire == false)
			{
				pc.sword_fire = true;
				weapon_Instance.GetComponent<Rigidbody>().velocity = new Vector3(-10,0,0);
			}
			if(weapon_Prefab.name == "bow")
			{
				arrow_Instance.GetComponent<Rigidbody>().velocity = new Vector3(-10,0,0);
				
			}
		}
		Quaternion new_Weapon_Rotaion = new Quaternion ();
		new_Weapon_Rotaion = Quaternion.Euler (direction_Eulerangle.x, direction_Eulerangle.y, direction_Eulerangle.z);
		if (weapon_Prefab.name != "Boom_Bomb") {
			weapon_Instance.transform.position += direction_Offset;

			weapon_Instance.transform.rotation = new_Weapon_Rotaion;
		
		}

		if (weapon_Prefab.name == "bow") 
		{

			arrow_Instance.transform.position += direction_Offset;
			arrow_Instance.transform.rotation = new_Weapon_Rotaion;
			pc.rupee_Count--;
		}
		if (weapon_Prefab.name != "bow") 
		{
			MonoBehaviour.Destroy(arrow_Instance);
		}

	}

	public override void OnUpdate(float time_Delta_Fraction)
	{
		cooldown -= time_Delta_Fraction;
		if (cooldown <= 0)
			ConcludeState ();
	}

	public override void OnFinish ()
	{
		pc.current_state = EntityState.NORMAL;
		if (weapon_Prefab.name == "bow") 
		{
			MonoBehaviour.Destroy(weapon_Instance);
		}
		if (pc.health_Max != pc.health_Count && weapon_Prefab.name == "wooden sword")
			MonoBehaviour.Destroy (weapon_Instance);
		if (pc.sword_fire == false && weapon_Prefab.name == "wooden sword")
			MonoBehaviour.Destroy (weapon_Instance);


	}
}

public class LinkStunning : State
{
	PlayerControl pc;
	Sprite [] sprites;
	float cooldown = 0.0f;
	int counter = 0;
	GameObject Collider_obj;
	Direction attack_direction;
	public LinkStunning(PlayerControl pc, Sprite[] sprites, int cooldown, GameObject Collider_obj_)
	{
		this.pc = pc;
		this.sprites = sprites;
		this.cooldown = cooldown;
		this.Collider_obj = Collider_obj_;
	}
	
	public override void OnStart ()
	{
		pc.current_state = EntityState.STUNNING;
		Vector3 attack_vector = pc.GetComponent<Transform> ().position - Collider_obj.GetComponent<Transform> ().position;
		if (Mathf.Abs (attack_vector.x) > Mathf.Abs (attack_vector.y)) {
			attack_vector.y = 0.0f;
		} else {
			attack_vector.x = 0.0f;
		}

		if (attack_vector.x < 0.0f)
			attack_direction = Direction.EAST;
		else if (attack_vector.x > 0.0f)
			attack_direction = Direction.WEST;
		else if (attack_vector.y < 0.0f)
			attack_direction = Direction.NORTH;
		else if (attack_vector.y > 0.0f)
			attack_direction = Direction.SOUTH;

		if (attack_direction == Direction.NORTH) 
		{
			pc.GetComponent<Rigidbody>().velocity = new Vector3(0,-2,0) * pc.walking_Velocity;
		}
		else if(attack_direction == Direction.WEST) 
		{
			pc.GetComponent<Rigidbody>().velocity = new Vector3(2,0,0) * pc.walking_Velocity;

		}
		else if(attack_direction == Direction.SOUTH) 
		{
			pc.GetComponent<Rigidbody>().velocity = new Vector3(0,2,0) * pc.walking_Velocity;

		}
		else if(attack_direction == Direction.EAST) 
		{
			pc.GetComponent<Rigidbody>().velocity = new Vector3(-2,0,0) * pc.walking_Velocity;

		}

	}
	
	public override void OnUpdate(float time_Delta_Fraction)
	{
		counter++;
		if (attack_direction == Direction.NORTH) 
		{
			if(counter <5)
			{
				pc.GetComponent<SpriteRenderer>().sprite = sprites[0];
			}
			else if(counter>=5 && counter <10)
			{
				pc.GetComponent<SpriteRenderer>().sprite = sprites[1];

			}
			else
				counter = 0;
		}
		else if(attack_direction == Direction.WEST) 
		{
			if(counter <5)
			{
				pc.GetComponent<SpriteRenderer>().sprite = sprites[2];
			}
			else if(counter>=5 && counter <10)
			{
				pc.GetComponent<SpriteRenderer>().sprite = sprites[3];
				
			}
			else
				counter = 0;
		}
		else if(attack_direction == Direction.SOUTH) 
		{
			if(counter <5)
			{
				pc.GetComponent<SpriteRenderer>().sprite = sprites[4];
			}
			else if(counter>=5 && counter <10)
			{
				pc.GetComponent<SpriteRenderer>().sprite = sprites[5];
				
			}
			else
				counter = 0;
		}
		else if(attack_direction == Direction.EAST) 
		{
			if(counter <5)
			{
				pc.GetComponent<SpriteRenderer>().sprite = sprites[6];
			}
			else if(counter>=5 && counter <10)
			{
				pc.GetComponent<SpriteRenderer>().sprite = sprites[7];
				
			}
			else
				counter = 0;
		}

		cooldown -= time_Delta_Fraction;
		if (cooldown <= 0) 
		{
			if (attack_direction == Direction.NORTH) 
			{
				pc.GetComponent<SpriteRenderer>().sprite = sprites[0];

			}
			else if(attack_direction == Direction.WEST) 
			{

					pc.GetComponent<SpriteRenderer>().sprite = sprites[2];

			}
			else if(attack_direction == Direction.SOUTH) 
			{

					pc.GetComponent<SpriteRenderer>().sprite = sprites[4];

			}
			else if(attack_direction == Direction.EAST) 
			{

					pc.GetComponent<SpriteRenderer>().sprite = sprites[6];
			}
			ConcludeState ();
		}
	}
	
	public override void OnFinish ()
	{
		pc.current_state = EntityState.NORMAL;

	}
}

public class LinkDead : State
{
	PlayerControl pc;
	Sprite [] sprites;
	float cooldown = 0.0f;
	int counter = 0;
	public LinkDead(PlayerControl pc, Sprite[] spritesfordead, int cooldown)
	{
		this.pc = pc;
		this.sprites = spritesfordead;
		this.cooldown = cooldown;
	}
	
	public override void OnStart ()
	{
		pc.current_state = EntityState.DEAD;

		
	}
	
	public override void OnUpdate(float time_Delta_Fraction)
	{
		counter++;

			if(counter <5)
			{
				pc.GetComponent<SpriteRenderer>().sprite = sprites[0];
			}
			else if(counter>=5 && counter <10)
			{
				pc.GetComponent<SpriteRenderer>().sprite = sprites[1];
				
			}
			else if(counter>=10 && counter <15)
			{
				pc.GetComponent<SpriteRenderer>().sprite = sprites[2];
				
			}
			else if(counter>=15 && counter <20)
			{
				pc.GetComponent<SpriteRenderer>().sprite = sprites[3];
				
			}
			else
				counter = 0;

		
		cooldown -= time_Delta_Fraction;
		if (cooldown <= 0) 
		{

			ConcludeState ();
		}
	}
	
	public override void OnFinish ()
	{
		pc.current_state = EntityState.NORMAL;
		pc.transform.position = new Vector3(39.654f,2.904f,0f);
		Camera.main.transform.position  = new Vector3(39.51f, 6.41f, -10f);
		pc.GetComponent<SpriteRenderer> ().sprite = sprites [4];
		pc.health_Count = 3;
	}
}
/*
public class Linkcanvas : State
{
	Canvas canvas;
	PlayerControl pc;
	public Linkcanvas(PlayerControl pc,Canvas canvas)
	{
		this.canvas = canvas;
		this.pc = pc;
	}
	
	public override void OnStart ()
	{
		pc.current_state = EntityState.CANVAS;
		
		
	}
	
	public  void OnFixedUpdate(float time_Delta_Fraction)
	{

		canvas.transform.position = new Vector3 (canvas.transform.position.x, canvas.transform.position.y+10, canvas.transform.position.z);
		if (Input.GetKeyDown (KeyCode.Z))
		     
		{
			ConcludeState ();
		}
	}
	
	public override void OnFinish ()
	{
		pc.current_state = EntityState.NORMAL;

	}
}
*/


public class LinkInventory : State
{
	PlayerControl pc;
	panel panel;
	bool flag = false;
	public LinkInventory(PlayerControl pc,panel panel)
	{
		this.pc = pc;
		this.panel = panel;
	}
	
	public override void OnStart ()
	{
		pc.current_state = EntityState.PANEL;
		for (int i = 0; i < inventory.Inventory_bool.Length; i++) 
		{
			if(inventory.Inventory_bool[i] == true)
				inventory.active_Max++;
		}
		
		
	}
	
	public override void OnUpdate(float time_Delta_Fraction)
	{

		if (flag == false) 
		{
			if (panel.GetComponent<RectTransform> ().anchoredPosition.y != -206)
				panel.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (panel.GetComponent<RectTransform> ().anchoredPosition.x, panel.GetComponent<RectTransform> ().anchoredPosition.y - 4);
		}
		if (Input.GetKeyDown (KeyCode.RightArrow))
			inventory.movemen_counter++;
		if (Input.GetKeyDown (KeyCode.LeftArrow))
			inventory.movemen_counter--;
		if (Input.GetKeyDown (KeyCode.A))
		{
			if(inventory.active_counter != -1)
			{
				if (inventory.inventory_instance.Inventory [inventory.active_counter].name == "boom_pic_inventory")
				{
					pc.selected_weapon_prefab1 = pc.weapon_Inventory [0];
					inventory.inventory_instance.selected[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(-50f,-31f,0f);
					inventory.inventory_instance.selected[1].GetComponent<RectTransform>().anchoredPosition = new Vector3(-277f,-31f,0f);

					
				}
				else if (inventory.inventory_instance.Inventory [inventory.active_counter].name == "Bow") 
				{
					pc.selected_weapon_prefab1 = pc.weapon_Inventory [1];
					inventory.inventory_instance.selected[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(-277f,-31f,0f);
					inventory.inventory_instance.selected[1].GetComponent<RectTransform>().anchoredPosition = new Vector3(-50f,-31f,0f);

				}
			}
		}
		//Debug.Log(inventory.inventory_instance.Inventory[inventory.movemen_counter].name);
		if (Input.GetKeyDown (KeyCode.Z))
		{
			flag = true;

		}
		if (flag == true) 
		{
			if (panel.GetComponent<RectTransform> ().anchoredPosition.y != 114)
				panel.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (panel.GetComponent<RectTransform> ().anchoredPosition.x, panel.GetComponent<RectTransform> ().anchoredPosition.y+4);
			else
				ConcludeState();
		}

	}
	
	public override void OnFinish ()
	{
		Time.timeScale = 1;
		inventory.movemen_counter = 0;
		inventory.active_Max = 0;
		pc.current_state = EntityState.NORMAL;

	}
}

public class EnemyMovementState : State {
	Enemy enemy;
	Vector3 Destination;
	float speed;
	float timeStart;
	float duration;
	Vector3 Departure;
	public EnemyMovementState(Enemy enemy, Vector3 Destination_, float speed)
	{
		
		Destination = Destination_;
		Departure = enemy.gameObject.transform.position;
		float distance = (Departure - Destination).magnitude;
		if (distance != 0) {
			duration = distance / speed;
		} else {
			duration = 1;
		}
		timeStart = Time.time;
		this.enemy = enemy;


	}

	public override void OnStart () {
		Vector3 position = enemy.GetComponent<Transform> ().position;
		position.x = Mathf.RoundToInt (position.x);
		position.y = Mathf.RoundToInt (position.y);
		enemy.GetComponent<Transform> ().position = position;
	}

	public override void OnUpdate (float time_delta_fraction)
	{
		enemy.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		float u = (Time.time - timeStart) / duration;
		if (u >= 1) {  // if u >=1...
			Vector3 position = enemy.GetComponent<Transform> ().position;
			position.x = Mathf.RoundToInt (position.x);
			position.y = Mathf.RoundToInt (position.y);
			enemy.GetComponent<Transform> ().position = position;
			ConcludeState ();
		} else {
			enemy.GetComponent<Transform> ().position = (1 - u) * Departure + u * Destination; // Simple linear interpolation
		}

	}
}

class EnemyStunState : State{
	Enemy enemy;
//	Sprite [] sprites;
	float cooldown = 0.0f;
//	int counter = 0;
	GameObject Collider_obj;
	Direction attack_direction;
	public EnemyStunState(Enemy enemy_, int cooldown, GameObject Collider_obj_)
	{
		this.enemy = enemy_;
//		this.sprites = sprites;
		this.cooldown = cooldown;
		this.Collider_obj = Collider_obj_;
	}

	public override void OnStart ()
	{
		enemy.invincible = true;
		Vector3 attack_vector = enemy.GetComponent<Transform> ().position - Collider_obj.GetComponent<Transform> ().position;
		if (Mathf.Abs (attack_vector.x) > Mathf.Abs (attack_vector.y)) {
			attack_vector.y = 0.0f;
		} else {
			attack_vector.x = 0.0f;
		}

		if (attack_vector.x < 0.0f)
			attack_direction = Direction.EAST;
		else if (attack_vector.x > 0.0f)
			attack_direction = Direction.WEST;
		else if (attack_vector.y < 0.0f)
			attack_direction = Direction.NORTH;
		else if (attack_vector.y > 0.0f)
			attack_direction = Direction.SOUTH;

		if (attack_direction == Direction.NORTH) 
		{
			enemy.GetComponent<Rigidbody>().velocity = new Vector3(0,-5,0);
		}
		else if(attack_direction == Direction.WEST) 
		{
			enemy.GetComponent<Rigidbody>().velocity = new Vector3(5,0,0);

		}
		else if(attack_direction == Direction.SOUTH) 
		{
			enemy.GetComponent<Rigidbody>().velocity = new Vector3(0,5,0);

		}
		else if(attack_direction == Direction.EAST) 
		{
			enemy.GetComponent<Rigidbody>().velocity = new Vector3(-5,0,0);

		}
		enemy.GetComponent<SpriteRenderer> ().color = Color.red;
	}

	public override void OnUpdate(float time_Delta_Fraction)
	{
//		counter++;
//		if (attack_direction == Direction.NORTH) 
//		{
//			if(counter <5)
//			{
//				enemy.GetComponent<SpriteRenderer>().sprite = sprites[0];
//			}
//			else if(counter>=5 && counter <10)
//			{
//				enemy.GetComponent<SpriteRenderer>().sprite = sprites[1];
//
//			}
//			else
//				counter = 0;
//		}
//		else if(attack_direction == Direction.WEST) 
//		{
//			if(counter <5)
//			{
//				enemy.GetComponent<SpriteRenderer>().sprite = sprites[2];
//			}
//			else if(counter>=5 && counter <10)
//			{
//				enemy.GetComponent<SpriteRenderer>().sprite = sprites[3];
//
//			}
//			else
//				counter = 0;
//		}
//		else if(attack_direction == Direction.SOUTH) 
//		{
//			if(counter <5)
//			{
//				enemy.GetComponent<SpriteRenderer>().sprite = sprites[4];
//			}
//			else if(counter>=5 && counter <10)
//			{
//				enemy.GetComponent<SpriteRenderer>().sprite = sprites[5];
//
//			}
//			else
//				counter = 0;
//		}
//		else if(attack_direction == Direction.EAST) 
//		{
//			if(counter <5)
//			{
//				enemy.GetComponent<SpriteRenderer>().sprite = sprites[6];
//			}
//			else if(counter>=5 && counter <10)
//			{
//				enemy.GetComponent<SpriteRenderer>().sprite = sprites[7];
//
//			}
//			else
//				counter = 0;
//		}

		cooldown -= time_Delta_Fraction;
		if (cooldown <= 0) 
		{
//			if (attack_direction == Direction.NORTH) 
//			{
//				enemy.GetComponent<SpriteRenderer>().sprite = sprites[0];
//
//			}
//			else if(attack_direction == Direction.WEST) 
//			{
//
//				enemy.GetComponent<SpriteRenderer>().sprite = sprites[2];
//
//			}
//			else if(attack_direction == Direction.SOUTH) 
//			{
//
//				enemy.GetComponent<SpriteRenderer>().sprite = sprites[4];
//
//			}
//			else if(attack_direction == Direction.EAST) 
//			{
//
//				enemy.GetComponent<SpriteRenderer>().sprite = sprites[6];
//			}
			ConcludeState ();
		}
	}

	public override void OnFinish ()
	{
		enemy.invincible = false;
//		enemy.current_state = EntityState.NORMAL;
		enemy.GetComponent<SpriteRenderer> ().color = Color.white;
	}
}

public class GoriyaAttackState : State {
	Enemy enemy;
	GameObject boomerang;
	public GoriyaAttackState(Enemy enemy_, GameObject boomerang_, Vector3 velocity_) {
		enemy = enemy_;
		boomerang = GameObject.Instantiate (boomerang_, enemy.transform.position, Quaternion.Euler (Vector3.zero)) as GameObject;
		boomerang.GetComponent<Boomerange> ().throw_obj = enemy.gameObject;
		boomerang.GetComponent<Boomerange> ().velocity = velocity_;
		boomerang.tag = "EnemyProjectile";
		boomerang.layer = 12;
		Vector3 position = boomerang.GetComponent<Transform> ().position;
		position.z = -1;
		boomerang.GetComponent<Transform> ().position = position;

	}

	public override void OnStart () {
		Vector3 position = enemy.GetComponent<Transform> ().position;
		position.x = Mathf.RoundToInt (position.x);
		position.y = Mathf.RoundToInt (position.y);
		enemy.GetComponent<Transform> ().position = position;
	}

	public override void OnUpdate (float time_delta_fraction)
	{
		if (boomerang == null) {
			ConcludeState ();
		}
	}
}



// Additional recommended states:
// StateDeath
// StateDamaged
// StateWeaponSwing
// StateVictory

// Additional control states:
// LinkNormalMovement.
// LinkStunnedState.

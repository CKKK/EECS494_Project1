using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Direction {NORTH, EAST, SOUTH, WEST};
public enum EntityState {NORMAL, ATTACKING, STUNNING,DEAD,PANEL,CANVAS, VICTORY};
public class PlayerControl : MonoBehaviour {
	public bool invince = false;
	public float walking_Velocity = 4.0f;
	public float pushPower = 2.0F;
	public AudioClip[] audios;
	public int rupee_Count = 0;
	public int health_Count = 3;
	public int health_Max = 3;
	public int boom_Count = 0;
	public int key = 0;
	public int boss_kill_counter = 0;
	public bool has_boomerange = false;
	public Sprite [] link_backup_sprites;
	public Sprite [] sprites;
	public Sprite[] link_backup_spritesfordead;
	public Sprite[] spritesfordead;
	public Sprite[] link_run_down;
	public Sprite[] link_run_up;
	public Sprite[] link_run_right;
	public Sprite[] link_run_left;
	public Sprite[] link_backkup_run_down;
	public Sprite[] link_backkup_run_up;
	public Sprite[] link_backkup_run_right;
	public Sprite[] link_backkup_run_left;
	public Sprite tile;
	public bool sword_fire;
	public GridBasedMovement movement_controller;
	public Sprite[] doors;
	StateMachine animation_state_machine;
	StateMachine control_state_machine;
	public EntityState current_state = EntityState.NORMAL;
	public Direction current_direction = Direction.SOUTH;
	public int direction;
	public GameObject rock;
	public float presstime,downtime;
	public bool ready = false;
	public float countdown;
	public GameObject selected_weapon_prefab;
	public GameObject selected_weapon_prefab1;
	public GameObject[] weapon_Inventory;
	public GameObject Arrow;
	public static PlayerControl instance;
	public GameObject[] enemy_prefab;
	private GameObject trash;
	public GameObject Sword_image;
	public Dictionary<int,List<GameObject>> Room_to_Enmeies = new Dictionary<int, List<GameObject>>();
	public GameObject[] traps;
	// Use this for initialization


	public bool block(GameObject projectile){
		if (current_state == EntityState.NORMAL) {
			Direction attack_dir;
			Vector3 attack_vec = projectile.transform.position - transform.position;
			if (Mathf.Abs (attack_vec.x) > Mathf.Abs (attack_vec.y)) {
				if (attack_vec.x > 0) {
					attack_dir = Direction.EAST;
				} else {
					attack_dir = Direction.WEST;
				}
			} else {
				if (attack_vec.y > 0) {
					attack_dir = Direction.NORTH;
				} else {
					attack_dir = Direction.SOUTH;
				}
			}

			if (attack_dir == current_direction) {
				return true;
			}
				

		}
		return false;
	}

	public void takeDamage(GameObject collider) {
		if (invince != true)
			health_Count -= 1;
		if (health_Count > 0)
			control_state_machine.ChangeState (new LinkStunning (this, sprites, 15, collider));
		else
			control_state_machine.ChangeState (new LinkDead (this, spritesfordead, 47));
	}

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

		if (Application.loadedLevel == 0) {
			if (this.transform.position.x >= 32 && this.transform.position.x < 32.1 && this.transform.position.y >= 4 && this.transform.position.y < 4) {
				
			} else if (this.transform.position.x >= 46 && this.transform.position.x < 47 && this.transform.position.y >= 5 && this.transform.position.y < 6) {
				
				print ("1");
				if (Room_to_Enmeies.ContainsKey (2) == false) {
					List<GameObject> enenmy_list = new List<GameObject> ();
					Room_to_Enmeies.Add (2, enenmy_list);
					GameObject temp = Instantiate (enemy_prefab [0], new Vector3 (55, 7, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [2].Add (temp);
					temp = Instantiate (enemy_prefab [0], new Vector3 (59, 3, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [2].Add (temp);
					temp = Instantiate (enemy_prefab [0], new Vector3 (54, 8, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [2].Add (temp);
					temp = Instantiate (enemy_prefab [0], new Vector3 (52, 4, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [2].Add (temp);
					temp = Instantiate (enemy_prefab [0], new Vector3 (50, 3, 0), Quaternion.identity) as GameObject;
					temp.gameObject.GetComponent<Enemy> ().drop_kind = Drop_Kinds.key;
					Room_to_Enmeies [2].Add (temp);
				} else {
					print (Room_to_Enmeies [2].Count);
					for (int i = 0; i < Room_to_Enmeies[2].Count; i++) {
						if (Room_to_Enmeies [2] [i] != null) {
							Room_to_Enmeies [2] [i].SetActive (true);
						}
					}
					
				}
				this.transform.position = new Vector3 (this.transform.position.x + 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x + 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 48 && this.transform.position.x < 49 && this.transform.position.y >= 5 && this.transform.position.y < 6) {
				print ("2");
				for (int i = 0; i < Room_to_Enmeies[2].Count; i++) {
					if (Room_to_Enmeies [2] [i] != null)
						Room_to_Enmeies [2] [i].SetActive (false);
				}			
				this.transform.position = new Vector3 (this.transform.position.x - 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x - 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 32 && this.transform.position.x < 33 && this.transform.position.y >= 5 && this.transform.position.y < 6) {
				print ("3");
				if (Room_to_Enmeies.ContainsKey (0) == false) {
					List<GameObject> enenmy_list = new List<GameObject> ();
					Room_to_Enmeies.Add (0, enenmy_list);
					GameObject temp = Instantiate (enemy_prefab [1], new Vector3 (10, 6, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [0].Add (temp);
					temp = Instantiate (enemy_prefab [1], new Vector3 (12, 8, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [0].Add (temp);
					temp = Instantiate (enemy_prefab [1], new Vector3 (12, 4, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [0].Add (temp);
					
				} else {
					for (int i = 0; i < Room_to_Enmeies[0].Count; i++) {
						if (Room_to_Enmeies [0] [i] != null)
							Room_to_Enmeies [0] [i].SetActive (true);
					}
				}
				
				this.transform.position = new Vector3 (this.transform.position.x - 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x - 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 30 && this.transform.position.x < 31 && this.transform.position.y >= 5 && this.transform.position.y < 6) {
				print ("4");
				for (int i = 0; i < Room_to_Enmeies[0].Count; i++) {
					if (Room_to_Enmeies [0] [i] != null)
						Room_to_Enmeies [0] [i].SetActive (false);
				}	
				this.transform.position = new Vector3 (this.transform.position.x + 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x + 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 38 && this.transform.position.x < 40 && this.transform.position.y >= 9 && this.transform.position.y < 10) {
				print ("5");
				
				if (Room_to_Enmeies.ContainsKey (3) == false) {
					List<GameObject> enenmy_list = new List<GameObject> ();
					Room_to_Enmeies.Add (3, enenmy_list);
					GameObject temp = Instantiate (enemy_prefab [0], new Vector3 (34, 18, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [3].Add (temp);
					temp = Instantiate (enemy_prefab [0], new Vector3 (43, 16, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [3].Add (temp);
					temp = Instantiate (enemy_prefab [0], new Vector3 (36, 16, 0), Quaternion.identity) as GameObject;
					temp.GetComponent<Enemy> ().drop_kind = Drop_Kinds.key;
					Room_to_Enmeies [3].Add (temp);
					
				} else {
					for (int i = 0; i < Room_to_Enmeies[3].Count; i++) {
						if (Room_to_Enmeies [3] [i] != null)
							Room_to_Enmeies [3] [i].SetActive (true);
					}
				}
				this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y + 2.7f, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y + 11, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 38 && this.transform.position.x < 40 && this.transform.position.y >= 11 && this.transform.position.y < 12.2) {
				print ("6");
				for (int i = 0; i < Room_to_Enmeies[3].Count; i++) {
					if (Room_to_Enmeies [3] [i] != null)
						Room_to_Enmeies [3] [i].SetActive (false);
				}	
				
				this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y - 2.7f, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y - 11, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 39 && this.transform.position.x < 40 && this.transform.position.y >= 20 && this.transform.position.y < 21) {
				print ("7");
				
				if (Room_to_Enmeies.ContainsKey (5) == false) {
					List<GameObject> enenmy_list = new List<GameObject> ();
					Room_to_Enmeies.Add (5, enenmy_list);
					GameObject temp = Instantiate (enemy_prefab [0], new Vector3 (39, 28, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [5].Add (temp);
					temp = Instantiate (enemy_prefab [0], new Vector3 (42, 30, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [5].Add (temp);
					temp = Instantiate (enemy_prefab [0], new Vector3 (39, 27, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [5].Add (temp);
					temp = Instantiate (enemy_prefab [0], new Vector3 (36, 28, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [5].Add (temp);
					temp = Instantiate (enemy_prefab [0], new Vector3 (34, 30, 0), Quaternion.identity) as GameObject;
					temp.GetComponent<Enemy> ().drop_kind = Drop_Kinds.key;
					Room_to_Enmeies [5].Add (temp);
					
				} else {
					for (int i = 0; i < Room_to_Enmeies[5].Count; i++) {
						if (Room_to_Enmeies [5] [i] != null)
							Room_to_Enmeies [5] [i].SetActive (true);
					}
				}
				for (int i = 0; i < Room_to_Enmeies[3].Count; i++) {
					if (Room_to_Enmeies [3] [i] != null)
						Room_to_Enmeies [3] [i].SetActive (false);
				}
				this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y + 2.7f, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y + 11, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 39 && this.transform.position.x < 40 && this.transform.position.y >= 22 && this.transform.position.y < 23.1) {
				print ("7.1");
				for (int i = 0; i < Room_to_Enmeies[3].Count; i++) {
					if (Room_to_Enmeies [3] [i] != null)
						Room_to_Enmeies [3] [i].SetActive (true);
				}
				for (int i = 0; i < Room_to_Enmeies[5].Count; i++) {
					if (Room_to_Enmeies [5] [i] != null)
						Room_to_Enmeies [5] [i].SetActive (false);
				}
				
				this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y - 2.7f, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y - 11, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 32 && this.transform.position.x < 33 && this.transform.position.y >= 27 && this.transform.position.y < 27.1) {
				print ("8");
				for (int i = 0; i < Room_to_Enmeies[5].Count; i++) {
					if (Room_to_Enmeies [5] [i] != null)
						Room_to_Enmeies [5] [i].SetActive (false);
				}
				if (Room_to_Enmeies.ContainsKey (4) == false) {
					List<GameObject> enenmy_list = new List<GameObject> ();
					Room_to_Enmeies.Add (4, enenmy_list);
					GameObject temp = Instantiate (enemy_prefab [1], new Vector3 (27, 30, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [4].Add (temp);
					temp = Instantiate (enemy_prefab [1], new Vector3 (27, 24, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [4].Add (temp);
					temp = Instantiate (enemy_prefab [1], new Vector3 (18, 27, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [4].Add (temp);
					temp = Instantiate (enemy_prefab [1], new Vector3 (20, 27, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [4].Add (temp);
					temp = Instantiate (enemy_prefab [1], new Vector3 (20, 29, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [4].Add (temp);
					temp = Instantiate (enemy_prefab [1], new Vector3 (20, 25, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [4].Add (temp);
					
				} else {
					for (int i = 0; i < Room_to_Enmeies[4].Count; i++) {
						if (Room_to_Enmeies [4] [i] != null)
							Room_to_Enmeies [4] [i].SetActive (true);
					}
					
				}
				
				this.transform.position = new Vector3 (this.transform.position.x - 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x - 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 30 && this.transform.position.x < 31 && this.transform.position.y >= 27 && this.transform.position.y < 27.1) {
				print ("9");
				for (int i = 0; i < Room_to_Enmeies[4].Count; i++) {
					if (Room_to_Enmeies [4] [i] != null)
						Room_to_Enmeies [4] [i].SetActive (false);
				}
				if (Room_to_Enmeies.ContainsKey (5) == false) {
					List<GameObject> enenmy_list = new List<GameObject> ();
					Room_to_Enmeies.Add (5, enenmy_list);
					GameObject temp = Instantiate (enemy_prefab [0], new Vector3 (39, 28, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [5].Add (temp);
					temp = Instantiate (enemy_prefab [0], new Vector3 (42, 30, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [5].Add (temp);
					temp = Instantiate (enemy_prefab [0], new Vector3 (39, 27, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [5].Add (temp);
					temp = Instantiate (enemy_prefab [0], new Vector3 (36, 28, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [5].Add (temp);
					temp = Instantiate (enemy_prefab [0], new Vector3 (34, 30, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [5].Add (temp);
					
				} else {
					for (int i = 0; i < Room_to_Enmeies[5].Count; i++) {
						if (Room_to_Enmeies [5] [i] != null)
							Room_to_Enmeies [5] [i].SetActive (true);
					}
					
				}
				this.transform.position = new Vector3 (this.transform.position.x + 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x + 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 23 && this.transform.position.x < 24 && this.transform.position.y >= 31.3 && this.transform.position.y < 31.6) {
				print ("10");
				for (int i = 0; i < Room_to_Enmeies[4].Count; i++) {
					if (Room_to_Enmeies [4] [i] != null)
						Room_to_Enmeies [4] [i].SetActive (false);
				}
				if (Room_to_Enmeies.ContainsKey (8) == false) {
					List<GameObject> enenmy_list = new List<GameObject> ();
					Room_to_Enmeies.Add (8, enenmy_list);
					GameObject temp = Instantiate (enemy_prefab [3], new Vector3 (25, 41, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [8].Add (temp);
					temp = Instantiate (enemy_prefab [3], new Vector3 (21, 41, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [8].Add (temp);
					temp = Instantiate (enemy_prefab [3], new Vector3 (21, 37, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [8].Add (temp);
					
					
				} else {
					for (int i = 0; i < Room_to_Enmeies[8].Count; i++) {
						if (Room_to_Enmeies [8] [i] != null)
							Room_to_Enmeies [8] [i].SetActive (true);
					}
					
				}
				
				this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y + 2.7f, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y + 11, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 23 && this.transform.position.x < 24 && this.transform.position.y >= 33.9 && this.transform.position.y < 34) {
				print ("11");
				if (trash != null)
					Destroy (trash.gameObject);
				GameObject.Find ("023x038").GetComponent<SpriteRenderer> ().sprite = rock.GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("023x038").GetComponent<BoxCollider> ().enabled = true;
				for (int i = 0; i < Room_to_Enmeies[8].Count; i++) {
					if (Room_to_Enmeies [8] [i] != null)
						Room_to_Enmeies [8] [i].SetActive (false);
				}
				for (int i = 0; i < Room_to_Enmeies[4].Count; i++) {
					if (Room_to_Enmeies [4] [i] != null)
						Room_to_Enmeies [4] [i].SetActive (true);
				}
				
				
				this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y - 2.7f, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y - 11, Camera.main.transform.position.z);
			}
			
			if (this.transform.position.x >= 16 && this.transform.position.x < 17 && this.transform.position.y >= 38 && this.transform.position.y < 38.1) {
				print ("13");
				if (trash != null)
					Destroy (trash.gameObject);
				GameObject.Find ("023x038").GetComponent<SpriteRenderer> ().sprite = rock.GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("023x038").GetComponent<BoxCollider> ().enabled = true;
				for (int i = 0; i < Room_to_Enmeies[8].Count; i++) {
					if (Room_to_Enmeies [8] [i] != null)
						Room_to_Enmeies [8] [i].SetActive (false);
				}
				this.transform.position = new Vector3 (this.transform.position.x - 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x - 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 14 && this.transform.position.x < 15 && this.transform.position.y >= 38 && this.transform.position.y < 38.1) {
				print ("14");
				for (int i = 0; i < Room_to_Enmeies[8].Count; i++) {
					if (Room_to_Enmeies [8] [i] != null)
						Room_to_Enmeies [8] [i].SetActive (true);
				}
				this.transform.position = new Vector3 (this.transform.position.x + 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x + 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 30 && this.transform.position.x < 31 && this.transform.position.y >= 38 && this.transform.position.y < 38.1) {
				print ("15");
				if (trash != null)
					Destroy (trash.gameObject);
				GameObject.Find ("023x038").GetComponent<SpriteRenderer> ().sprite = rock.GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("023x038").GetComponent<BoxCollider> ().enabled = true;
				GameObject.Find ("017x038").GetComponent<SpriteRenderer> ().sprite = doors [4];
				GameObject.Find ("017x038").GetComponent<BoxCollider> ().isTrigger = false;
				for (int i = 0; i < Room_to_Enmeies[8].Count; i++) {
					if (Room_to_Enmeies [8] [i] != null)
						Room_to_Enmeies [8] [i].SetActive (false);
				}
				
				if (Room_to_Enmeies.ContainsKey (9) == false) {
					List<GameObject> enenmy_list = new List<GameObject> ();
					Room_to_Enmeies.Add (9, enenmy_list);
					GameObject temp = Instantiate (enemy_prefab [3], new Vector3 (37, 41, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [9].Add (temp);
					temp = Instantiate (enemy_prefab [3], new Vector3 (42, 41, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [9].Add (temp);
					temp = Instantiate (enemy_prefab [3], new Vector3 (42, 37, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [9].Add (temp);
					temp = Instantiate (enemy_prefab [3], new Vector3 (38, 37, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [9].Add (temp);
					temp = Instantiate (enemy_prefab [3], new Vector3 (41, 36, 0), Quaternion.identity) as GameObject;
					temp.GetComponent<Enemy> ().drop_kind = Drop_Kinds.key;
					Room_to_Enmeies [9].Add (temp);
				} else {
					for (int i = 0; i < Room_to_Enmeies[9].Count; i++) {
						if (Room_to_Enmeies [9] [i] != null)
							Room_to_Enmeies [9] [i].SetActive (true);
					}
					
				}
				
				this.transform.position = new Vector3 (this.transform.position.x + 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x + 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 32 && this.transform.position.x < 33 && this.transform.position.y >= 38 && this.transform.position.y < 38.1) {
				print ("16");
				for (int i = 0; i < Room_to_Enmeies[8].Count; i++) {
					if (Room_to_Enmeies [8] [i] != null)
						Room_to_Enmeies [8] [i].SetActive (true);
				}
				for (int i = 0; i < Room_to_Enmeies[9].Count; i++) {
					if (Room_to_Enmeies [9] [i] != null)
						Room_to_Enmeies [9] [i].SetActive (false);
				}
				this.transform.position = new Vector3 (this.transform.position.x - 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x - 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 39 && this.transform.position.x < 40 && this.transform.position.y >= 42 && this.transform.position.y < 43) {
				print ("17");
				for (int i = 0; i < Room_to_Enmeies[9].Count; i++) {
					if (Room_to_Enmeies [9] [i] != null)
						Room_to_Enmeies [9] [i].SetActive (false);
				}
				
				if (Room_to_Enmeies.ContainsKey (14) == false) {
					List<GameObject> enenmy_list = new List<GameObject> ();
					Room_to_Enmeies.Add (14, enenmy_list);
					GameObject temp = Instantiate (enemy_prefab [0], new Vector3 (37, 49, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [14].Add (temp);
					temp = Instantiate (enemy_prefab [0], new Vector3 (39, 50, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [14].Add (temp);
					temp = Instantiate (enemy_prefab [0], new Vector3 (42, 49, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [14].Add (temp);
					
				} else {
					for (int i = 0; i < Room_to_Enmeies[14].Count; i++) {
						if (Room_to_Enmeies [14] [i] != null)
							Room_to_Enmeies [14] [i].SetActive (true);
					}
					
				}
				this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y + 2.7f, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y + 11, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 39 && this.transform.position.x < 40 && this.transform.position.y >= 44 && this.transform.position.y < 45) {
				print ("18");
				for (int i = 0; i < Room_to_Enmeies[14].Count; i++) {
					if (Room_to_Enmeies [14] [i] != null)
						Room_to_Enmeies [14] [i].SetActive (false);
				}
				for (int i = 0; i < Room_to_Enmeies[9].Count; i++) {
					if (Room_to_Enmeies [9] [i] != null)
						Room_to_Enmeies [9] [i].SetActive (true);
				}
				this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y - 2.7f, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y - 11, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 39 && this.transform.position.x < 40 && this.transform.position.y >= 53 && this.transform.position.y < 54) {
				print ("19");
				for (int i = 0; i < Room_to_Enmeies[14].Count; i++) {
					if (Room_to_Enmeies [14] [i] != null)
						Room_to_Enmeies [14] [i].SetActive (false);
				}
				
				if (Room_to_Enmeies.ContainsKey (15) == false) {
					List<GameObject> enenmy_list = new List<GameObject> ();
					Room_to_Enmeies.Add (15, enenmy_list);
					GameObject temp = Instantiate (enemy_prefab [2], new Vector3 (38, 62, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [15].Add (temp);
					temp = Instantiate (enemy_prefab [2], new Vector3 (39, 61, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [15].Add (temp);
					temp = Instantiate (enemy_prefab [2], new Vector3 (40, 63, 0), Quaternion.identity) as GameObject;
					temp.GetComponent<Enemy> ().drop_kind = Drop_Kinds.key;
					Room_to_Enmeies [15].Add (temp);
					
				} else {
					for (int i = 0; i < Room_to_Enmeies[15].Count; i++) {
						if (Room_to_Enmeies [15] [i] != null)
							Room_to_Enmeies [15] [i].SetActive (true);
					}
					
				}
				this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y + 2.7f, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y + 11, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 39 && this.transform.position.x < 40 && this.transform.position.y >= 55 && this.transform.position.y < 56) {
				print ("20");
				for (int i = 0; i < Room_to_Enmeies[14].Count; i++) {
					if (Room_to_Enmeies [14] [i] != null)
						Room_to_Enmeies [14] [i].SetActive (true);
				}
				for (int i = 0; i < Room_to_Enmeies[15].Count; i++) {
					if (Room_to_Enmeies [15] [i] != null)
						Room_to_Enmeies [15] [i].SetActive (false);
				}
				this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y - 2.7f, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y - 11, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 32 && this.transform.position.x < 33 && this.transform.position.y >= 60 && this.transform.position.y < 60.1) {
				print ("21");
				for (int i = 0; i < Room_to_Enmeies[15].Count; i++) {
					if (Room_to_Enmeies [15] [i] != null)
						Room_to_Enmeies [15] [i].SetActive (false);
				}
				foreach (GameObject trap in traps) {
					trap.SetActive (true);
				}
				this.transform.position = new Vector3 (this.transform.position.x - 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x - 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 30 && this.transform.position.x < 31 && this.transform.position.y >= 60 && this.transform.position.y < 60.1) {
				print ("22");
				presstime = 0;
				if (trash != null)
					Destroy (trash.gameObject);
				GameObject.Find ("022x060").GetComponent<SpriteRenderer> ().sprite = rock.GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("022x060").GetComponent<BoxCollider> ().enabled = true;
				foreach (GameObject trap in traps) {
					trap.SetActive (false);
				}
				for (int i = 0; i < Room_to_Enmeies[15].Count; i++) {
					if (Room_to_Enmeies [15] [i] != null)
						Room_to_Enmeies [15] [i].SetActive (true);
				}
				this.transform.position = new Vector3 (this.transform.position.x + 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x + 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 46 && this.transform.position.x < 47 && this.transform.position.y >= 27 && this.transform.position.y < 28) {
				print ("23");
				for (int i = 0; i < Room_to_Enmeies[5].Count; i++) {
					if (Room_to_Enmeies [5] [i] != null)
						Room_to_Enmeies [5] [i].SetActive (false);
				}
				
				if (Room_to_Enmeies.ContainsKey (6) == false) {
					List<GameObject> enenmy_list = new List<GameObject> ();
					Room_to_Enmeies.Add (6, enenmy_list);
					GameObject temp = Instantiate (enemy_prefab [1], new Vector3 (53, 27, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [6].Add (temp);
					temp = Instantiate (enemy_prefab [1], new Vector3 (53, 26, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [6].Add (temp);
					temp = Instantiate (enemy_prefab [1], new Vector3 (57, 27, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [6].Add (temp);
					temp = Instantiate (enemy_prefab [1], new Vector3 (57, 25, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [6].Add (temp);
					temp = Instantiate (enemy_prefab [1], new Vector3 (59, 23, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [6].Add (temp);
					temp = Instantiate (enemy_prefab [1], new Vector3 (59, 28, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [6].Add (temp);
					temp = Instantiate (enemy_prefab [1], new Vector3 (60, 24, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [6].Add (temp);
					temp = Instantiate (enemy_prefab [1], new Vector3 (60, 23, 0), Quaternion.identity) as GameObject;
					temp.GetComponent<Enemy> ().drop_kind = Drop_Kinds.key;
					Room_to_Enmeies [6].Add (temp);
					
				} else {
					for (int i = 0; i < Room_to_Enmeies[6].Count; i++) {
						if (Room_to_Enmeies [6] [i] != null)
							Room_to_Enmeies [6] [i].SetActive (true);
					}
				}
				
				this.transform.position = new Vector3 (this.transform.position.x + 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x + 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 48 && this.transform.position.x < 49 && this.transform.position.y >= 27 && this.transform.position.y < 28) {
				print ("24");
				for (int i = 0; i < Room_to_Enmeies[6].Count; i++) {
					if (Room_to_Enmeies [6] [i] != null)
						Room_to_Enmeies [6] [i].SetActive (false);
				}
				if (Room_to_Enmeies.ContainsKey (5) == false) {
					List<GameObject> enenmy_list = new List<GameObject> ();
					Room_to_Enmeies.Add (5, enenmy_list);
					GameObject temp = Instantiate (enemy_prefab [0], new Vector3 (39, 28, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [5].Add (temp);
					temp = Instantiate (enemy_prefab [0], new Vector3 (42, 30, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [5].Add (temp);
					temp = Instantiate (enemy_prefab [0], new Vector3 (39, 27, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [5].Add (temp);
					temp = Instantiate (enemy_prefab [0], new Vector3 (36, 28, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [5].Add (temp);
					temp = Instantiate (enemy_prefab [0], new Vector3 (34, 30, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [5].Add (temp);
					
				} else {
					for (int i = 0; i < Room_to_Enmeies[5].Count; i++) {
						if (Room_to_Enmeies [5] [i] != null)
							Room_to_Enmeies [5] [i].SetActive (true);
					}
				}
				
				this.transform.position = new Vector3 (this.transform.position.x - 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x - 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 46 && this.transform.position.x < 47 && this.transform.position.y >= 38 && this.transform.position.y < 39) {
				print ("25");
				for (int i = 0; i < Room_to_Enmeies[9].Count; i++) {
					if (Room_to_Enmeies [9] [i] != null)
						Room_to_Enmeies [9] [i].SetActive (false);
				}
				
				if (Room_to_Enmeies.ContainsKey (10) == false) {
					List<GameObject> enenmy_list = new List<GameObject> ();
					Room_to_Enmeies.Add (10, enenmy_list);
					GameObject temp = Instantiate (enemy_prefab [2], new Vector3 (57, 41, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [10].Add (temp);
					temp = Instantiate (enemy_prefab [2], new Vector3 (57, 37, 0), Quaternion.identity) as GameObject;
					temp.GetComponent<Enemy> ().drop_kind = Drop_Kinds.key;
					Room_to_Enmeies [10].Add (temp);
					temp = Instantiate (enemy_prefab [2], new Vector3 (54, 36, 0), Quaternion.identity) as GameObject;
					temp.GetComponent<Enemy> ().drop_kind = Drop_Kinds.boomerang;
					Room_to_Enmeies [10].Add (temp);
					
				} else {
					for (int i = 0; i < Room_to_Enmeies[10].Count; i++) {
						if (Room_to_Enmeies [10] [i] != null)
							Room_to_Enmeies [10] [i].SetActive (true);
					}
					
				}
				
				this.transform.position = new Vector3 (this.transform.position.x + 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x + 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 48 && this.transform.position.x < 49 && this.transform.position.y >= 38 && this.transform.position.y < 39) {
				print ("26");
				for (int i = 0; i < Room_to_Enmeies[9].Count; i++) {
					if (Room_to_Enmeies [9] [i] != null)
						Room_to_Enmeies [9] [i].SetActive (true);
				}
				for (int i = 0; i < Room_to_Enmeies[10].Count; i++) {
					if (Room_to_Enmeies [10] [i] != null)
						Room_to_Enmeies [10] [i].SetActive (false);
				}
				this.transform.position = new Vector3 (this.transform.position.x - 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x - 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 62 && this.transform.position.x < 63 && this.transform.position.y >= 38 && this.transform.position.y < 39) {
				print ("27");
				for (int i = 0; i < Room_to_Enmeies[10].Count; i++) {
					if (Room_to_Enmeies [10] [i] != null)
						Room_to_Enmeies [10] [i].SetActive (false);
				}
				
				if (Room_to_Enmeies.ContainsKey (11) == false) {
					List<GameObject> enenmy_list = new List<GameObject> ();
					Room_to_Enmeies.Add (11, enenmy_list);
					GameObject temp = Instantiate (enemy_prefab [5], new Vector3 (69, 37, 0), Quaternion.identity) as GameObject;
					
					Room_to_Enmeies [11].Add (temp);
					
				} else {
					for (int i = 0; i < Room_to_Enmeies[11].Count; i++) {
						if (Room_to_Enmeies [11] [i] != null)
							Room_to_Enmeies [11] [i].SetActive (true);
					}
				}
				
				this.transform.position = new Vector3 (this.transform.position.x + 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x + 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 64 && this.transform.position.x < 65 && this.transform.position.y >= 38 && this.transform.position.y < 39) {
				print ("28");
				for (int i = 0; i < Room_to_Enmeies[10].Count; i++) {
					if (Room_to_Enmeies [10] [i] != null)
						Room_to_Enmeies [10] [i].SetActive (true);
				}
				for (int i = 0; i < Room_to_Enmeies[11].Count; i++) {
					if (Room_to_Enmeies [11] [i] != null)
						Room_to_Enmeies [11] [i].SetActive (false);
				}
				this.transform.position = new Vector3 (this.transform.position.x - 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x - 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 71 && this.transform.position.x < 72 && this.transform.position.y >= 42 && this.transform.position.y < 43) {
				print ("29");
				
				for (int i = 0; i < Room_to_Enmeies[11].Count; i++) {
					if (Room_to_Enmeies [11] [i] != null)
						Room_to_Enmeies [11] [i].SetActive (false);
				}
				if (Room_to_Enmeies.ContainsKey (12) == false) {
					List<GameObject> enenmy_list = new List<GameObject> ();
					Room_to_Enmeies.Add (12, enenmy_list);
					GameObject temp = Instantiate (enemy_prefab [4], new Vector3 (74, 50, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [12].Add (temp);
					
					
				} else {
					for (int i = 0; i < Room_to_Enmeies[12].Count; i++) {
						if (Room_to_Enmeies [12] [i] != null)
							Room_to_Enmeies [12] [i].SetActive (true);
					}
					
				}
				
				
				this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y + 2.7f, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y + 11, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 71 && this.transform.position.x < 72 && this.transform.position.y >= 44 && this.transform.position.y < 45) {
				print ("30");
				for (int i = 0; i < Room_to_Enmeies[11].Count; i++) {
					if (Room_to_Enmeies [11] [i] != null)
						Room_to_Enmeies [11] [i].SetActive (true);
				}
				for (int i = 0; i < Room_to_Enmeies[12].Count; i++) {
					if (Room_to_Enmeies [12] [i] != null)
						Room_to_Enmeies [12] [i].SetActive (false);
				}
				this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y - 2.7f, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y - 11, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 78 && this.transform.position.x < 79 && this.transform.position.y >= 49 && this.transform.position.y < 50) {
				print ("31");
				
				
				this.transform.position = new Vector3 (this.transform.position.x + 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x + 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 80 && this.transform.position.x < 81 && this.transform.position.y >= 49 && this.transform.position.y < 50) {
				print ("32");
				
				this.transform.position = new Vector3 (this.transform.position.x - 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x - 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 23.5 && this.transform.position.x < 24.1 && this.transform.position.y >= 60 && this.transform.position.y < 60.1) {
				print ("33");
				foreach (GameObject trap in traps) {
					trap.SetActive (false);
				}
				
				
				if (Room_to_Enmeies.ContainsKey (22) == false) {
					List<GameObject> enenmy_list = new List<GameObject> ();
					Room_to_Enmeies.Add (22, enenmy_list);
					GameObject temp = Instantiate (enemy_prefab [1], new Vector3 (379, 8, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [22].Add (temp);
					temp = Instantiate (enemy_prefab [1], new Vector3 (379, 8, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [22].Add (temp);
					temp = Instantiate (enemy_prefab [1], new Vector3 (379, 8, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [22].Add (temp);
					
					
				} else {
					for (int i = 0; i < Room_to_Enmeies[22].Count; i++) {
						if (Room_to_Enmeies [22] [i] != null)
							Room_to_Enmeies [22] [i].SetActive (true);
					}
				}
				
				
				this.transform.position = new Vector3 (376.4f, 9.5f, 0f);
				Camera.main.transform.position = new Vector3 (380.5f, 7f, -10);
			}
			if (this.transform.position.x >= 376 && this.transform.position.x < 377 && this.transform.position.y >= 10 && this.transform.position.y < 11) {
				foreach (GameObject trap in traps) {
					trap.SetActive (true);
				}
				for (int i = 0; i < Room_to_Enmeies[22].Count; i++) {
					if (Room_to_Enmeies [22] [i] != null)
						Room_to_Enmeies [22] [i].SetActive (false);
				}
				this.transform.position = new Vector3 (22.7f, 60f, 0f);
				Camera.main.transform.position = new Vector3 (23.51f, 61.41f, -10);
			}

		} 
		else
		{
			if (this.transform.position.x >= 48.9 && this.transform.position.x < 49 && this.transform.position.y >= 36.98 && this.transform.position.y < 37.1) 
			{
				print ("1");
				if (Room_to_Enmeies.ContainsKey (2) == false) {
					List<GameObject> enenmy_list = new List<GameObject> ();
					Room_to_Enmeies.Add (2, enenmy_list);
					GameObject temp = Instantiate (enemy_prefab [1], new Vector3 (43, 36, 0), Quaternion.identity) as GameObject;
					temp.GetComponent<Enemy>().drop_prob = 0;
					Room_to_Enmeies [2].Add (temp);
					temp = Instantiate (enemy_prefab [1], new Vector3 (35, 35, 0), Quaternion.identity) as GameObject;
					temp.GetComponent<Enemy>().drop_prob = 0;
					Room_to_Enmeies [2].Add (temp);
					temp = Instantiate (enemy_prefab [1], new Vector3 (36, 39, 0), Quaternion.identity) as GameObject;
					temp.GetComponent<Enemy>().drop_prob = 0;
					Room_to_Enmeies [2].Add (temp);
					temp = Instantiate (enemy_prefab [1], new Vector3 (43, 39, 0), Quaternion.identity) as GameObject;
					temp.GetComponent<Enemy>().drop_prob = 0;
					Room_to_Enmeies [2].Add (temp);
					temp = Instantiate (enemy_prefab [1], new Vector3 (36, 39, 0), Quaternion.identity) as GameObject;
					temp.GetComponent<Enemy>().drop_prob = 0;
					Room_to_Enmeies [2].Add (temp);
					
					
				} else {
					for (int i = 0; i < Room_to_Enmeies[2].Count; i++) {
						if (Room_to_Enmeies [2] [i] != null)
							Room_to_Enmeies [2] [i].SetActive (true);
					}
				}
				this.transform.position = new Vector3 (this.transform.position.x - 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x - 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 46.0 && this.transform.position.x < 46.2 && this.transform.position.y >= 36.98 && this.transform.position.y < 37.1) 
			{
				print ("2");
				for (int i = 0; i < Room_to_Enmeies[2].Count; i++) {
					if (Room_to_Enmeies [2] [i] != null)
						Room_to_Enmeies [2] [i].SetActive (false);
				}
				this.transform.position = new Vector3 (this.transform.position.x + 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x + 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 32.9 && this.transform.position.x < 33 && this.transform.position.y >= 36.98 && this.transform.position.y < 37.1) 
			{
				print ("3");
				if (Room_to_Enmeies.ContainsKey (3) == false) {
					List<GameObject> enenmy_list = new List<GameObject> ();
					Room_to_Enmeies.Add (3, enenmy_list);
					GameObject temp = Instantiate (enemy_prefab [3], new Vector3 (20, 35, 0), Quaternion.identity) as GameObject;
					temp.GetComponent<Enemy>().drop_prob = 0;
					Room_to_Enmeies [3].Add (temp);
					temp = Instantiate (enemy_prefab [3], new Vector3 (20, 40, 0), Quaternion.identity) as GameObject;
					temp.GetComponent<Enemy>().drop_prob = 0;
					Room_to_Enmeies [3].Add (temp);
					temp = Instantiate (enemy_prefab [3], new Vector3 (28, 39, 0), Quaternion.identity) as GameObject;
					temp.GetComponent<Enemy>().drop_prob = 0;
					Room_to_Enmeies [3].Add (temp);
					
					
				} else {
					for (int i = 0; i < Room_to_Enmeies[3].Count; i++) {
						if (Room_to_Enmeies [3] [i] != null)
							Room_to_Enmeies [3] [i].SetActive (true);
					}
				}
				for (int i = 0; i < Room_to_Enmeies[2].Count; i++) {
					if (Room_to_Enmeies [2] [i] != null)
						Room_to_Enmeies [2] [i].SetActive (false);
				}
				this.transform.position = new Vector3 (this.transform.position.x - 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x - 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 30 && this.transform.position.x < 30.1 && this.transform.position.y >= 36.98 && this.transform.position.y < 37.1) 
			{
				print ("4");
				for (int i = 0; i < Room_to_Enmeies[2].Count; i++) {
					if (Room_to_Enmeies [2] [i] != null)
						Room_to_Enmeies [2] [i].SetActive (true);
				}
				for (int i = 0; i < Room_to_Enmeies[3].Count; i++) {
					if (Room_to_Enmeies [3] [i] != null)
						Room_to_Enmeies [3] [i].SetActive (false);
				}
				this.transform.position = new Vector3 (this.transform.position.x + 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x + 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 16.9 && this.transform.position.x < 17 && this.transform.position.y >= 36.98 && this.transform.position.y < 37.1) 
			{
				print ("5");
				if (Room_to_Enmeies.ContainsKey (4) == false) {
					List<GameObject> enenmy_list = new List<GameObject> ();
					Room_to_Enmeies.Add (4, enenmy_list);
					GameObject temp = Instantiate (enemy_prefab [0], new Vector3 (3, 34, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [4].Add (temp);
					temp = Instantiate (enemy_prefab [0], new Vector3 (3, 39, 0), Quaternion.identity) as GameObject;
					temp.GetComponent<Enemy>().drop_kind = Drop_Kinds.key;
					Room_to_Enmeies [4].Add (temp);

					
					
				} else {
					for (int i = 0; i < Room_to_Enmeies[4].Count; i++) {
						if (Room_to_Enmeies [4] [i] != null)
							Room_to_Enmeies [4] [i].SetActive (true);
					}
				}
				for (int i = 0; i < Room_to_Enmeies[3].Count; i++) {
					if (Room_to_Enmeies [3] [i] != null)
						Room_to_Enmeies [3] [i].SetActive (false);
				}
				this.transform.position = new Vector3 (this.transform.position.x - 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x - 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 14 && this.transform.position.x < 14.1 && this.transform.position.y >= 36.98 && this.transform.position.y < 37.1) 
			{
				print ("6");
				for (int i = 0; i < Room_to_Enmeies[4].Count; i++) {
					if (Room_to_Enmeies [4] [i] != null)
						Room_to_Enmeies [4] [i].SetActive (false);
				}
				for (int i = 0; i < Room_to_Enmeies[3].Count; i++) {
					if (Room_to_Enmeies [3] [i] != null)
						Room_to_Enmeies [3] [i].SetActive (true);
				}
				this.transform.position = new Vector3 (this.transform.position.x + 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x + 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if(this.transform.position.x >= 7.5 && this.transform.position.x < 7.51 && this.transform.position.y >= 41.5 && this.transform.position.y < 41.55)
			{
				print ("7");
				if (Room_to_Enmeies.ContainsKey (5) == false) {
					List<GameObject> enenmy_list = new List<GameObject> ();
					Room_to_Enmeies.Add (5, enenmy_list);
					GameObject temp = Instantiate (enemy_prefab [2], new Vector3 (2, 51, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [5].Add (temp);
					temp = Instantiate (enemy_prefab [2], new Vector3 (2, 45, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [5].Add (temp);
					temp = Instantiate (enemy_prefab [2], new Vector3 (13, 51, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [5].Add (temp);
					temp = Instantiate (enemy_prefab [2], new Vector3 (13, 45, 0), Quaternion.identity) as GameObject;
					temp.GetComponent<Enemy>().drop_kind = Drop_Kinds.key;
					Room_to_Enmeies [5].Add (temp);
					
					
				} else {
					for (int i = 0; i < Room_to_Enmeies[5].Count; i++) {
						if (Room_to_Enmeies [5] [i] != null)
							Room_to_Enmeies [5] [i].SetActive (true);
					}
				}
				for (int i = 0; i < Room_to_Enmeies[4].Count; i++) {
					if (Room_to_Enmeies [4] [i] != null)
						Room_to_Enmeies [4] [i].SetActive (false);
				}
				this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y + 2.7f, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y + 11, Camera.main.transform.position.z);
			}
			if(this.transform.position.x >= 7.5 && this.transform.position.x < 7.51 && this.transform.position.y >= 43.9 && this.transform.position.y < 44)
			{
				print ("8");
				for (int i = 0; i < Room_to_Enmeies[5].Count; i++) {
					if (Room_to_Enmeies [5] [i] != null)
						Room_to_Enmeies [5] [i].SetActive (false);
				}
				for (int i = 0; i < Room_to_Enmeies[4].Count; i++) {
					if (Room_to_Enmeies [4] [i] != null)
						Room_to_Enmeies [4] [i].SetActive (true);
				}
				this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y - 2.7f, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y - 11, Camera.main.transform.position.z);
			}
			if(this.transform.position.x >= 7.5 && this.transform.position.x < 7.51 && this.transform.position.y >= 52.5 && this.transform.position.y < 52.55)
			{
				print ("9");
				for (int i = 0; i < Room_to_Enmeies[5].Count; i++) {
					if (Room_to_Enmeies [5] [i] != null)
						Room_to_Enmeies [5] [i].SetActive (false);
				}
				this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y + 2.7f, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y + 11, Camera.main.transform.position.z);
			}
			if(this.transform.position.x >= 7.5 && this.transform.position.x < 7.51 && this.transform.position.y >= 54.9 && this.transform.position.y < 55)
			{
				print ("10");
				for (int i = 0; i < Room_to_Enmeies[5].Count; i++) {
					if (Room_to_Enmeies [5] [i] != null)
						Room_to_Enmeies [5] [i].SetActive (true);
				}
				this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y- 2.7f, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y - 11, Camera.main.transform.position.z);
			}
			if(this.transform.position.x >= 3.5 && this.transform.position.x < 3.51 && this.transform.position.y >= 63.505 && this.transform.position.y < 63.51)
			{
				GameObject.Find ("011x063").GetComponent<SpriteRenderer> ().sprite = doors [0];
				GameObject.Find ("012x063").GetComponent<SpriteRenderer> ().sprite = doors [1];
				GameObject.Find ("011x063").GetComponent<BoxCollider>().isTrigger = true;
				GameObject.Find ("012x063").GetComponent<BoxCollider>().isTrigger = true;
				this.transform.position = new Vector3 (11.5f, this.transform.position.y, this.transform.position.z);
				current_direction = Direction.SOUTH;
				link_run_up = link_backkup_run_up;
				link_run_down = link_backkup_run_down;
				link_run_left = link_backkup_run_left;
				link_run_right = link_backkup_run_right;
				sprites = link_backup_sprites;
				spritesfordead = link_backup_spritesfordead;
				GetComponent<SpriteRenderer>().sprite = link_run_up[0];
				selected_weapon_prefab = weapon_Inventory[3];
				rupee_Count += 100;
				speechmachine.instance.sentence_ind = 1;
				Sword_image.SetActive(true);
			}
			if (this.transform.position.x >= 62 && this.transform.position.x < 62.5 && this.transform.position.y >= 36.98 && this.transform.position.y < 37.1) 
			{
				print ("11");
				this.transform.position = new Vector3 (this.transform.position.x + 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x + 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 64.9 && this.transform.position.x < 65 && this.transform.position.y >= 36.98 && this.transform.position.y < 37.1) 
			{
				print ("12");
				this.transform.position = new Vector3 (this.transform.position.x - 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x - 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 78.03 && this.transform.position.x < 78.5 && this.transform.position.y >= 36.98 && this.transform.position.y < 37.1) 
			{
				print ("13");
				if (Room_to_Enmeies.ContainsKey (8) == false) {
					List<GameObject> enenmy_list = new List<GameObject> ();
					Room_to_Enmeies.Add (8, enenmy_list);
					GameObject temp = Instantiate (enemy_prefab [4], new Vector3 (91, 39, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [8].Add (temp);
					temp = Instantiate (enemy_prefab [4], new Vector3 (91, 35, 0), Quaternion.identity) as GameObject;
					Room_to_Enmeies [8].Add (temp);
					
				} else {
					for (int i = 0; i < Room_to_Enmeies[8].Count; i++) {
						if (Room_to_Enmeies [8] [i] != null)
							Room_to_Enmeies [8] [i].SetActive (true);
					}
				}
				this.transform.position = new Vector3 (this.transform.position.x + 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x + 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 80.9 && this.transform.position.x < 81 && this.transform.position.y >= 36.98 && this.transform.position.y < 37.1) 
			{
				print ("14");
				for (int i = 0; i < Room_to_Enmeies[8].Count; i++) {
					if (Room_to_Enmeies [8] [i] != null)
						Room_to_Enmeies [8] [i].SetActive (false);
				}
				this.transform.position = new Vector3 (this.transform.position.x - 3, this.transform.position.y, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x - 16, Camera.main.transform.position.y, Camera.main.transform.position.z);
			}
			if (this.transform.position.x >= 87.5 && this.transform.position.x < 87.55 && this.transform.position.y >= 41.5 && this.transform.position.y < 42) 
			{
				this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y+ 2.7f, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y + 11, Camera.main.transform.position.z);

			}
			if (this.transform.position.x >= 87.5 && this.transform.position.x < 87.55 && this.transform.position.y >= 43.9 && this.transform.position.y < 44) 
			{
				this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y- 2.7f, this.transform.position.z);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y - 11, Camera.main.transform.position.z);
				
			}
			if (boss_kill_counter >= 2) {
				GameObject.Find ("087x041").GetComponent<SpriteRenderer> ().sprite = doors [0];
				GameObject.Find ("088x041").GetComponent<SpriteRenderer> ().sprite = doors [1];
				GameObject.Find ("087x041").GetComponent<BoxCollider>().isTrigger = true;
				GameObject.Find ("088x041").GetComponent<BoxCollider>().isTrigger = true;
				boss_kill_counter = 0;

			}
			
			
		}




	}
	
	void OnTriggerEnter(Collider coll)
	{
		if (coll.tag == "Weapon") 
		{
			if(coll.name == "bow")
			{
				print ("Succesful added");
				inventory.Inventory_bool[1] = true;
				Destroy(coll.gameObject);
			}
		}
		else if (coll.tag == "boom") 
		{
			Destroy(coll.gameObject);
			inventory.Inventory_bool[0] = true;
			boom_Count++;
		}
		else if (coll.tag == "Rupee") {
			rupee_Count++;
			Destroy (coll.gameObject);
		} 
		else if(coll.tag == "bommerage")
		{
			Destroy(coll.gameObject);
			inventory.Inventory_bool[2] = true;
			has_boomerange = true;
		}
		else if (coll.tag == "heart_1") { // add health
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
		} 
		else if (coll.gameObject.tag == "EnemyProjectile") 
		{
			if (coll.gameObject.GetComponent<Boomerange>()) {
				return;
			}
			EnemyProjectile enemyProjObj = coll.gameObject.GetComponent<EnemyProjectile> ();

				if(invince != true)
					health_Count -= enemyProjObj.getDamage ();
				if (health_Count > 0)
					control_state_machine.ChangeState (new LinkStunning (this, sprites, 15, coll.gameObject));
				else
					control_state_machine.ChangeState (new LinkDead (this, spritesfordead, 47));


		}
		else if (coll.gameObject.tag == "triangle") 
		{
			print (coll.gameObject.tag);
			movement_controller.SetSpeed(0);
			movement_controller.SetDirection(Direction.SOUTH);
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = sprites [8];
			this.transform.position = new Vector3(87.5f,48.5f,0);
			control_state_machine.ChangeState(new LinkVictory(this,100,coll.gameObject));

		}
	}

	void OnCollisionEnter(Collision coll) 
	{
		if (coll.gameObject.tag == "trap") 
		{
			Enemy enemyObj = coll.gameObject.GetComponent<Enemy> ();
			if (invince != true)
				health_Count -= 1;
			if (health_Count > 0)
				control_state_machine.ChangeState (new LinkStunning (this, sprites, 15, coll.gameObject));
			else
				control_state_machine.ChangeState (new LinkDead (this, spritesfordead, 47));


		}else if (coll.gameObject.tag == "Enemy") {
			Enemy enemyObj = coll.gameObject.GetComponent<Enemy> ();
			if (invince != true)
				health_Count -= enemyObj.getDamage ();
			if (health_Count > 0)
				control_state_machine.ChangeState (new LinkStunning (this, sprites, 15, coll.gameObject));
			else
				control_state_machine.ChangeState (new LinkDead (this, spritesfordead, 47));
			
		}

		else if (coll.gameObject.tag == "EnemyProjectile") {
			Enemy enemyObj = coll.gameObject.GetComponent<Enemy> ();
			if (coll.gameObject.name == "Boomerange") {
				return;
			}
			if (invince != true)
				health_Count -= enemyObj.getDamage ();
			if (health_Count > 0)
				control_state_machine.ChangeState (new LinkStunning (this, sprites, 15, coll.gameObject));
			else
				control_state_machine.ChangeState (new LinkDead (this, spritesfordead, 47));

		} else if (coll.gameObject.tag == "locked") {
			if (key >= 1) {
				if(Application.loadedLevel == 0)
				{
					if (coll.gameObject.name == "039x009" || coll.gameObject.name == "040x009") {
						
						GameObject.Find ("039x009").GetComponent<SpriteRenderer> ().sprite = doors [0];
						GameObject.Find ("040x009").GetComponent<SpriteRenderer> ().sprite = doors [1];
						GameObject.Find ("039x009").GetComponent<BoxCollider> ().isTrigger = true;
						GameObject.Find ("040x009").GetComponent<BoxCollider> ().isTrigger = true;
						
					}
					if (coll.gameObject.name == "023x031" || coll.gameObject.name == "024x031") {
						GameObject.Find ("023x031").GetComponent<SpriteRenderer> ().sprite = doors [0];
						GameObject.Find ("024x031").GetComponent<SpriteRenderer> ().sprite = doors [1];
						GameObject.Find ("023x031").GetComponent<BoxCollider> ().isTrigger = true;
						GameObject.Find ("024x031").GetComponent<BoxCollider> ().isTrigger = true;
					}
					if (coll.gameObject.name == "039x053" || coll.gameObject.name == "040x053") {
						GameObject.Find ("039x053").GetComponent<SpriteRenderer> ().sprite = doors [0];
						GameObject.Find ("040x053").GetComponent<SpriteRenderer> ().sprite = doors [1];
						GameObject.Find ("039x053").GetComponent<BoxCollider> ().isTrigger = true;
						GameObject.Find ("040x053").GetComponent<BoxCollider> ().isTrigger = true;
					}
					if (coll.gameObject.name == "071x042" || coll.gameObject.name == "072x042") {
						GameObject.Find ("071x042").GetComponent<SpriteRenderer> ().sprite = doors [0];
						GameObject.Find ("072x042").GetComponent<SpriteRenderer> ().sprite = doors [1];
						GameObject.Find ("071x042").GetComponent<BoxCollider> ().isTrigger = true;
						GameObject.Find ("072x042").GetComponent<BoxCollider> ().isTrigger = true;
					}
					
					
					if (coll.gameObject.name == "033x060") {
						GameObject.Find ("033x060").GetComponent<SpriteRenderer> ().sprite = doors [2];
						GameObject.Find ("033x060").GetComponent<BoxCollider> ().isTrigger = true;
						
					}
					if (coll.gameObject.name == "046x038") {
						GameObject.Find ("046x038").GetComponent<SpriteRenderer> ().sprite = doors [3];
						GameObject.Find ("046x038").GetComponent<BoxCollider> ().isTrigger = true;
						
					}
				}
				else
				{
					if (coll.gameObject.name == "062x037") {
						GameObject.Find ("062x037").GetComponent<SpriteRenderer> ().sprite = doors [3];
						GameObject.Find ("062x037").GetComponent<BoxCollider> ().isTrigger = true;
						
					}
					if (coll.gameObject.name == "078x037") {
						GameObject.Find ("078x037").GetComponent<SpriteRenderer> ().sprite = doors [3];
						GameObject.Find ("078x037").GetComponent<BoxCollider> ().isTrigger = true;
						
					}
				}

				key--;
				
			}


		} 

	}
	//Credit to http://answers.unity3d.com/questions/815394/how-to-get-time-of-key-held-down.html
	void OnCollisionStay(Collision coll)
	{
		if (coll.gameObject.tag == "push") 
		{
			if(Input.GetKeyDown(KeyCode.UpArrow) && ready == false)
			{
				presstime = 0;
				downtime = Time.time;
				presstime = downtime + countdown;
				direction = 1;
				ready = true;
			}
			if(Input.GetKeyUp(KeyCode.UpArrow))
			{
				ready = false;
				presstime = 0;
				direction = 0;
			}

			if(Input.GetKeyDown(KeyCode.DownArrow) && ready == false)
			{
				presstime = 0;
				downtime = Time.time;
				presstime = downtime + countdown;
				direction = 2;
				ready = true;
			}

			if(Input.GetKeyUp(KeyCode.DownArrow))
			{
				ready = false;
				presstime = 0;
				direction = 0;
			}


			if(Input.GetKeyDown(KeyCode.LeftArrow) && ready == false)
			{
				presstime = 0;
				downtime = Time.time;
				presstime = downtime + countdown;
				direction = 3;
				ready = true;
			}
			if(Input.GetKeyUp(KeyCode.LeftArrow))
			{
				ready = false;
				presstime = 0;
				direction = 0;
			}

			if(Input.GetKeyDown(KeyCode.RightArrow) && ready == false)
			{
				presstime = 0;
				downtime = Time.time;
				presstime = downtime + countdown;
				direction = 4;
				ready = true;
			}
			if(Input.GetKeyUp(KeyCode.RightArrow))
			{
				ready = false;
				presstime = 0;
				direction = 0;
			}

			if(Time.time >= presstime && ready == true && direction !=0)
			{
				if(coll.gameObject.transform.position.x == 23f && coll.gameObject.transform.position.y == 38f && direction !=0)
				{
					presstime = 0;
					coll.gameObject.GetComponent<SpriteRenderer>().sprite = tile;
					coll.gameObject.GetComponent<BoxCollider>().enabled = false;
					GameObject block_temp = Instantiate(rock,new Vector3(23,38,0),Quaternion.identity) as GameObject;
					trash = block_temp;
					GameObject.Find ("017x038").GetComponent<SpriteRenderer> ().sprite = doors [2];
					GameObject.Find ("017x038").GetComponent<BoxCollider> ().isTrigger = true;
						

				}
				if(coll.gameObject.transform.position.x == 22f && coll.gameObject.transform.position.y == 60f && direction !=0)
				{
					presstime = 0;
					coll.gameObject.GetComponent<SpriteRenderer>().sprite = tile;
					coll.gameObject.GetComponent<BoxCollider>().enabled = false;
					GameObject block_temp = Instantiate(rock,new Vector3(22,60,0),Quaternion.identity) as GameObject;
					trash = block_temp;
			
				}
				ready = false;
			}

		}
	}



	
}

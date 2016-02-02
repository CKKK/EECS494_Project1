using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum Drop_Kinds {other, key, boomerang};
public class Enemy : MonoBehaviour {
	public int health = 1;
	public int damage = 1;
	public StateMachine BehaviorStateMathine;
	public bool invincible = false;
	public List<GameObject> item_drop;
	public GameObject Key_obj;
	public GameObject Boomerang_obj;
	public Drop_Kinds drop_kind = Drop_Kinds.other;
	float drop_prob;

	public Enemy(int health_, int damage_){
		health = health_;
		damage = damage_;
		BehaviorStateMathine = new StateMachine ();
		drop_prob = .2f;


	}

	public int getDamage(){
		return damage;
	}

	public virtual void beAttacked(int damage, GameObject Collider){
		if (!invincible) {
			health -= damage; // need the fix this
			if (health == 0)
				randomDrop ();
				Destroy (this.gameObject);
			}
		}
	}

	public virtual void hittenByBoomerange(GameObject collider) {
		if (!invincible) {
			health -= 1; // need the fix this
			if (health == 0)
				randomDrop ();
			if (health == 0){
				randomDrop();
				Destroy (this.gameObject);
			}
		}
	}

	// Use this for initialization
	protected virtual void Start () {
	
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}

	protected virtual void OnTriggerEnter(Collider coll) {
//		print (coll.gameObject.tag);

	}
	
	public void randomDrop() {
		if (drop_kind == Drop_Kinds.key) {
			GameObject.Instantiate (Key_obj, transform.position, Quaternion.identity);
		} else if (drop_kind == Drop_Kinds.boomerang) {
			GameObject.Instantiate (Boomerang_obj, transform.position, Quaternion.identity);
		} else {
			float rand_num = Random.value;
			if (rand_num < drop_prob) {
				int ind = Random.Range (0, item_drop.Count);
				print (ind);

				GameObject.Instantiate (item_drop [ind], transform.position, Quaternion.identity);
				print (ind);
			}
		}
	}
	public virtual void OnDestroy() {
		
	}
}

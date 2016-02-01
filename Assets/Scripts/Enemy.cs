using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {
	public int health = 1;
	public int damage = 1;
	public StateMachine BehaviorStateMathine;
	public bool invincible = false;
	public List<GameObject> item_drop;
	float drop_prob;

	public Enemy(int health_, int damage_){
		health = health_;
		damage = damage_;
		BehaviorStateMathine = new StateMachine ();
		drop_prob = 1f;

	}

	public int getDamage(){
		return damage;
	}

	public virtual void beAttacked(int damage, GameObject Collider){
		if (!invincible) {
			health -= damage; // need the fix this
			if (health == 0)
				Destroy (this.gameObject);
		}
	}

	public virtual void hittenByBoomerange(GameObject collider) {
		if (!invincible) {
			health -= 1; // need the fix this
			if (health == 0)
				Destroy (this.gameObject);
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


	public virtual void OnDestroy() {
		float rand_num = Random.value;
		if (rand_num < drop_prob) {
			int ind = Random.Range (0, item_drop.Count);

			GameObject.Instantiate(item_drop[ind], transform.position, Quaternion.identity);
			print(ind);
		}
	}
}

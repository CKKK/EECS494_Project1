using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public int health = 1;
	public int damage = 1;
	public StateMachine BehaviorStateMathine;
	public bool invincible = false;
	public Enemy(int health_, int damage_){
		health = health_;
		damage = damage_;
		BehaviorStateMathine = new StateMachine ();
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
}

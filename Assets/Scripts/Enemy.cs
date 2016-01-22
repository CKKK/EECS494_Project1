using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public int health = 1;
	public int damage = 1;

	public Enemy(int health_, int damage_){
		health = health_;
		damage = damage_;
	}

	public int getDamage(){
		return damage;
	}

	// Use this for initialization
	protected virtual void Start () {
	
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}

	protected void OnTriggerEnter(Collider coll) {
		print (coll.gameObject.tag);
		if (coll.tag == "Weapon") {
			health -= 1; // need the fix this
			if(health == 0)
				Destroy(this.gameObject);
		}
	}
}

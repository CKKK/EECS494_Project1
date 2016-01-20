using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	int health = 1;
	int damage = 1;

	public Enemy(int health_, int damage_){
		health = health_;
		damage = damage_;
	}

	public int getDamage(){
		return damage;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.tag == "Weapon") {
			
		}
	}
}

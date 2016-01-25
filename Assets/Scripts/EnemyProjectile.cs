using UnityEngine;
using System.Collections;

public class EnemyProjectile : MonoBehaviour {

	int damage = 1;

	public EnemyProjectile(int damage_){
		damage = damage_;
	}

	public int getDamage(){
		return damage;
	}

	void OnTriggerEnter(Collider coll)
	{
		if(coll.gameObject.tag == "Player")
		{
			Destroy(this.gameObject);
		}
	}
}

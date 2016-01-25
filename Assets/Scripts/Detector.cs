using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Detector : MonoBehaviour {
	int collide_with_tile_counter = 0;
	HashSet<GameObject> colliders = new HashSet<GameObject>();
	public bool CollideWithTile(){
		return collide_with_tile_counter != 0;
	}

	void OnTriggerEnter(Collider coll) {
		if(coll.gameObject.tag == "Tiles") {
			colliders.Add (coll.gameObject);
			collide_with_tile_counter++;
		}
	}

	void OnTriggerExit(Collider coll) {
		if(coll.gameObject.tag == "Tiles") {
			colliders.Remove (coll.gameObject);
			collide_with_tile_counter--;
		}
	}

}

using UnityEngine;
using System.Collections;

public class Boomerange : MonoBehaviour {
	public Vector3 velocity;
	public float maxTime = 3;
	public GameObject throw_obj;
	float beginningTime;
	float actualFarestTime;
	Vector3 farestPoint;

	enum BoomerangeState {going_out, coming_back};
	BoomerangeState state;
	// Use this for initialization
	void Start () {
		state = BoomerangeState.going_out;
		beginningTime = Time.time;
		GetComponent<Rigidbody> ().velocity = velocity;

	}
	
	// Update is called once per frame
	void Update () {
		
		if (state == BoomerangeState.going_out) {
			float currentTime = Time.time;
			if (currentTime - beginningTime >= maxTime) {
				state = BoomerangeState.coming_back;
				farestPoint = this.transform.position;
				actualFarestTime = currentTime;
			}
		} else {
			float u = (Time.time - actualFarestTime)/(actualFarestTime - beginningTime);
			transform.position = (1 - u) * farestPoint + u * throw_obj.transform.position;
			if (u >= 1) {
				Destroy (gameObject);
			}
		}
	}
}

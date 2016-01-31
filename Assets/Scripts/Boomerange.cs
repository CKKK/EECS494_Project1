using UnityEngine;
using System.Collections;

public class Boomerange : MonoBehaviour {
	public Vector3 velocity;
	public float maxTime = 3;
	public GameObject throw_obj;
	float beginningTime;
	float actualFarestTime;
	Vector3 farestPoint;
	int counter = 0;

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
		counter++;
		switch (counter / 5) {
		case 0:
			gameObject.GetComponent<Transform> ().eulerAngles = new Vector3(0, 0, 0);
			break;
		case 1:
			gameObject.GetComponent<Transform> ().eulerAngles = new Vector3(0, 0, 90);
			break;
		case 2:
			gameObject.GetComponent<Transform> ().eulerAngles = new Vector3(0, 0, 180);
			break;
		case 3:
			gameObject.GetComponent<Transform> ().eulerAngles = new Vector3(0, 0, 270);
			break;
		default:
			counter = 0;
			break;
			

		}



		if (state == BoomerangeState.going_out) {
			float currentTime = Time.time;
			if (currentTime - beginningTime >= maxTime) {
				state = BoomerangeState.coming_back;
				farestPoint = this.transform.position;
				actualFarestTime = currentTime;
			} else if (Camera.main.transform.position.y + 3 <= this.transform.position.y) {
				state = BoomerangeState.coming_back;
				farestPoint = this.transform.position;
				actualFarestTime = Time.time;

			} else if(Camera.main.transform.position.y - 6 >= this.transform.position.y) {
				state = BoomerangeState.coming_back;
				farestPoint = this.transform.position;
				actualFarestTime = Time.time;

			} else if(Camera.main.transform.position.x-6 >= this.transform.position.x) {
				state = BoomerangeState.coming_back;
				farestPoint = this.transform.position;
				actualFarestTime = Time.time;

			} else if(Camera.main.transform.position.x +6 <= this.transform.position.x) {
				state = BoomerangeState.coming_back;
				farestPoint = this.transform.position;
				actualFarestTime = Time.time;

			}

		} else {
			float u = (Time.time - actualFarestTime)/(actualFarestTime - beginningTime);
			if (throw_obj == null) {
				Destroy (gameObject);
				return;
			}
			transform.position = (1 - u) * farestPoint + u * throw_obj.transform.position;
			if (u >= 1) {
				Destroy (gameObject);
			}
		}
	}

	void OnTriggerEnter(Collider coll)
	{
		print (coll.gameObject.tag);
		if(coll.gameObject.tag == "Enemy")
		{
			if (state == BoomerangeState.going_out) {
				state = BoomerangeState.coming_back;
				farestPoint = this.transform.position;
				actualFarestTime = Time.time;
			}

			coll.gameObject.GetComponent<Enemy> ().hittenByBoomerange (gameObject);
			PlayerControl.instance.sword_fire = false;
		}
	}

	void OnDestroy() {
		if (throw_obj != null && throw_obj.GetComponent<PlayerControl> ()) {
			throw_obj.GetComponent<PlayerControl> ().has_boomerange = true;
		}
	}


}

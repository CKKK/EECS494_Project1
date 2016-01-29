using UnityEngine;
using System.Collections;

public class rock : MonoBehaviour {

	public float speed = 1.0F;
	private float startTime;
	private float journeyLength;
	public Vector3 endpos;
	public Vector3 startpos;
	// Use this for initialization
	void Start () {
		if(PlayerControl.instance.direction == 1)
			endpos = new Vector3 (this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
		if(PlayerControl.instance.direction == 2)
			endpos = new Vector3 (this.transform.position.x, this.transform.position.y - 1, this.transform.position.z);
		if(PlayerControl.instance.direction == 4)
			endpos = new Vector3 (this.transform.position.x+1, this.transform.position.y, this.transform.position.z);
		if(PlayerControl.instance.direction == 3)
			endpos = new Vector3 (this.transform.position.x-1, this.transform.position.y, this.transform.position.z);
		startpos = this.transform.position;
		startTime = Time.time;
		journeyLength = Vector3.Distance(startpos, endpos);
	}
	
	// Update is called once per frame
	void Update () {
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		transform.position = Vector3.Lerp(startpos, endpos, fracJourney);
		
	}
}

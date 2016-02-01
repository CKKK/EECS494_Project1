using UnityEngine;
using System.Collections;
public enum entityState {NORMAL,ATTACKING,BACK};
public class trap : MonoBehaviour {
	public bool[] Directions = {false,false,false,false};//north,south,west,east
	public Vector3 startpos;
	public entityState current_state;
	public int Direction;
	public float startTime;
	public	float journeyLength;
	public float speed = 2.0f;
	public float speedback = 1.0f;
	public bool temp = false;
	public Vector3[] endpos = {Vector3.zero,Vector3.zero,Vector3.zero,Vector3.zero};
	PlayerControl pc;
	// Use this for initialization
	void Start () {
		pc = PlayerControl.instance;
		current_state = entityState.NORMAL;
		startpos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (current_state == entityState.NORMAL)
		{
			if (Directions[0] == true || Directions[1] == true)
			{
				if (pc.transform.position.x >= this.transform.position.x - 0.5 && pc.transform.position.x <= this.transform.position.x + 0.5) 
				{
					if(pc.transform.position.y > this.transform.position.y)
					{
						Direction = 0;
						startTime = Time.time;
						journeyLength = Vector3.Distance(startpos, endpos[0]);
						current_state = entityState.ATTACKING;
					}
					else
					{
						Direction = 1;
						startTime = Time.time;
						journeyLength = Vector3.Distance(startpos, endpos[1]);
						current_state = entityState.ATTACKING;

					}
				}
			}
			if(Directions[2] == true || Directions[3] == true)
			{
				if(pc.transform.position.y >= this.transform.position.y-0.5 && pc.transform.position.y <= this.transform.position.y+0.5)
				{
					if(pc.transform.position.x > this.transform.position.x)
					{
						Direction = 3;
						startTime = Time.time;
						journeyLength = Vector3.Distance(startpos, endpos[3]);
						current_state = entityState.ATTACKING;
					}
					else
					{
						Direction = 2;
						startTime = Time.time;
						journeyLength = Vector3.Distance(startpos, endpos[2]);
						current_state = entityState.ATTACKING;
					}
				}
			}
		}
		if (current_state == entityState.ATTACKING) 
		{
			if(Direction == 0)
			{

					float distCovered = (Time.time - startTime) * speed;
					float fracJourney = distCovered / journeyLength;
				if(temp == true)
				{
					distCovered = (Time.time - startTime) * speedback;
					fracJourney = distCovered / journeyLength;
				}
				if (temp == false && fracJourney <= 1)
					transform.position = Vector3.Lerp (startpos, endpos[0], fracJourney);
				else if (temp == false && fracJourney > 1) {
					fracJourney = 0;
					startTime = Time.time;
					temp = true;
				} else if (temp == true && fracJourney <= 1) {
					transform.position = Vector3.Lerp (endpos[0], startpos, fracJourney);
				} else if (temp == true && fracJourney > 1) 
				{
					fracJourney = 0;
					startTime = Time.time;
					current_state = entityState.NORMAL;
					temp = false;
				}
			}
			else if (Direction == 1)
			{
				float distCovered = (Time.time - startTime) * speed;
				float fracJourney = distCovered / journeyLength;
				if(temp == true)
				{
					distCovered = (Time.time - startTime) * speedback;
					fracJourney = distCovered / journeyLength;
				}
				if (temp == false && fracJourney <= 1)
					transform.position = Vector3.Lerp (startpos, endpos[1], fracJourney);
				else if (temp == false && fracJourney > 1) {
					fracJourney = 0;
					startTime = Time.time;
					temp = true;
				} else if (temp == true && fracJourney <= 1) {
					transform.position = Vector3.Lerp (endpos[1], startpos, fracJourney);
				} else if (temp == true && fracJourney > 1) 
				{
					fracJourney = 0;
					startTime = Time.time;
					current_state = entityState.NORMAL;
					temp = false;
				}
			}
			else if (Direction == 2)
			{
				float distCovered = (Time.time - startTime) * speed;
				float fracJourney = distCovered / journeyLength;
				if(temp == true)
				{
					distCovered = (Time.time - startTime) * speedback;
					fracJourney = distCovered / journeyLength;
				}
				if (temp == false && fracJourney <= 1)
					transform.position = Vector3.Lerp (startpos, endpos[2], fracJourney);
				else if (temp == false && fracJourney > 1) {
					fracJourney = 0;
					startTime = Time.time;
					temp = true;
				} else if (temp == true && fracJourney <= 1) {
					transform.position = Vector3.Lerp (endpos[2], startpos, fracJourney);
				} else if (temp == true && fracJourney > 1) 
				{
					fracJourney = 0;
					startTime = Time.time;
					current_state = entityState.NORMAL;
					temp = false;
				}
			}
			else if (Direction == 3)
			{
				float distCovered = (Time.time - startTime) * speed;
				float fracJourney = distCovered / journeyLength;
				if(temp == true)
				{
					distCovered = (Time.time - startTime) * speedback;
					fracJourney = distCovered / journeyLength;
				}
				if (temp == false && fracJourney <= 1)
					transform.position = Vector3.Lerp (startpos, endpos[3], fracJourney);
				else if (temp == false && fracJourney > 1) {
					fracJourney = 0;
					startTime = Time.time;
					temp = true;
				} else if (temp == true && fracJourney <= 1) {
					transform.position = Vector3.Lerp (endpos[3], startpos, fracJourney);
				} else if (temp == true && fracJourney > 1) 
				{
					fracJourney = 0;
					startTime = Time.time;
					current_state = entityState.NORMAL;
					temp = false;
				}
			}

		}
	}
}

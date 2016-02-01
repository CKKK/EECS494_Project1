using UnityEngine;
using System.Collections;
public enum entityState {NORMAL,ATTACKING,BACK};
public class trap : MonoBehaviour {
	public bool[] Directions = {false,false,false,false};//north,south,west,east
	public Vector3 startpos;
	public entityState current_state;
	public int Direction;
	public Vector3[] endpos = {Vector3.zero,Vector3.zero,Vector3.zero,Vector3.zero};
	PlayerControl pc;
	// Use this for initialization
	void Start () {
		pc = PlayerControl.instance;
		current_state = entityState.NORMAL;
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
						Direction = 1;
					else
						Direction = 0;
				}
			}
	
		}
	}
}

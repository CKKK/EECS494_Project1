using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Aquamentus : Enemy {
	public GameObject projectile_prefeb;
	public float fire_interval = 100;
	float projectile_speed = 2;
	float fire_cooldown = 0;
	public Sprite [] sprites;
	int counter = 0;
	private Vector3 startpos;
	private Vector3 endpos;
	public float startTime;
	public	float journeyLength;
	public float speed = 1.0F;
	public bool temp = false;
	public Aquamentus() : base(10, 2){
	}

	protected override void Start()
	{
		startpos = transform.position;
		endpos = transform.position + new Vector3 (2, 0, 0);
		startTime = Time.time;
		journeyLength = Vector3.Distance(startpos, endpos);

	}

	// Update is called once per frame
	protected override void Update () {
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		if (temp == false && fracJourney <= 1)
			transform.position = Vector3.Lerp (startpos, endpos, fracJourney);
		else if (temp == false && fracJourney > 1) {
			fracJourney = 0;
			startTime = Time.time;
			temp = true;
		} else if (temp == true && fracJourney <= 1) {
			transform.position = Vector3.Lerp (endpos, startpos, fracJourney);
		} else if (temp == true && fracJourney > 1) 
		{
			fracJourney = 0;
			startTime = Time.time;
			temp = false;
		}


		counter++;
		if (counter < 10) 
		{
			this.gameObject.GetComponent<SpriteRenderer>().sprite = sprites[0];
		} 
		else if (counter >= 10 && counter < 20)
		{
			this.gameObject.GetComponent<SpriteRenderer>().sprite = sprites[1];

		} 
		else
			counter = 0;
		base.Update ();
		float time_delta_fraction = Time.deltaTime / (1.0f / Application.targetFrameRate);
		fire_cooldown += time_delta_fraction;
		if (fire_cooldown > fire_interval) {
			fire_cooldown -= fire_interval;
			if (PlayerControl.instance != null) {
				Vector3 mouse_postion = gameObject.transform.position;
				Vector3 shooting_direction = PlayerControl.instance.gameObject.transform.position - mouse_postion;
				shooting_direction.z = 0;
				shooting_direction.Normalize ();
				shooting_direction *= projectile_speed;
				mouse_postion.z = 0;
				mouse_postion += new Vector3 (-1f, 1f, 0f);
				for (float angle = -15; angle <= 15; angle += 15) {
					GameObject projectile_Instance = MonoBehaviour.Instantiate (projectile_prefeb, mouse_postion, Quaternion.identity) as GameObject;
					projectile_Instance.GetComponent<BossProjectile> ().direction = Quaternion.AngleAxis(angle, Vector3.forward) * shooting_direction;
				}

			}

		}
	}
}


          

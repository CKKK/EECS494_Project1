using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Aquamentus : Enemy {
	public GameObject projectile_prefeb;
	public float fire_interval = 100;
	float projectile_speed = 2;
	float fire_cooldown = 0;
	public Aquamentus() : base(10, 2){
	}
	public override void hittenByBoomerange (GameObject collider)
	{
		
	}
	// Update is called once per frame
	protected override void Update () {
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

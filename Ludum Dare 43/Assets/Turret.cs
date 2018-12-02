﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class Turret : MonoBehaviour {
	public GameObject Main;
	public GameObject BulletOrigin;
	public SoundEffect DeathSound;
	public float Energy = 10f;

	public float BulletSpeed = 1f;
	public float BulletDamage = 1f;
	public float BulletLifetime = 10f;

	public float FireRange = 20f;
	public float FireChargeTime = 1f;
	public float FireRate = 2f;

	private RaycastHit2D[] hits = new RaycastHit2D[1];
	private bool firing;
	private new ExtendedAudioSource audio;

	void Update() {
		if(Energy <= 0f) {
			audio.Play (DeathSound);
			Destroy (Main);
			return;
		}

		if (MainActions.Instance.Player) {
			Vector3 target = MainActions.Instance.Player.transform.position;
			Vector3 dir = target - transform.position;
			float angle = (Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg) - 90f;
			Main.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);

			//Check if recharging
			if (!firing) {
				dir = target - BulletOrigin.transform.position;
				Debug.DrawRay (BulletOrigin.transform.position, dir * FireRange, Color.red);
				int hit = Physics2D.RaycastNonAlloc (BulletOrigin.transform.position, dir, hits, FireRange);
				//Can we see the player
				if(hit > 0 && hits[0].collider != null && hits[0].collider.gameObject == MainActions.Instance.Player.gameObject) {
					StartCoroutine (Fire (BulletOrigin.transform.position, target));
				}
			}
		}
	}

	IEnumerator Fire(Vector2 origin, Vector2 target) {
		firing = true;
		yield return new WaitForSeconds (FireChargeTime);
		BulletPool.Instance.Next (BulletSpeed, BulletDamage, BulletLifetime, origin, target);
		yield return new WaitForSeconds (FireRate);
		firing = false;
	}

	private void Awake() {
		audio = ExtendedAudioSource.Prepare (gameObject);
	}
	private void OnEnable() {
		firing = false;
	}

	void OnCollisionEnter2D(Collision2D collisionD) {
		Energy -= collisionD.gameObject.GetComponent<CollisionDamage> ().energyDamage;
	}

	public void LoseEnergy(float amount) {
		Energy -= amount;
	}
}

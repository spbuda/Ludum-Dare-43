using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class Bullet : MonoBehaviour {
	float speed = 1f;
	float damage = 1f;
	float lifetime = 10f;
	public SoundEffect LaunchSound;

	Rigidbody2D rb;
	private new ExtendedAudioSource audio = null;
	public bool Active { get; private set; }

	private void OnDisable() {
		Active = false;
		rb = null;
	}

	public void Init(float speed, float damage, float lifetime, Vector2 position, Vector2 target) {
		if(audio == null) {
			audio = ExtendedAudioSource.Prepare (gameObject, LaunchSound);
		}
		rb = GetComponent<Rigidbody2D> ();
		Active = true;
		transform.position = position;
		transform.LookAt (target);
		this.speed = speed;
		this.damage = damage;
		this.lifetime = lifetime;
		rb.drag = 0f;
		rb.velocity = (target - position).normalized * speed;
		GetComponent<CollisionDamage> ().energyDamage = damage;
		audio.Play ();
		//rb.AddForce ();
	}

	private void Update() {
		lifetime -= Time.deltaTime;

		if(lifetime <= 0f) {
			Detonate ();
		}
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if(collision.gameObject.tag == "Hittable") {
			collision.gameObject.GetComponent<OnHit> ().onHitScript.Invoke ();
		}
		Detonate ();
	}

	private void Detonate() {
		Active = false;
		this.gameObject.SetActive (false);
	}
}

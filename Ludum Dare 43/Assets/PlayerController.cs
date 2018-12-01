using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, HittableThing {
	public float MaxEnergy = 100f;
	public float Speed = 1f;
	public float Strength = 1f;

	public bool FireForce = false;
	public bool MoveForce = true;

	private float energy;
	private bool dead = false;
	Rigidbody2D rb;
	PlayerHealth healthOrb;

	private void OnEnable() {
		energy = MaxEnergy;
		rb = GetComponent<Rigidbody2D> ();
		healthOrb = GetComponentInChildren<PlayerHealth> ();
	}

	void Update() {
		if (!dead) {
			HandleCamera ();

			CheckLifeState ();
		}
	}

	#region HittableThing implementation
	public float HitAmount => Strength;
	public void HitBy(HittableThing instigator) {
		energy -= instigator.HitAmount;
	}
	#endregion

	void HandleActions(float timestep) {
		if (Input.GetMouseButton (0)) {
			energy -= timestep;

			healthOrb.Resize (MaxEnergy, energy);

			if (FireForce) {
				Vector3 lookPos = PlayerToMouse ();
				rb.AddForce (lookPos.normalized * (-Speed * timestep));
			}
		}
	}

	void HandleCamera() {
		Camera.main.transform.position = new Vector3 (transform.position.x, transform.position.y, -14f);
	}

	private Vector3 PlayerToMouse() {
		Vector3 mousePos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10);
		Vector3 lookPos = Camera.main.ScreenToWorldPoint (mousePos);
		return lookPos - transform.position;
	}

	void HandleRotation() {
		Vector3 lookPos = PlayerToMouse ();
		float angle = (Mathf.Atan2 (lookPos.y, lookPos.x) * Mathf.Rad2Deg) - 90f;
		transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
	}

	void CheckLifeState() {
		if(energy <= 0f) {
			MainActions.Instance.LoseGame ();
			dead = true;
		}
	}

	private void FixedUpdate() {
		if (!dead) {
			HandleControlsForce (Time.fixedDeltaTime);

			HandleRotation ();

			HandleActions (Time.fixedDeltaTime);
		}
	}

	void HandleControls(float timestep) {
		if (MoveForce) {
			HandleControlsForce (Time.fixedDeltaTime);
		} else {
			HandleControlsDirect (Time.fixedDeltaTime);
		}
	}

	void HandleControlsForce(float timestep) {
		Vector2 force = GetForce ();

		if (force != Vector2.zero) {
			force = force.normalized * Speed * timestep;
			rb.AddForce (force);
		}
	}

	void HandleControlsDirect(float timestep) {
		Vector2 force = GetForce ();

		if (force != Vector2.zero) {
			force = force.normalized * Speed * timestep;
			rb.MovePosition(rb.position + force);
		}
	}

	private Vector2 GetForce() {
		Vector2 force = Vector2.zero;
		if (Input.GetKey (KeyCode.W)) {
			force += Vector2.up;
		}
		if (Input.GetKey (KeyCode.S)) {
			force += Vector2.down;
		}
		if (Input.GetKey (KeyCode.A)) {
			force += Vector2.left;
		}
		if (Input.GetKey (KeyCode.D)) {
			force += Vector2.right;
		}

		return force;
	}
}

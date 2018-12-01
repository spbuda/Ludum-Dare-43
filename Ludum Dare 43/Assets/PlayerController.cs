using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float MaxEnergy = 100f;
	public float Speed = 1f;
	public float Strength = 1f;

	public bool FireForce = false;
	public bool MoveForce = true;

	private float energy;
	private bool dead = false;
	Rigidbody2D rb;
	PlayerHealth healthOrb;
	BaseSounds sounds;

	private void OnEnable() {
		energy = MaxEnergy;
		rb = GetComponent<Rigidbody2D> ();
		healthOrb = GetComponentInChildren<PlayerHealth> ();
		sounds = GetComponent<BaseSounds> ();
	}

	void Update() {
		if (!dead) {
			HandleCamera ();

			CheckLifeState ();
		}
	}

	void HandleActions(float timestep) {
		if (Input.GetMouseButton (0)) {
			sounds.OnShoot ();
			energy -= timestep;

			healthOrb.Resize (MaxEnergy, energy);

			if (FireForce) {
				Vector3 lookPos = PlayerToMouse ();
				rb.AddForce (lookPos.normalized * (-Speed * timestep));
			}
		} else {
			sounds.StopShoot ();
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

	private bool warning = false;
	void CheckLifeState() {
		if(!warning && energy <= MaxEnergy * .3f) {
			warning = true;
			sounds.OnWarning ();
		}
		if(energy <= 0f) {
			MainActions.Instance.LoseGame ();
			dead = true;
			sounds.StopShoot ();
			sounds.OnDeath ();
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
		Vector2 force;
		switch (MoveType) {
		case (MoveType.Relative):
			force = HandleControlsRelative (Time.fixedDeltaTime);
			break;
		case (MoveType.Direct):
			force = HandleControlsDirect (Time.fixedDeltaTime);
			break;
		case (MoveType.Force):
			force = HandleControlsForce (Time.fixedDeltaTime);
			break;
		default:
			throw new UnityException ("Not implemented");
		}

		if (!Tools.Calcu.ZeroIsh (force)) {
			energy -= force.magnitude * .001f;
		}
	}

	Vector2 HandleControlsForce(float timestep) {
		Vector2 force = GetForce ();

		if (force != Vector2.zero) {
			force = force.normalized * Speed * timestep;
			rb.AddForce (force);
		}
		return force;
	}

	Vector2 HandleControlsDirect(float timestep) {
		Vector2 force = GetForce ();

		if (force != Vector2.zero) {
			force = force.normalized * Speed * timestep;
			rb.MovePosition(rb.position + force);
		}
		return force;
	}

	Vector2 HandleControlsRelative(float timestep) {
		Vector2 force = GetForce ();

		if (force != Vector2.zero) {
			force = (force.normalized) * Speed * timestep;
			rb.AddRelativeForce (force);
		}
		return force;
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

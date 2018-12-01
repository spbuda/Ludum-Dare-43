using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType { Force, Direct, Relative }
public class PlayerController : MonoBehaviour {
	public float MaxEnergy = 100f;
	public float Speed = 1f;
	public float Strength = 1f;
	public float BeamLength = 10f;

	public bool FireForce = false;
	public MoveType MoveType = MoveType.Relative;

	private float energy;
	private bool dead = false;
	Rigidbody2D rb;
	PlayerHealth healthOrb;
	BaseSounds sounds;
	BeamEffectController beam;

	private void OnEnable() {
		energy = MaxEnergy;
		rb = GetComponent<Rigidbody2D> ();
		healthOrb = GetComponentInChildren<PlayerHealth> ();
		sounds = GetComponent<BaseSounds> ();
		beam = GetComponentInChildren<BeamEffectController> ();
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

			Vector2 lookPos = PlayerToMouse ().normalized;
			if (FireForce) {
				rb.AddForce (lookPos * (-Speed * timestep));
			}
			beam.MakeBeam ();

			//TODO: find beam target.
			Vector2 pos = transform.position;
			beam.UpdateBeam (pos + lookPos * BeamLength);
		} else {
			sounds.StopShoot ();
			beam.StopBeam ();
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
		switch (MoveType) {
		case (MoveType.Relative):
			HandleControlsRelative (Time.fixedDeltaTime);
			break;
		case (MoveType.Direct):
			HandleControlsDirect (Time.fixedDeltaTime);
			break;
		case (MoveType.Force):
			HandleControlsForce (Time.fixedDeltaTime);
			break;
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

	void HandleControlsRelative(float timestep) {
		Vector2 force = GetForce ();

		if (force != Vector2.zero) {
			Vector2 lookPos = PlayerToMouse ();
			force = lookPos * force.normalized * Speed * timestep;
			rb.AddForce (force);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType { Force, Direct, Relative }
public class PlayerController : MonoBehaviour {
	public float MaxEnergy = 100f;
	public float Speed = 1f;
	public float Strength = 1f;
	public float BeamLength = 10f;
	public GameObject BeamOrigin;
	public BeamEffectController BeamPrefab;

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
		if (beam != null) {
			Destroy (beam);
		}
		if (BeamOrigin == null) {
			BeamOrigin = gameObject;
		}
		beam = Instantiate (BeamPrefab, transform);
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

			//TODO: limit length if contact with target.
			Vector2 origin = BeamOrigin.transform.position;
			Vector2 target = lookPos.normalized;
			target = ShootThing (origin, target);
			beam.UpdateBeam (origin, origin + target);
		} else {
			sounds.StopShoot ();
			beam.StopBeam ();
		}
	}

	RaycastHit2D[] hit = new RaycastHit2D[1];
	Vector2 ShootThing(Vector2 origin, Vector2 direction) {
		int hits = Physics2D.RaycastNonAlloc (origin, direction, hit, BeamLength);
		if(hits > 0 && hit[0].collider != null) {
			if(hit[0].collider.tag == "Hittable") {
				hit[0].collider.gameObject.GetComponent<OnHit> ().onHitScript.Invoke ();
			}
			return direction * hit[0].distance;
		} else {
			return direction * BeamLength;
		}
	}

	void HandleCamera() {
		Camera.main.transform.position = new Vector3 (transform.position.x, transform.position.y, -14f);
	}

	private Vector2 PlayerToMouse() {
		Vector2 mousePos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y);
		Vector2 lookPos = Camera.main.ScreenToWorldPoint (mousePos);
		return lookPos - (Vector2) transform.position;
	}

	void HandleRotation() {
		Vector2 lookPos = PlayerToMouse ();
		float angle = (Mathf.Atan2 (lookPos.y, lookPos.x) * Mathf.Rad2Deg) - 90f;
		transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
	}

	private bool warning = false;
	void CheckLifeState() {
		if (!warning && energy <= MaxEnergy * .3f) {
			warning = true;
			sounds.OnWarning ();
		}
		if (energy <= 0f) {
			dead = true;
			sounds.StopShoot ();
			sounds.OnDeath ();
			beam.StopBeam ();

			MainActions.Instance.LoseGame ();
		}
	}

	private void FixedUpdate() {
		if (!dead) {
			HandleControls (Time.fixedDeltaTime);

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
			energy -= Time.fixedDeltaTime;
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
			rb.MovePosition (rb.position + force);
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

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float MaxEnergy = 100f;
	public float Speed = 1f;

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
			HandleActions ();

			HandleCamera ();

			CheckLifeState ();
		}
	}

	void HandleActions() {
		if (Input.GetMouseButton (0)) {
			energy -= Time.deltaTime;

			healthOrb.Resize (MaxEnergy, energy);
		}
	}

	void HandleCamera() {
		Camera.main.transform.position = new Vector3 (transform.position.x, transform.position.y, -14f);
	}

	void HandleRotation() {
		Vector3 mousePos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10);
		Vector3 lookPos = Camera.main.ScreenToWorldPoint (mousePos);
		lookPos = lookPos - transform.position;
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
			HandleControls (Time.fixedDeltaTime);

			HandleRotation ();
		}
	}

	void HandleControls(float timestep) {
		Vector2 force = Vector2.zero;
		bool changed = false;
		if (Input.GetKey (KeyCode.W)) {
			force += Vector2.up;
			changed = true;
		}
		if (Input.GetKey (KeyCode.S)) {
			force += Vector2.down;
			changed = true;
		}
		if (Input.GetKey (KeyCode.A)) {
			force += Vector2.left;
			changed = true;
		}
		if (Input.GetKey (KeyCode.D)) {
			force += Vector2.right;
			changed = true;
		}

		if (changed) {
			force = force.normalized * Speed * timestep;
			rb.AddForce (force);
		}
	}
}

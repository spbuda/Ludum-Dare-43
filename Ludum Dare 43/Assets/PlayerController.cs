using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float MaxEnergy = 100f;
	private float energy;
	private bool dead = false;

	private void OnEnable() {
		energy = MaxEnergy;
	}

	void Update() {
		if (!dead) {
			HandleRotation ();

			HandleControls ();

			HandleCamera ();

			CheckLifeState ();
		}
	}

	void HandleControls() {

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
}

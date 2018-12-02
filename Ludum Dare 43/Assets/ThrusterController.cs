using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterController : MonoBehaviour {
	private Quaternion targetRotation;
	
	ParticleSystem[] Thrust;
	SpriteRenderer Sprite;
	private bool isThrust = true;

	private void Awake() {
		Thrust = GetComponentsInChildren<ParticleSystem> ();
		Sprite = GetComponent<SpriteRenderer> ();
		ToggleThrust (false);
	}

	public void ShowThrusters() {
		ToggleThrust (true);
	}

	public void HideThrusters() {
		ToggleThrust (false);
	}

	public void ToggleThrust(bool doThrust) {
		if (isThrust != doThrust) {
			Sprite.enabled = doThrust;
			foreach (ParticleSystem part in Thrust) {
				if (doThrust) {
					part.Play (true);
				} else {
					part.Stop (true);
				}
			}
			isThrust = doThrust;
		}
	}

	public void ThrustAt(Vector2 direction) {
		targetRotation = Quaternion.FromToRotation (Vector3.down, direction);
	}

	public void UpdateRotation(float timestep) {
		transform.localRotation = Quaternion.RotateTowards (transform.localRotation, targetRotation, 1620f * timestep);
	}
}

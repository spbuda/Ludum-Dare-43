using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterController : MonoBehaviour {
	private Quaternion targetRotation;

	public void ThrustAt(Vector2 direction) {
		targetRotation = Quaternion.FromToRotation (Vector3.down, direction);
	}

	public void UpdateRotation(float timestep) {
		transform.localRotation = Quaternion.RotateTowards (transform.localRotation, targetRotation, 900f * timestep);
	}
}

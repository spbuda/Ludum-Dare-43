using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterController : MonoBehaviour {
	private Quaternion targetRotation;

	public void ThrustAt(Vector2 direction) {
		targetRotation = Quaternion.FromToRotation (Vector3.down, direction);
	}

	public void UpdateRotation(float timestep) {
		transform.rotation = Quaternion.RotateTowards (transform.rotation, targetRotation, 900f * timestep);
	}
}

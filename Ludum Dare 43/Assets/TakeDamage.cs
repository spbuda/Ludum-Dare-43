using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour {
	public void At(Vector3 point) {
		transform.position = point;
		GetComponentInChildren<ParticleSystem> ().Play ();
	}
}

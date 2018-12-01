using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
	private Vector3 scale;
	
	void Start() {
		scale = transform.localScale;
	}

	internal void Resize(float maxEnergy, float energy) {
		float scaleFactor = energy / maxEnergy;
		transform.localScale = new Vector3 (scale.x * scaleFactor, scale.y * scaleFactor, scale.z);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamImpact : MonoBehaviour {
	private ParticleSystem[] Impacts;
	private bool isImpact = true;

	private void Awake() {
		Impacts = GetComponentsInChildren<ParticleSystem> ();
		TriggerImpact (false);
	}

	public void TriggerImpact(bool doImpact) {
		if (isImpact != doImpact) {
			foreach (ParticleSystem part in Impacts) {
				if (doImpact) {
					part.Play (true);
				} else {
					part.Stop (true);
				}
			}
			isImpact = doImpact;
		}
	}

	public void ImpactDistance(float distance) {
		transform.localPosition = new Vector3 (transform.localPosition.x, distance + 1.44f, transform.localPosition.z);
	}
}

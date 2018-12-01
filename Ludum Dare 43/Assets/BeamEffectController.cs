using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeamEffectController : MonoBehaviour {
	public LineRenderer lineRenderer;
	GameObject[] chillins;

	bool beamOn = true;
	public void Awake() {
		chillins = GetComponentsInChildren<Transform> ().Select(t=>t.gameObject).ToArray();
		ToggleChildrenState (false);
	}

	//Vector3 hitPoint;
	public void MakeBeam() {
		ToggleChildrenState (true);
	}

	public void UpdateBeam(Vector2 origin, Vector2 target) {
		if (beamOn) {
			//Line is rendered from a point at index 0 to a point at index 1, in worldspace
			lineRenderer.SetPosition (0, origin); //There's an origin point marker empty gameobject on the player prefab. Use this as the point to fire the beam from
			//hitPoint = mainCamera.ScreenToWorldPoint (Input.mousePosition); //This needs to get the point of collision, or the end of the laser raycast (if it doesn't hit anything) instead of the mouse position - mouse pos is filler
			lineRenderer.SetPosition (1, target);

			//TODO: if the beam is colliding with something, move the beam impact effect to the point of contact. Spark effect is intended to splatter back towards the player.
		}
	}

	public void StopBeam() {
		ToggleChildrenState (false);
	}

	private void ToggleChildrenState(bool state) {
		if(state != beamOn) {
			foreach (GameObject c in chillins) {
				c.SetActive (state);
			}
			beamOn = state;
		}
	}
}

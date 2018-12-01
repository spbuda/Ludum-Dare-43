using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamEffectController : MonoBehaviour
{

	public Camera mainCamera;
	public GameObject originPoint;
	public LineRenderer lineRenderer;

	Vector3 hitPoint;

	void Update()
    {
		//Line is rendered from a point at index 0 to a point at index 1, in worldspace
		lineRenderer.SetPosition (0, originPoint.transform.position); //There's an origin point marker empty gameobject on the player prefab. Use this as the point to fire the beam from
		hitPoint = mainCamera.ScreenToWorldPoint (Input.mousePosition); //This needs to get the point of collision, or the end of the laser raycast (if it doesn't hit anything) instead of the mouse position - mouse pos is filler
		lineRenderer.SetPosition (1, hitPoint);

		//TODO: if the beam is colliding with something, move the beam impact effect to the point of contact. Spark effect is intended to splatter back towards the player.
    }
}

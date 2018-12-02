using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class PlayPickupSound : MonoBehaviour
{
	public SoundEffect pickupSound;

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.tag == "Player") {
			pickupSound.Play ();
		}
	}
}

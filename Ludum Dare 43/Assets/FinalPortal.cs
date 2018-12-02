using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPortal : MonoBehaviour {
	private void OnTriggerEnter2D(Collider2D collision) {
		PlayerController player = collision.GetComponent<PlayerController> ();
		if (player != null) {
			MainActions.Instance.WinGame (player.Energy);
			Destroy (player.gameObject);
		}
	}
}

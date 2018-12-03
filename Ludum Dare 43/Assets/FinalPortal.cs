using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPortal : MonoBehaviour {
	public float ScoreMultiplier = 1f;
	private void OnTriggerEnter2D(Collider2D collision) {
		PlayerController player = collision.GetComponent<PlayerController> ();
		if (player != null && !player.Dead) {
			MainActions.Instance.WinGame (player.MaxEnergy - player.Energy, ScoreMultiplier);
			Destroy (player.gameObject);
		}
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.magenta + Color.cyan;
		Gizmos.DrawWireSphere (transform.position, 1f);
	}
}

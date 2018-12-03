using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPortal : MonoBehaviour {
	public MainActions.SceneName Destination;

	private void OnTriggerEnter2D(Collider2D collision) {
		PlayerController player = collision.GetComponent<PlayerController> ();
		if(player != null && !player.Dead) {
			MainActions.Instance.NextScene (player.MaxEnergy - player.Energy, Destination);
		}
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere (transform.position, 1f);
	}
}

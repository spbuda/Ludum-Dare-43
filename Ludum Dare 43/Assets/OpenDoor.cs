using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour {
	public void onHit() {
		GetComponent<Collider2D> ().enabled = false;
		transform.transform.Find ("ClosedDoor").GetComponent<SpriteRenderer> ().enabled = false;
		transform.transform.Find ("OpenDoor").GetComponent<SpriteRenderer> ().enabled = true;
		gameObject.tag = "Shootover";
	}
}

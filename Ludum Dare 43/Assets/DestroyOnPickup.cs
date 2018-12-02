using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnPickup : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.tag == "Player") {
			this.gameObject.SetActive(false);
		}
	}
}

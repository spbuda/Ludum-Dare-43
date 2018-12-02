﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPortal : MonoBehaviour {
	public MainActions.SceneName Destination;

	private void OnTriggerEnter2D(Collider2D collision) {
		PlayerController player = collision.GetComponent<PlayerController> ();
		if(player != null) {
			MainActions.Instance.NextScene (player.Energy, Destination);
		}
	}
}
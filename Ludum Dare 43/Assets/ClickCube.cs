using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCube : MonoBehaviour {

	RaycastHit hit;
	// Update is called once per frame
	void Update() {
		if (Input.GetMouseButtonDown (0)) {
			var ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit)) {
				MainActions.Instance.LoseGame ();
			}
		}
	}
}

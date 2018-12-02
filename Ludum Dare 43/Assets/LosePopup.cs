using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosePopup : MonoBehaviour {
	public void ChangeScore(float value) {
		GetComponentInChildren<ScoreValue> ().GetComponent<UnityEngine.UI.Text>().text = "" + (Mathf.RoundToInt(value * 100));
	}
	public void KillMe() {
		Destroy (this.gameObject);
	}
}

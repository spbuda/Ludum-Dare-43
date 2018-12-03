using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosePopup : MonoBehaviour {
	public delegate void NextLevel();

	public NextLevel NextLevelCallback;
	public void NextLevelClick() {
		NextLevelCallback ();
	}

	public void ChangeScore(float value, float mult) {
		ScoreValue[] vals = GetComponentsInChildren<ScoreValue> ();
		vals[0].GetComponent<UnityEngine.UI.Text>().text = "" + (Mathf.RoundToInt(value * 100));
		vals[1].GetComponent<UnityEngine.UI.Text> ().text = Mathf.RoundToInt (mult) + "x";
	}
	public void KillMe() {
		Destroy (this.gameObject);
	}
}

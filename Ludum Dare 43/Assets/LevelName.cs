using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelName : MonoBehaviour {
	// Start is called before the first frame update
	void Start() {
		string name = MainActions.Instance.CurrentSceneName ();
		GetComponent<UnityEngine.UI.Text> ().text = name;
	}
}

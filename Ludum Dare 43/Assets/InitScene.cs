using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitScene : MonoBehaviour {
	void Start() {
		MainActions.Instance.PauseBehaviors = false;
	}
}

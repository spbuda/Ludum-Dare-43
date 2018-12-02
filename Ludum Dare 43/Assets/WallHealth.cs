using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHealth : MonoBehaviour
{
	public float Energy = 1f;
	public GameObject Main;

	void Update() {
		if (Energy <= 0f) {
			Destroy (Main);
			return;
		}
	}
	public void LoseEnergy(float amount) {
		Energy -= amount;
	}
}

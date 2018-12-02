using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButton : MonoBehaviour
{
	public void onHit() {
		transform.transform.Find("ButtonOff").GetComponent<SpriteRenderer> ().enabled = false;
		transform.transform.Find("ButtonOn").GetComponent<SpriteRenderer> ().enabled = true;
	}
}

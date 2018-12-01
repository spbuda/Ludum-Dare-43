using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
	public void onHit() {
		transform.position = new Vector3 (transform.position.x, transform.position.y, -10);
	}
}

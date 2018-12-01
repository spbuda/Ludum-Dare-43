using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
	
	public void onHit() {
		GetComponent<Collider2D>().isTrigger = true;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
	public GameObject Main;
	public GameObject BulletOrigin;

	public float BulletSpeed = 1f;
	public float BulletDamage = 1f;
	public float BulletLifetime = 10f;

	public float FireChargeTime = 1f;
	public float FireRate = 2f;

	private RaycastHit2D[] hits = new RaycastHit2D[1];
	private Coroutine fireRoutine = null;

	void Update() {
		Vector3 target = MainActions.Instance.Player.transform.position;
		BulletOrigin.transform.LookAt (target);

		//Check if recharging
		if(fireRoutine != null) {
			int hit = Physics2D.RaycastNonAlloc (BulletOrigin.transform.position, target, hits);
			//Can we see the player
			if(hit > 0 && hits[0].collider != null) {
				fireRoutine = StartCoroutine (Fire (BulletOrigin.transform.position, target));
			}
		}
	}

	IEnumerator Fire(Vector2 origin, Vector2 target) {
		yield return new WaitForSeconds (FireChargeTime);
		BulletPool.Instance.Next (BulletSpeed, BulletDamage, BulletLifetime, origin, target);
		yield return new WaitForSeconds (FireRate);
		fireRoutine = null;
	}
}

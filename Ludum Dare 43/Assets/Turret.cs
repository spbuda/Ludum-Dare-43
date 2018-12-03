using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Tools;

public class Turret : MonoBehaviour {
	public GameObject Main;
	public GameObject BulletOrigin;
	public SoundEffect DeathSound;
	public SoundEffect ChargeSound;
	public float Energy = 10f;

	public float BulletSpeed = 1f;
	public float BulletDamage = 1f;
	public float BulletLifetime = 10f;

	public float FireRange = 20f;
	public float FireChargeTime = 1f;
	public float FireRate = 2f;

	private RaycastHit2D[] hits = new RaycastHit2D[5];
	private bool firing, charging;
	private new ExtendedAudioSource audio;
	private Material mat;
	bool dead = false;
	void Update() {
		if (dead || MainActions.Instance.PauseBehaviors) { return; }
		if(Energy <= 0f) {
			audio.Play (DeathSound);
			Destroy (Main);

			GetComponentsInChildren<Transform> ().Where (t => t.name == "EnemyDeathFX").First ()?
				.GetComponentsInChildren<ParticleSystem>().ToList().ForEach(p=> p.Play ());

			StopAllCoroutines ();
			dead = true;
			return;
		}

		if (MainActions.Instance.Player) {
			Transform target = MainActions.Instance.Player.transform, origin = BulletOrigin.transform;
			//Vector3 dir = target.position - transform.position;

			Vector3 dir = target.position - transform.position;
			float angle = (Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg) - 90f;
			Main.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);

			//Check if recharging
			if (!firing) {
				if (!charging) {
					dir = target.position - origin.position;
					Debug.DrawRay (origin.position, dir * FireRange, Color.red);
					int hit = Physics2D.RaycastNonAlloc (origin.position, dir, hits, FireRange);
					//Can we see the player
					GameObject go;
					for (int i = 0; i < hit; i++) {
						go = hits[i].collider.gameObject;
						if(go.tag == "Shootover") {
							continue;
						} else {
							if (go == MainActions.Instance.Player.gameObject) {
								StartCoroutine (Fire (origin, target));
							}
							break;
						}
					}
				}
			}
		}
	}

	IEnumerator Fire(Transform origin, Transform target) {
		audio.Play (ChargeSound);
		float colorIntensity = 5f, fireDisperse = .3f, fireRecharge = .3f, time = 0f;
		charging = true;
		Vector4 color = mat.GetVector ("_EmissionColor");
		while (time < FireChargeTime) {
			float val = Ease.Fall.From (1f, colorIntensity, time, FireChargeTime);
			mat.SetVector ("_EmissionColor", color * val);
			time += Time.deltaTime;
			yield return null;
		}

		if (MainActions.Instance.PauseBehaviors) {
			mat.SetVector ("_EmissionColor", color);
		} else {
			BulletPool.Instance.Next (BulletSpeed, BulletDamage, BulletLifetime, origin.position, target.position);

			float adjustedFireRate = FireRate - fireDisperse - fireRecharge;
			time = 0f;
			while (time < fireDisperse) {
				float val = Ease.Linear.From (colorIntensity, .3f, time, fireDisperse);
				mat.SetVector ("_EmissionColor", color * val);
				time += Time.deltaTime;
				yield return null;
			}
			time = 0f;
			while (time < fireRecharge) {
				float val = Ease.Linear.From (.4f, 1f, time, fireRecharge);
				mat.SetVector ("_EmissionColor", color * val);
				time += Time.deltaTime;
				yield return null;
			}
			mat.SetVector ("_EmissionColor", color);
			yield return new WaitForSeconds (adjustedFireRate);
		}
		firing = false;
		charging = false;
	}

	private void Awake() {
		audio = ExtendedAudioSource.Prepare (gameObject);
		mat = GetComponentInChildren<TurretRenderer> ().GetComponent<MeshRenderer>().material;
	}
	private void OnEnable() {
		firing = false;
		charging = false;
	}

	void OnCollisionEnter2D(Collision2D collisionD) {
		Energy -= collisionD.gameObject.GetComponent<CollisionDamage> ().energyDamage;
	}

	public void LoseEnergy(float amount) {
		Energy -= amount;
	}
}

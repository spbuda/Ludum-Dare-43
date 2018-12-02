using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class BaseSounds : MonoBehaviour {
	public SoundEffect Shoot;
	public SoundEffect Bump;
	public SoundEffect Hit;
	public SoundEffect Health;
	public SoundEffect Warning;
	public SoundEffect Death;
	public SoundEffect Movement;
	public SoundEffect Spawn;

	private ExtendedAudioSource shoot;
	private ExtendedAudioSource hit;
	private ExtendedAudioSource alert;
	private ExtendedAudioSource move;

	void Start() {
		shoot = ExtendedAudioSource.Prepare (this.gameObject, Shoot);
		hit = ExtendedAudioSource.Prepare (this.gameObject, Hit);
		alert = ExtendedAudioSource.Prepare (this.gameObject);
		move = ExtendedAudioSource.Prepare (this.gameObject, Movement);
	}

	public bool Shooting => shoot.Source.isPlaying;
	public void OnShoot() {
		if (!Shooting) {
			shoot.Play ();
		}
	}

	public void OnSoundType(SoundType type) {
		switch (type) {
		case SoundType.Bullet:
			OnHit ();
			break;
		case SoundType.Wall:
			OnBump ();
			break;
		case SoundType.Buff:
			OnHealth ();
			break;
		case SoundType.None:
		default:
			break;
		}
	}

	public void StopShoot() {
		if (Shooting) {
			shoot.Source.Stop ();
		}
	}

	public void OnBump() {
		alert.Play (Bump);
	}

	public void OnWarning() {
		alert.Play (Warning);
	}

	public void OnDeath() {
		alert.Play (Death);
	}

	public void OnHealth() {
		alert.Play (Health);
	}

	public void OnHit() {
		hit.Play ();
	}

	public void OnSpawn() {
		alert.Play (Spawn);
	}

	public bool isMoving => move.Source.isPlaying;
	public void Moving() {
		if (!isMoving) {
			move.Play ();
		}
	}
	public void StopMoving() {
		if (isMoving) {
			move.Source.Stop ();
		}
	}
}

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
	public SoundEffect Speed;
	public SoundEffect Slow;

	private ExtendedAudioSource shoot;
	private ExtendedAudioSource hit;
	private ExtendedAudioSource alert;
	private ExtendedAudioSource move;
	private ExtendedAudioSource speedPad;
	private ExtendedAudioSource slowPad;

	void Start() {
		shoot = ExtendedAudioSource.Prepare (this.gameObject, Shoot);
		hit = ExtendedAudioSource.Prepare (this.gameObject, Hit);
		alert = ExtendedAudioSource.Prepare (this.gameObject);
		move = ExtendedAudioSource.Prepare (this.gameObject, Movement);
		speedPad = ExtendedAudioSource.Prepare (this.gameObject, Speed);
		slowPad = ExtendedAudioSource.Prepare (this.gameObject, Slow);
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

	public bool speeding => speedPad.Source.isPlaying;
	public void SpeedUp() {
		if (!speeding) {
			speedPad.Play ();
		}
	}

	public bool slowing => slowPad.Source.isPlaying;
	public void SlowDown() {
		if (!slowing) {
			slowPad.Play ();
		}
	}
}

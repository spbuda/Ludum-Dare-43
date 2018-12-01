using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class BaseSounds : MonoBehaviour {
	public SoundEffect Shoot;
	public SoundEffect Bump;
	public SoundEffect Hit;
	public SoundEffect Warning;
	public SoundEffect Death;

	private ExtendedAudioSource shoot;
	private ExtendedAudioSource hit;
	private ExtendedAudioSource alert;

	void Start() {
		shoot = ExtendedAudioSource.Prepare (this.gameObject, Shoot);
		hit = ExtendedAudioSource.Prepare (this.gameObject, Hit);
		alert = ExtendedAudioSource.Prepare (this.gameObject);
	}

	public bool Shooting => shoot.Source.isPlaying;
	public void OnShoot() {
		if (!Shooting) {
			shoot.Play ();
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

	public void OnHit() {
		hit.Play ();
	}
}

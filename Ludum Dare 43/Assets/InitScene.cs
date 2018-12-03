using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitScene : MonoBehaviour {
	public AudioClip Music;
	public float volume = .2f;

	void Start() {
		MusicManager.Instance.PlayTrack (Music, volume);
		MainActions.Instance.PauseBehaviors = false;
	}
}

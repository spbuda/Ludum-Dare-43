using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitScene : MonoBehaviour {
	public Tools.SoundEffect Music;
	private new Tools.ExtendedAudioSource audio;

	void Start() {
		MainActions.Instance.PauseBehaviors = false;
		audio = Tools.ExtendedAudioSource.Prepare (this.gameObject, Music);
		audio.Play (volume: MainActions.Instance.Volume * Music.volume);
	}

	private void OnDisable() {
		audio.Source.Stop ();
	}
}

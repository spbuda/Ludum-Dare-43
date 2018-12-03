using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
	public AudioSource Audio { get; private set; }
	private static MusicManager _inst;
	public static MusicManager Instance {
		get {
			if (_inst == null) {
				GameObject go = new GameObject ("MusicManager");
				_inst = go.AddComponent<MusicManager> ();
				_inst.Audio = go.AddComponent<AudioSource> ();
				_inst.Audio.loop = true;
				DontDestroyOnLoad (go);
			}
			return _inst;
		}
	}

	public void PlayTrack(AudioClip clip, float volume) {
		Audio.volume = volume;
		if(Audio.clip == null || (Audio.clip != null && Audio.clip.name != clip.name)) {
			Audio.clip = clip;
			Audio.Play ();
		}
	}
}

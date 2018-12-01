using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tools {
	[Serializable]
	public class SoundEffect {
		public AudioClip[] sounds = new AudioClip[1];
		[Range (0f, 1f)]
		public float volume = 1f;
		public float pitchVariance = 0f;
		public bool loop = false;
	}

	public class ExtendedAudioSource {
		public AudioSource Source { get; set; }

		protected static System.Random random = new System.Random ();
		protected readonly bool freeClip;
		protected SoundEffect sound;

		protected ExtendedAudioSource() {
			this.freeClip = false;
		}
		protected ExtendedAudioSource(bool freeClip){
			this.freeClip = freeClip;
		}

		protected static ExtendedAudioSource PrepareInstance(GameObject parent, bool free) {
			ExtendedAudioSource source = new ExtendedAudioSource (free);
			source.Source = parent.AddComponent<AudioSource> ();
			return source;
		}

		public static ExtendedAudioSource Prepare(GameObject parent) {
			ExtendedAudioSource source = PrepareInstance (parent, true);
			return source;
		}

		public static ExtendedAudioSource Prepare(GameObject parent, SoundEffect soundEffect) {
			Debug.Assert (soundEffect.sounds.Length >= 1, "No audio clips added to sound effect object being added to " + parent.name);
			ExtendedAudioSource source = PrepareInstance (parent, false);
			source.sound = soundEffect;
			return source;
		}

		/// <summary>
		/// This function is used for free play SoundEffects. Generally this will be for sounds that don't require a reserved instance.
		/// This may be because they have lesser importance or they are never going to happen during other actions.
		/// </summary>
		/// <param name="sound"></param>
		/// <param name="pitchVariance"></param>
		/// <param name="volume"></param>
		/// <param name="overrideCurrent"></param>
		public void Play(SoundEffect sound, float pitchVariance = -1f, float volume = -1f, bool overrideCurrent = true) {
			Debug.Assert (freeClip, "Reserved ExtendedAudioSource on " + Source.name + " attempted to be used for free clip play.");

			if (overrideCurrent || !Source.isPlaying) {
				SetPitch (sound, pitchVariance);
				SetVolume (sound, volume);
				SetClip (sound);

				Source.loop = sound.loop;

				Source.Play ();
			}
		}

		/// <summary>
		/// This is for reserved instances to play Sound Effects. Generally,  this will be used for sounds that need a reserved instance.
		/// This may be because they have high importance or shouldn't override other sounds.
		/// </summary>
		/// <param name="pitchVariance"></param>
		/// <param name="volume"></param>
		/// <param name="overrideCurrent"></param>
		public void Play(float pitchVariance = -1f, float volume = -1f, bool overrideCurrent = true) {
			Debug.Assert (!freeClip, "Free Play ExtendedAudioSource on " + Source.name + " attempted to be used as reserved instance.");

			if (overrideCurrent || !Source.isPlaying) {
				SetPitch (sound, pitchVariance);
				SetVolume (sound, volume);
				SetClip (sound);

				Source.loop = sound.loop;

				Source.Play ();
			}
		}

		protected void SetVolume(SoundEffect sound, float overrideValue = -1f) {
			if (overrideValue < 0f) {
				overrideValue = sound.volume;
			}
			Source.volume = overrideValue;
		}
	
		protected void SetPitch(SoundEffect sound, float overrideValue = -1f) {
			if (overrideValue < 0f) {
				overrideValue = sound.pitchVariance;
			}

			if (Mathf.Approximately (overrideValue, 0f)) {
				Source.pitch = 1f;
			} else {
				Source.pitch = random.Next (
					(int) ((1f - overrideValue) * 100),
					(int) ((1f + overrideValue) * 100)
				) * .01f;
			}
		}

		protected void SetClip(SoundEffect sound) {
			if(sound.sounds.Length > 1) {
				var randomIndex = random.Next (0, sound.sounds.Length);
				Source.clip = sound.sounds[randomIndex];
			} else {
				Source.clip = sound.sounds[0];
			}
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonVolume : MonoBehaviour {
	public Sprite Mute;
	public Sprite Low;
	public Sprite Mid;
	public Sprite Full;

	private void Start() {
		ChangeVolumeImage ();
	}

	public void ChangeVolumeImage() {
		if (MainActions.Instance.Volume < .1f) {
			GetComponent<Button>().image.sprite = Mute;
		} else if (MainActions.Instance.Volume < .5f) {
			GetComponent<Button> ().image.sprite = Low;
		} else if (MainActions.Instance.Volume < .9f) {
			GetComponent<Button> ().image.sprite = Mid;
		} else {
			GetComponent<Button> ().image.sprite = Full;
		}
	}
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu (menuName = "Scriptables/Main Actions")]
public class MainActions : ScriptableObject {
	private static readonly string ASSETPATH = "Assets/Scriptables/MainActions.asset";
	private static MainActions _inst;
	public static MainActions Instance {
		get {
			if (!_inst)
				_inst = Tools.Teyrutils.GetAsset<MainActions> (ASSETPATH);
			return _inst;
		}
	}

	[SerializeField]
	private LosePopup LosePopup;
	
	public float TotalScore = 0f;

	public bool PauseBehaviors = false;
	
	public void StartGame() {
		TotalScore = 0f;
		SceneManager.LoadScene(SceneFromEnum (SceneName.TheHorseShoe), LoadSceneMode.Single);
		PauseBehaviors = false;
	}


	public void NextScene(float score, float multiplier, SceneName scene) {
		BulletPool.Instance.ResetAll ();
		TotalScore += score * multiplier;
		SceneManager.LoadScene (SceneFromEnum(scene), LoadSceneMode.Single);
		BulletPool.Instance.ResetAll ();
		PauseBehaviors = false;
	}

	public void RestartScene() {
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
		BulletPool.Instance.ResetAll ();
		PauseBehaviors = false;
	}

	public void WinLevel(float score, float multiplier, SceneName scene) {
		LosePopup pop = Instantiate (LosePopup, Vector3.zero, Quaternion.identity);
		BulletPool.Instance.ResetAll ();
		//TotalScore += score * multiplier;
		pop.ChangeScore (score, TotalScore, multiplier);

		pop.GetComponentInChildren<RetryButton> ().GetComponent<UnityEngine.UI.Button> ().interactable = true;
		LosePopup.NextLevel callback = delegate () {
			NextScene (score, multiplier, scene);
		};

		pop.GetComponentInChildren<NextButton> ().GetComponent<UnityEngine.UI.Button> ().interactable = true;
		pop.NextLevelCallback += callback;
		PauseBehaviors = true;
	}

	public void WinGame(float score, float multiplier) {
		LosePopup pop = Instantiate (LosePopup, Vector3.zero, Quaternion.identity);
		BulletPool.Instance.ResetAll ();
		pop.ChangeScore (score, TotalScore + (score * multiplier), multiplier);

		pop.GetComponentInChildren<RetryButton> ().GetComponent<UnityEngine.UI.Button> ().interactable = true;
		pop.GetComponentInChildren<NextButton> ().GetComponent<UnityEngine.UI.Button> ().interactable = true;
		PauseBehaviors = true;
	}

	public void LoseGame() {
		LosePopup pop = Instantiate (LosePopup, Vector3.zero, Quaternion.identity);
		BulletPool.Instance.ResetAll ();
		pop.GetComponentInChildren<RetryButton> ().GetComponent<UnityEngine.UI.Button> ().interactable = true;
		pop.GetComponentInChildren<NextButton> ().GetComponent<UnityEngine.UI.Button> ().interactable = false;
		pop.ChangeScore (0f, TotalScore + (score * multiplier), 0f);
		PauseBehaviors = true;
	}

	public void QuitGame() {
		SceneManager.LoadScene ("MenuScene", LoadSceneMode.Single);
	}

	public string SceneFromEnum(SceneName name) {
		return name.ToString ();
	}

	public enum SceneName {
		TheHorseShoe, SpeedPadIntro, ButtonTurretIntro, HealthIntro, SpikeAndDotPadIntro, TheGlove, Strafing, Helix, Edgy, Snek, Walls, LevelFinal
	};

	public PlayerController Player { get; set; } = null;
}

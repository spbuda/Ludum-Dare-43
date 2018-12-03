using System.Collections;
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


	public void NextScene(float score, SceneName scene) {
		BulletPool.Instance.ResetAll ();
		TotalScore += score;
		SceneManager.LoadScene (SceneFromEnum(scene), LoadSceneMode.Single);
		BulletPool.Instance.ResetAll ();
		PauseBehaviors = false;
	}

	public void RestartScene() {
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
		BulletPool.Instance.ResetAll ();
		PauseBehaviors = false;
	}

	public void WinGame(float score) {
		LosePopup pop = Instantiate (LosePopup, Vector3.zero, Quaternion.identity);
		BulletPool.Instance.ResetAll ();

		pop.ChangeScore (TotalScore);
		pop.GetComponentInChildren<RetryButton> ().gameObject.SetActive (false);
		PauseBehaviors = true;
	}

	public void LoseGame() {
		LosePopup pop = Instantiate (LosePopup, Vector3.zero, Quaternion.identity);
		BulletPool.Instance.ResetAll ();
		pop.ChangeScore (TotalScore);
		PauseBehaviors = true;
	}

	public void QuitGame() {
		SceneManager.LoadScene ("MenuScene", LoadSceneMode.Single);
	}

	public string SceneFromEnum(SceneName name) {
		return name.ToString ();
	}

	public enum SceneName {
		TheHorseShoe, SpeedPadIntro, ButtonTurretIntro, HealthIntro, SpikeAndDotPadIntro, TheGlove, LevelFinal
	};

	public PlayerController Player { get; set; } = null;
}

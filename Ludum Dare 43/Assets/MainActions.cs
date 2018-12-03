using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu (menuName = "Scriptables/Main Actions")]
public class MainActions : ScriptableObject {
	private static readonly string ASSETPATH = "MainActions";
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

	public float Volume { get; private set; } = .6f;
	
	public void StartGame() {
		PauseBehaviors = false;
		TotalScore = 0f;
		SceneManager.LoadScene(SceneFromEnum (SceneName.TheHorseShoe), LoadSceneMode.Single);
	}

	public void StepVolume() {
		if(Volume > .9f) {
			Volume = .6f;
		}else if(Volume > .5f) {
			Volume = .3f;
		} else if (Volume > .1f) {
			Volume = 0f;
		} else {
			Volume = 1f;
		}
		AudioListener.volume = Volume;
	}

	public void NextScene(float score, float multiplier, SceneName scene) {
		PauseBehaviors = false;
		BulletPool.Instance.ResetAll ();
		TotalScore += score * multiplier;
		SceneManager.LoadScene (SceneFromEnum(scene), LoadSceneMode.Single);
		BulletPool.Instance.ResetAll ();
	}

	public void RestartScene() {
		PauseBehaviors = false;
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
		BulletPool.Instance.ResetAll ();
	}

	public void WinLevel(float score, float multiplier, SceneName scene) {
		PauseBehaviors = true;
		LosePopup pop = Instantiate (LosePopup, Vector3.zero, Quaternion.identity);
		BulletPool.Instance.ResetAll ();
		//TotalScore += score * multiplier;
		pop.ChangeScore (score, TotalScore + (score * multiplier), multiplier);

		pop.GetComponentInChildren<RetryButton> ().GetComponent<UnityEngine.UI.Button> ().interactable = true;
		LosePopup.NextLevel callback = delegate () {
			NextScene (score, multiplier, scene);
		};

		pop.GetComponentInChildren<NextButton> ().GetComponent<UnityEngine.UI.Button> ().interactable = true;
		pop.NextLevelCallback += callback;
	}

	public void WinGame(float score, float multiplier) {
		PauseBehaviors = true;
		LosePopup pop = Instantiate (LosePopup, Vector3.zero, Quaternion.identity);
		BulletPool.Instance.ResetAll ();
		pop.ChangeScore (score, TotalScore + (score * multiplier), multiplier);

		pop.GetComponentInChildren<RetryButton> ().GetComponent<UnityEngine.UI.Button> ().interactable = true;

		pop.GetComponentInChildren<NextButton> ().GetComponent<UnityEngine.UI.Button> ().gameObject.SetActive(false);
		pop.GetComponentInChildren<YouWinText> (true).gameObject.SetActive (true);
	}

	public void LoseGame() {
		PauseBehaviors = true;
		LosePopup pop = Instantiate (LosePopup, Vector3.zero, Quaternion.identity);
		BulletPool.Instance.ResetAll ();
		pop.GetComponentInChildren<RetryButton> ().GetComponent<UnityEngine.UI.Button> ().interactable = true;
		pop.GetComponentInChildren<NextButton> ().GetComponent<UnityEngine.UI.Button> ().interactable = false;
		pop.ChangeScore (0f, TotalScore, 0f);
	}

	public void QuitGame() {
		SceneManager.LoadScene ("MenuScene", LoadSceneMode.Single);
	}

	public string SceneFromEnum(SceneName name) {
		return name.ToString ();
	}

	public string CurrentSceneName() {
		string name = SceneManager.GetActiveScene ().name;
		foreach (SceneName scene in System.Enum.GetValues (typeof (SceneName))) {
			if(name == scene.ToString ()) {
				return ReadableSceneName (scene);
			}
		}
		//No scene found by that name? Make something up!
		return "Ascendence";
	}

	public enum SceneName {
		TheHorseShoe, Liftoff, InfiltrationI, InfiltrationII, TheSnoon, TheGlove, Strafing, Helix, Edgy, Snek, Walls, ReachForTheSkies, SolarSystem, WeLiveInASociety, LevelFinal
	};

	public string ReadableSceneName (SceneName scene) {
		switch (scene) {
		case SceneName.TheHorseShoe:
			return "The Horse Shoe";
		case SceneName.Liftoff:
			return "Liftoff";
		case SceneName.InfiltrationI:
			return "Infiltration I";
		case SceneName.InfiltrationII:
			return "Infiltration II";
		case SceneName.TheSnoon:
			return "The Snoon";
		case SceneName.TheGlove:
			return "The Glove";
		case SceneName.Strafing:
			return "Strafing";
		case SceneName.Helix:
			return "Helix";
		case SceneName.Edgy:
			return "Edgy";
		case SceneName.Snek:
			return "Snek";
		case SceneName.Walls:
			return "Walls";
		case SceneName.ReachForTheSkies:
			return "Reach For The Skies";
		case SceneName.SolarSystem:
			return "Solar System";
		case SceneName.WeLiveInASociety:
			return "We Live In A Society";
		default:
			throw new UnityException ("No name given for " + scene);
		}
	}

	public PlayerController Player { get; set; } = null;
}

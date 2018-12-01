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

	public GameObject LosePopup;
	
	public void StartGame() {
		SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
	}

	public void LoseGame() {
		Instantiate (LosePopup, Vector3.zero, Quaternion.identity);
	}

	public void QuitGame() {
		SceneManager.LoadScene ("MenuScene", LoadSceneMode.Single);
	}
}

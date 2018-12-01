using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu (menuName = "Scriptables/Main Actions")]
public class MainActions : ScriptableObject {
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

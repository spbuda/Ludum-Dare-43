using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu (menuName = "Scriptables/Main Actions")]
public class MainActions : ScriptableObject {
	public void StartGame() {
		SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
	}
}

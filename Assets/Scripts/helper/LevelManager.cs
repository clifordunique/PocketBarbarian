using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour {

    private static LevelManager _instance;

    public static LevelManager GetInstance() {
        if (!_instance) {
            GameObject go = new GameObject("LevelManager");
            _instance = go.AddComponent<LevelManager>();
        }
        return _instance;
    }


	public void LoadLevel (string scene) {
		Debug.Log("Level load requested for:"+ name);
        SceneManager.LoadScene(scene);
	}
	
	public void LoadNextLevel () {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	
	
	public void Quit () {
		Debug.Log("Quit requested!");
		Application.Quit();
	}
}

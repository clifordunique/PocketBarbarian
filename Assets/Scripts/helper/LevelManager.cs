using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour {

    private static LevelManager _instance;
    

    //Awake is always called before any Start functions
    void Awake() {
        //Check if instance already exists
        if (_instance == null) {
            //if not, set instance to this
            _instance = this;
        }
        //If instance already exists and it's not this:
        else if (_instance != this) {
            Destroy(gameObject);
        }


        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

    }

    public static LevelManager GetInstance() {
        if (!_instance) {
            GameObject go = new GameObject("LevelManager");
            _instance = go.AddComponent<LevelManager>();
        }
        return _instance;
    }
    

    public void LoadLevel(int scene) {
        Debug.Log("Level load index requested for:" + name);
        StartCoroutine(LoadLevelCoroutine(scene));
    }

    public void ReloadActiveLevel() {
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel () {
        LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
	}

    public void LoadMainMenue() {
        LoadLevel(0);
    }

    IEnumerator LoadLevelCoroutine(int sceneIndex) {
        Debug.Log("ReloadLevel");
        FadeCanvasEffect fadeEffect = FadeCanvasEffect.GetInstance();
        fadeEffect.FadeOutSceneCanvas();
        yield return new WaitUntil(() => fadeEffect.fadeComplete);
        SceneManager.LoadScene(sceneIndex);
    }


    public void Quit () {
		Debug.Log("Quit requested!");
		Application.Quit();
	}
}

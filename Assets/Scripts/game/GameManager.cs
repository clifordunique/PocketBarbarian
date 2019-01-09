using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float waitTimeRestartOnDeath = 0F;

    private bool playerDead = false;
    private static GameManager _instance;

    public static GameManager GetInstance() {
        if (_instance) {
            return _instance;
        } else {
            Debug.LogError("GameManager not yet instantiated!");
            return null;
        }
    }

    // called first
    void OnEnable() {
        _instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log("GameManager: OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        FadeCanvasEffect.GetInstance().FadeInSceneCanvas();
    }

 

    public void PlayerDied() {
        if (!playerDead) {
            playerDead = true;
            GuiController.GetInstance().InstanciateDiedEffect();
            StartCoroutine(RestartLevel());
        }
    }

    IEnumerator RestartLevel() {
        yield return new WaitForSeconds(waitTimeRestartOnDeath);
        Debug.Log("Restart Level!");
        FadeCanvasEffect fadeEffect = FadeCanvasEffect.GetInstance();
        fadeEffect.FadeOutSceneCanvas();
        yield return new WaitUntil(() => fadeEffect.fadeComplete);
        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
    }
}

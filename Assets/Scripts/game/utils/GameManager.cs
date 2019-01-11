using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public string testSavepointUuid;
    public float waitTimeRestartOnDeath = 0F;

    private LoadSaveGame loadSaveGame;
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

        // place Player on correct savepoint
        //LoadSavePoint();
        
        FadeCanvasEffect.GetInstance().FadeInSceneCanvas();
    }

    private void Start() {
        Debug.Log("In GM start");
        loadSaveGame = GetComponent<LoadSaveGame>();
        loadSaveGame.Load();
        //StartCoroutine(CheckUniqueIds());
    }
    IEnumerator CheckUniqueIds() {
        yield return 0;
        UniqueId[] uniqueIds = FindObjectsOfType<UniqueId>();
        Debug.Log("Found " +uniqueIds.Length + " unique ids");
        foreach(UniqueId ui in uniqueIds) {
            Debug.Log(ui.uniqueId);
        }
    }


    public void SaveGame() {
        loadSaveGame.Save();
    }

    private void LoadSavePoint() {
        SavePoint[] savePoints = FindObjectsOfType<SavePoint>();
        SavePoint foundSavePoint = null;
        foreach(SavePoint savePoint in savePoints) {
            if (savePoint.uuid == testSavepointUuid) {
                foundSavePoint = savePoint;
            }
        }

        if (foundSavePoint != null) {
            // Player an position setzen
            PlayerController.GetInstance().transform.position = foundSavePoint.GetSpawnPosition();
        }
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

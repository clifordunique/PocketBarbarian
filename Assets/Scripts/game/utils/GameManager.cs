using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public float waitTimeRestartOnDeath = 0F;

    private LoadSaveGame loadSaveGame;
    private LoadSavePlayer loadSavePlayer;
    private bool playerDead = false;

    private InGameMenueManager menueManager;

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
        Cursor.visible = false;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        FadeCanvasEffect.GetInstance().FadeInSceneCanvas();
    }

    private void Start() {     
        loadSaveGame = GetComponent<LoadSaveGame>();
        loadSavePlayer = GetComponent<LoadSavePlayer>();
        menueManager = GetComponent<InGameMenueManager>();

        StartCoroutine(LoadGameCoroutine());
        ItemManager im = new ItemManager();
        //im.SaveShopItems();
        im.LoadShopItems();
    }

    public void Update() {

        // Pause / Ingame Manue handling
        if (Time.timeScale != 0 && (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))) {
            StartCoroutine(ShowMenue());
        }
    }

    IEnumerator ShowMenue() {
        yield return new WaitForEndOfFrame();
        menueManager.ShowMenue();
    }


    IEnumerator LoadGameCoroutine() {
        yield return 0;
        loadSaveGame.Load();
        loadSavePlayer.Load();
    }


    public void SaveGame(SavePoint savePoint) {
        loadSaveGame.Save(savePoint);
        loadSavePlayer.Save();
    }


    public void PlayerDied() {
        if (!playerDead) {
            playerDead = true;
            GuiController.GetInstance().InstanciateDiedEffect();
            StartCoroutine(ReloadLevelOnDeathCoroutine());
        }
    }

    IEnumerator ReloadLevelOnDeathCoroutine() {
        yield return new WaitForSeconds(waitTimeRestartOnDeath);
        StartCoroutine(ReloadLevelCouroutine(SceneManager.GetActiveScene().buildIndex));
    }


    public void ReloadLevel() {
        StartCoroutine(ReloadLevelCouroutine(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadMainMenue() {
        StartCoroutine(ReloadLevelCouroutine(0));
    }

    IEnumerator ReloadLevelCouroutine(int sceneIndex) {
        Debug.Log("ReloadLevel");
        FadeCanvasEffect fadeEffect = FadeCanvasEffect.GetInstance();
        fadeEffect.FadeOutSceneCanvas();
        yield return new WaitUntil(() => fadeEffect.fadeComplete);
        SceneManager.LoadScene(sceneIndex);
    }

    public void ExitGame() {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenueManager : MonoBehaviour {

    public SpriteRenderer lines;
    public SpriteRenderer company;
    public Texture2D mouseTexture;

    private bool itemsReady = false;

    // Use this for initialization
    void Start () {
        Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // called first
    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        FadeCanvasEffect.GetInstance().FadeInSceneCanvas();
        MenueItem[] menueItems = FindObjectsOfType<MenueItem>();
        StartCoroutine(HideMenueItems(menueItems));
        StartCoroutine(ShowItems(menueItems));
        StartCoroutine(CheckIfAllItemsFinishedMovement(menueItems));
    }

    private IEnumerator HideMenueItems(MenueItem[] menueItems) {
        yield return new WaitForEndOfFrame();

        foreach (MenueItem menueItem in menueItems) {
            float width = menueItem.GetWidth();
            SetGUIPosition(menueItem.gameObject, 0.0f, -(width / 2));
        }
    }


    private IEnumerator ShowItems(MenueItem[] menueItems) {

        while(!FadeCanvasEffect.GetInstance().fadeComplete) {
            yield return new WaitForEndOfFrame();
        }        

        foreach (MenueItem menueItem in menueItems) {
            menueItem.ShowItem();
        }
    }

    IEnumerator CheckIfAllItemsFinishedMovement(MenueItem[] menueItems) {
        while (!itemsReady) {
            yield return new WaitForEndOfFrame();
            bool test = true;
            foreach (MenueItem menueItem in menueItems) {
                if (!menueItem.movementComplete) {
                    test = false;
                }
            }
            itemsReady = test;
        }
        Debug.Log("Bereits fertig!");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.SetCursor(mouseTexture, Vector2.zero, CursorMode.ForceSoftware);
    }


    private void SetGUIPosition(GameObject go, float x, float offsetX) {
        float z = go.transform.position.z;
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(x, 0, 0));
        
        go.transform.position = new Vector3(pos.x + offsetX, go.transform.position.y, z);
    }
    
}

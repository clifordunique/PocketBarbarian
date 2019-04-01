using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenManager : AbstractMenueManager {

    public bool skipTitle = false;

    public SpriteRenderer version;
    public SpriteRenderer company;
    public SpriteRenderer lines;
    public SimpleMovement footer;
    public SimpleMovement title;
    public SpriteRenderer pressButtonText;
    public Texture2D mouseTexture;

    public AudioClip bgMusic;

    private bool startSceneComplete = false;

    private bool itemsReady = false;
    private MENUEITEM_TYPE selectedMenueItem = MENUEITEM_TYPE.NAN;


    // Use this for initialization
    void Start() {
        Cursor.visible = false;
        StartCoroutine(PlayMusic());
    }

    IEnumerator PlayMusic() {
        yield return new WaitForSeconds(5F);
        SoundManager.SetBGMVolume(0.5F);
        SoundManager.PlayBGM(bgMusic, false, 0);
    }

    // Update is called once per frame
    void Update() {
        SetGUIPosition(version.gameObject, 1.0f, 0.0f, -1, 0.5f);

        if (!startSceneComplete && Input.anyKey) {
            startSceneComplete = true;
            footer.StartMoving();
            title.StartMoving();
            Destroy(pressButtonText.gameObject);

            StartCoroutine(WaitForStartSceneComplete());
        }

        if (itemsReady && Input.GetKeyDown(KeyCode.DownArrow)) {
            menueItemManager.NextMenueItem();
        }
        if (itemsReady && Input.GetKeyDown(KeyCode.UpArrow)) {
            menueItemManager.PreviousMenueItem();
        }
    }

    // called first
    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        FadeCanvasEffect.GetInstance().FadeInSceneCanvas();
        StartCoroutine(Init());
    }


    IEnumerator Init() {
        yield return new WaitForEndOfFrame();

        // Menue Items initialisieren
        //menueItems = FindObjectsOfType<MenueItem>();
        Debug.Log("Found menue items: " + menueItems.Length);
        foreach (MenueItem menueItem in menueItems) {
            float width = menueItem.GetWidth();
            SetMenueItemsPosition(menueItem.gameObject, 0.0f, -(width / 2));
        }
        menueItemManager = new MenueItemManager(menueItems);

        if (skipTitle) {
            startSceneComplete = true;
            title.transform.position = title.endpos;
            Destroy(footer.gameObject);
            Destroy(pressButtonText.gameObject);
            StartCoroutine(ShowItems());
        } else {
            float height = Camera.main.orthographicSize * 2.0f;
            float width = height * Camera.main.aspect;
            lines.size = new Vector2(width, lines.size.y);
            SetGUIPosition(lines.gameObject, 0.5f, 0.0f, 0, 1.5f);
            SetGUIPosition(company.gameObject, 0.0f, 0.0f, 3, 1.5f);
        }
    }


    private IEnumerator ShowItems() {
        while (!FadeCanvasEffect.GetInstance().fadeComplete) {
            yield return new WaitForEndOfFrame();
        }

        foreach (MenueItem menueItem in menueItems) {
            menueItem.ShowItem();
        }

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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.SetCursor(mouseTexture, Vector2.zero, CursorMode.ForceSoftware);

        menueItemManager.NextMenueItem();       

    }

    IEnumerator WaitForStartSceneComplete() {
        while (!footer.IsInEndposition() || !title.IsInEndposition()) {
            yield return new WaitForEndOfFrame();
        }
        Destroy(footer.gameObject);
        Debug.Log("Jetzt kommt das main menue!");
        StartCoroutine(ShowItems());
    }


    public override void MenueItemSelected(MENUEITEM_TYPE itemSelected) {
        selectedMenueItem = itemSelected;
        switch (itemSelected) {
            case MENUEITEM_TYPE.NEW_GAME:
            case MENUEITEM_TYPE.LOAD_GAME:
            case MENUEITEM_TYPE.OPTIONS:
            case MENUEITEM_TYPE.CREDITS:
                ExecuteSelectedMenueItem();
                break;
            case MENUEITEM_TYPE.EXIT:
                ShowQuestion();
                break;
            default: break;
        }
    }

    public override void ExecuteSelectedMenueItem() {
        switch (selectedMenueItem) {
            case MENUEITEM_TYPE.NEW_GAME:
                LevelManager.GetInstance().LoadNextLevel();
                break;
            case MENUEITEM_TYPE.LOAD_GAME:                
                Debug.Log("LOAD GAME");
                break;
            case MENUEITEM_TYPE.OPTIONS:
                Debug.Log("OPTIONS");
                break;
            case MENUEITEM_TYPE.CREDITS:
                Debug.Log("CREDITS");
                break;
            case MENUEITEM_TYPE.EXIT:                
                Debug.Log("Exit");
                LevelManager.GetInstance().Quit();
                break;
            default: break;
        }
    }


    private void SetGUIPosition(GameObject go, float x, float y, float offsetX, float offsetY) {
        float z = go.transform.position.z;
        go.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0));
        go.transform.position = new Vector3(go.transform.position.x + offsetX, go.transform.position.y + offsetY, z);
    }


    private void SetMenueItemsPosition(GameObject go, float x, float offsetX) {
        float z = go.transform.position.z;
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(x, 0, 0));

        go.transform.position = new Vector3(pos.x + offsetX, go.transform.position.y, z);
    }
}

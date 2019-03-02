using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenueManager : AbstractMenueManager
{
    
    public GameObject menue;
    public Texture2D mouseTexture;

    private MENUEITEM_TYPE selectedMenueItem = MENUEITEM_TYPE.NAN;
    private bool showMenue = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init() {
        yield return new WaitForEndOfFrame();
        menueItemManager = new MenueItemManager(menueItems);
        selectedMenueItem = MENUEITEM_TYPE.NAN;
        questionManager.gameObject.SetActive(false);
    }


    public override void MenueItemSelected(MENUEITEM_TYPE itemSelected) {
        selectedMenueItem = itemSelected;
        switch (itemSelected) {
            case MENUEITEM_TYPE.BACK_TO_GAME:
                ExecuteSelectedMenueItem();
                break;
            case MENUEITEM_TYPE.RELOAD_SAVEPOINT:
            case MENUEITEM_TYPE.MAIN_MENUE:
            case MENUEITEM_TYPE.EXIT:
                ShowQuestion();
                break;
            default: break;
        }
    }
    
    public override void ExecuteSelectedMenueItem() {
        HideMenue();
        switch (selectedMenueItem) {
            case MENUEITEM_TYPE.BACK_TO_GAME:
                HideMenue();
                break;
            case MENUEITEM_TYPE.RELOAD_SAVEPOINT:
                GameManager.GetInstance().ReloadLevel();
                break;
            case MENUEITEM_TYPE.MAIN_MENUE:
                GameManager.GetInstance().LoadMainMenue();
                break;
            case MENUEITEM_TYPE.EXIT:
                GameManager.GetInstance().ExitGame();
                break;
            default: break;
        }
    }
    


    public void ShowMenue() {
        Time.timeScale = 0;
        InputController.GetInstance().moveInputEnabled = false;
        showMenue = true;
        menueItemManager.ReInit();
        menue.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.SetCursor(mouseTexture, Vector2.zero, CursorMode.ForceSoftware);
    }


    public void HideMenue() {
        Time.timeScale = 1;
        InputController.GetInstance().moveInputEnabled = true;
        showMenue = false;
        Cursor.visible = false;
        menue.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (menueInputEnabled && showMenue && Input.GetKeyDown(KeyCode.Escape)) {
            HideMenue();
        }
        if (menueInputEnabled && showMenue && Input.GetKeyDown(KeyCode.DownArrow)) {
            menueItemManager.NextMenueItem();
        }
        if (menueInputEnabled && showMenue && Input.GetKeyDown(KeyCode.UpArrow)) {
            menueItemManager.PreviousMenueItem();
        }
    }
}

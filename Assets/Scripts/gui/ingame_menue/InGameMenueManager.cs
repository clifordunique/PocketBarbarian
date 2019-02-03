using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenueManager : AbstractMenueManager
{

    public QuestionManager questionManager;
    public GameObject menue;
    public Texture2D mouseTexture;
    

    private bool showMenue = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init() {
        yield return new WaitForEndOfFrame();
        menueItemManager = new MenueItemManager(menueItems);
    }

    public void ShowQuestion() {
        questionManager.gameObject.SetActive(true);
        questionManager.Init();
    }


    public void ShowMenue() {
        showMenue = true;
        menueItemManager.ReInit();
        menue.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.SetCursor(mouseTexture, Vector2.zero, CursorMode.ForceSoftware);
    }


    public void HideMenue() {
        showMenue = false;
        Cursor.visible = false;
        menue.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (showMenue && Input.GetKeyDown(KeyCode.DownArrow)) {
            menueItemManager.NextMenueItem();
        }
        if (showMenue && Input.GetKeyDown(KeyCode.UpArrow)) {
            menueItemManager.PreviousMenueItem();
        }
    }
}

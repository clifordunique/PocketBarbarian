using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiController: MonoBehaviour {

    public GuiTextMoveOnScreen fantasyMoveInText;
    public GuiTextMoveOnScreen fantasyPopInText;
    public GuiTextMoveOnScreen fantasyBounceInText;

    public TextAsset randomFightTextAsset;

    public GameObject backgroundLeft;
    public GameObject backgroundCenter;
    public GameObject backgroundRight;
    public GameObject prefabLevelCompleteEffect;
    public GameObject prefabCanvasFlashEffect;

    private static GuiController _instance;
    private float lastScreenWidth = 0;
    private string[] listFightTexts;

    // Use this for initialization
    void Start() {
        _instance = this;
        Invoke("RefreshPositions", 0.5F);
        listFightTexts = randomFightTextAsset.text.Split('\n');
    }

    public static GuiController GetInstance() {
        if (_instance) {
            return _instance;
        } else {
            Debug.LogError("GuiController not yet instantiated!");
            return null;
        }
    }

    // Update is called once per frame
    void LateUpdate() {
        if (Screen.width != lastScreenWidth) {
            RefreshPositions();
            lastScreenWidth = Screen.width;
        }
    }
    

    public void InstanciateLevelCompleteEffect() {
        GameObject effect = Instantiate(prefabLevelCompleteEffect);
        effect.transform.parent = EffectCollection.GetInstance().transform;
    }

    public void RefreshPositions() {
        Utils.SetGUIPosition(backgroundLeft, 0f, 1.0f, 0, 0);
        Utils.SetGUIPosition(backgroundCenter, 0.5f, 1.0f, (5F / Constants.PPU), 0);
        Utils.SetGUIPosition(backgroundRight, 1f, 1.0f, 0, 0);
    }


    public void ShowFantasyText(string text, float removeTime) {
        fantasyMoveInText.Init(text);
        Debug.Log("In Show Fantasy Text");
        StartCoroutine(RemoveFantasyTextEffect(removeTime, fantasyMoveInText));
    }

    public void ShowFantasyBounceInText(string text, float removeTime) {
        fantasyBounceInText.Init(text);
        StartCoroutine(RemoveFantasyTextEffect(removeTime, fantasyBounceInText));
    }


    public void ShowRandomFightText(float time) {
        CanvasFlash();
        string text = listFightTexts[ Random.Range(0, listFightTexts.Length) ];
        fantasyPopInText.Init(text);
        StartCoroutine(RemoveFantasyTextEffect(time, fantasyPopInText));
    }

    public void CanvasFlash() {
        Instantiate(prefabCanvasFlashEffect);
    }

    private IEnumerator RemoveFantasyTextEffect(float waitTime, GuiTextMoveOnScreen textMove) {
        yield return new WaitForSeconds(waitTime);
        textMove.Remove();
    }
}

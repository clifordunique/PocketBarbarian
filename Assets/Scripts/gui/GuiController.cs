﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiController: MonoBehaviour {

    public GuiTextMoveOnScreen fantasyMoveInText;
    public GuiTextMoveOnScreen fantasyPopInText;

    public TextAsset randomFightTextAsset;

    public GameObject backgroundLeft;
    public GameObject backgroundCenter;
    public GameObject backgroundRight;
    public GameObject prefabDiedEffect;
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

        if (Input.GetKeyDown(KeyCode.R)) {
            ShowRandomFightText(0.5F);
        }
    }

    public void InstanciateDiedEffect() {
        GameObject effect = Instantiate(prefabDiedEffect);
        effect.transform.parent = EffectCollection.GetInstance().transform;
    }

    public void InstanciateLevelCompleteEffect() {
        GameObject effect = Instantiate(prefabLevelCompleteEffect);
        effect.transform.parent = EffectCollection.GetInstance().transform;
    }

    public void RefreshPositions() {
        SetGUIPosition(backgroundLeft, 0f, 1.0f, 0, 0);
        SetGUIPosition(backgroundCenter, 0.5f, 1.0f, (5F / Constants.PPU), 0);
        SetGUIPosition(backgroundRight, 1f, 1.0f, 0, 0);
    }

    private void SetGUIPosition(GameObject go, float x, float y, float offsetX, float offsetY) {
        float z = go.transform.position.z;
        go.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0));
        go.transform.position = new Vector3(go.transform.position.x + offsetX, go.transform.position.y + offsetY, z);
    }

    public void ShowFantasyText(string text, float removeTime) {
        fantasyMoveInText.Init(text);
        Debug.Log("In Show Fantasy Text");
        StartCoroutine(RemoveFantasyTextEffect(removeTime, fantasyMoveInText));
    }


    public void ShowRandomFightText(float time) {
        CanvasFlash();
        string text = listFightTexts[ Random.Range(0, listFightTexts.Length) ];
        fantasyPopInText.Init(text);
        StartCoroutine(RemoveFantasyTextEffect(time, fantasyPopInText));
    }

    public void CanvasFlash() {
        GameObject effect1 = Instantiate(prefabCanvasFlashEffect);
    }

    private IEnumerator RemoveFantasyTextEffect(float waitTime, GuiTextMoveOnScreen textMove) {
        yield return new WaitForSeconds(waitTime);
        textMove.Remove();
    }
}

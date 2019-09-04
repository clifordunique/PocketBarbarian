using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeCanvasEffect : MonoBehaviour
{
    private static FadeCanvasEffect _instance;

    private Animator fadeAnimator;
    private Image uiImage;
    [HideInInspector]
    public bool fadeComplete = false;

    public static FadeCanvasEffect GetInstance() {
        if (_instance) {
            return _instance;
        } else {
            Debug.LogError("FadeCanvasEffect not yet instantiated!");
            return null;
        }
    }

    void Awake() {
        Debug.Log("Canvas awakend");
        _instance = this;
        fadeAnimator = GetComponent<Animator>();
        uiImage = GetComponent<Image>();
    }

    public void FadeInSceneCanvas() {
        StartCoroutine(FadeInScene());
    }

    IEnumerator FadeInScene() {
        fadeComplete = false;
        fadeAnimator.SetBool("FADE_IN_SCENE", true);
        yield return new WaitUntil(() => uiImage.color.a == 0);
        fadeComplete = true;
    }


    public void FadeOutSceneCanvas() {
        StartCoroutine(FadeOutScene());
    }

    IEnumerator FadeOutScene() {
        fadeComplete = false;
        fadeAnimator.SetBool("FADE_OUT_SCENE", true);
        yield return new WaitUntil(() => uiImage.color.a == 1);
        fadeComplete = true;
    }

    public void FadeInCanvas() {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn() {
        fadeComplete = false;
        fadeAnimator.SetBool("FADE_OUT", false);
        fadeAnimator.SetBool("FADE_IN", true);
        yield return new WaitUntil(() => uiImage.color.a == 0);
        fadeComplete = true;
    }

    public void FadeOutCanvas() {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut() {
        fadeComplete = false;
        fadeAnimator.SetBool("FADE_OUT", true);
        fadeAnimator.SetBool("FADE_IN", false);
        yield return new WaitUntil(() => uiImage.color.a == 1);
        fadeComplete = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenueItem : MonoBehaviour
{
    private MenueItemSprite sr;
    private SimpleMovementPosition movement;
    private float startTime = -1F;

    [HideInInspector]
    public bool movementComplete = false;

    public void Awake() {
        movement = GetComponent<SimpleMovementPosition>();
        sr = transform.GetComponentInChildren<MenueItemSprite>();
    }
    void OnMouseEnter() {
        if (Cursor.visible) {
            sr.SetEnabled();
            sr.StartShake();
        }
    }

    void OnMouseExit() {
        sr.SetDisabled();
    }

    void OnMouseDown() {
        sr.Click();
        FadeCanvasEffect.GetInstance().FadeOutSceneCanvas();
        Cursor.visible = false;
    }

    public void ShowItem() {
        movement.StartMoving();
        StartCoroutine(CheckIfMovementComplete());
    }

    IEnumerator CheckIfMovementComplete() {
        while (!movement.EndPosReached()) {
            yield return new WaitForEndOfFrame();
        }
        // fertig
        Debug.Log("Movement Complete!");
        movementComplete = true;
    }

    public float GetWidth() {
        return Utils.GetWidthFromSpriteGO(sr.gameObject);
    }
}

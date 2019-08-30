using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiTextMoveOnScreen : MonoBehaviour {

    public float secondsIn;
    public float secondsOut;
    public float startScreenX = 0.5F;
    public float startScreenY = 1.0F;
    public float endScreenX = 0.5F;
    public float endScreenY = 0.5F;
    public float removeScreenX = 0.5F;
    public float removeScreenY = 1F;
    public float startOffsetX = 0F;
    public float startOffsetY = 2F;
    public float endOffsetX = 0F;
    public float endOffsetY = 0F;
    public float removeOffsetX = 0F;
    public float removeOffsetY = 0F;
    public EASE_FUNCTION easeFunctionIn;
    public EASE_FUNCTION easeFunctionOut;

    public enum EASE_FUNCTION {EASEINEXPO, EASEOUTEXPO, EASEOUTBOUNCE };

    private GuiCharacterController characterController;
    private float t = 0;
    private bool isMoving = false;
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 removePos;
    private EASE_FUNCTION easeFunction;
    private bool remove = false;
    private float seconds;

    
    public void Init(string text) {
        // InitPosition
        characterController = GetComponentInChildren<GuiCharacterController>();
        transform.position = GetPosition(startScreenX, startScreenY, startOffsetX, startOffsetY);
        startPos = transform.position;
        endPos = GetPosition(endScreenX, endScreenY, endOffsetX, endOffsetY);
        removePos = GetPosition(removeScreenX, removeScreenY, endOffsetX, endOffsetY);
        easeFunction = easeFunctionIn;
        seconds = secondsIn;
        isMoving = true;
        t = 0;
        characterController.Show(text);
    }

    public void Remove() {
        startPos = endPos;
        endPos = removePos;
        easeFunction = easeFunctionOut;
        seconds = secondsOut;
        isMoving = true;
        t = 0;
        remove = true;
    }

    private Vector3 GetPosition(float x, float y, float offsetX, float offsetY) {
        Vector3 result;
        float z = transform.position.z;
        result = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0));
        result = new Vector3(result.x + offsetX, result.y + offsetY, z);
        return result;
    }

    // Update is called once per frame
    void Update () {
        if (isMoving) {
            SmoothMove();
        }
    }


    private void SmoothMove() {

        if ((t <= 1.0) || (seconds == 0)) {

            t += Time.deltaTime / seconds;
            float newTime = t;
            if (newTime > 1) {
                newTime = 1;
            }
            float easeFactor = 0;
            if (easeFunction == EASE_FUNCTION.EASEOUTBOUNCE) {
                easeFactor = EasingFunction.EaseOutBounce(0.0F, 1.0F, newTime);
            }
            if (easeFunction == EASE_FUNCTION.EASEINEXPO) {
                easeFactor = EasingFunction.EaseInExpo(0.0F, 1.0F, newTime);
            }
            if (easeFunction == EASE_FUNCTION.EASEOUTEXPO) {
                easeFactor = EasingFunction.EaseOutExpo(0.0F, 1.0F, newTime);
            }

            Vector2 newPosition = Vector2.Lerp(startPos, endPos, easeFactor);
            Vector3 pixelPerfectMoveAmount = Utils.MakePixelPerfect(newPosition);
            transform.Translate(pixelPerfectMoveAmount - transform.position);
        } else {
            isMoving = false;
            if (remove) {
                characterController.DestroyCharacters();
            }
        }
    }
}

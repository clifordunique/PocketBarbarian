﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveReactor: MonoBehaviour, ITriggerReactor
{
    
    public float cameraMoveDelaySeconds;
    public float cameraMoveSeconds;
    public float stayOnFocusSeconds;
    public Vector2 focusPoint;
    
    private Vector3 startPos;
    private Vector3 endPos;

    
    public virtual bool TriggerActivated() {
        Debug.Log("Tigger cam triggered");
        InputController.GetInstance().moveInputEnabled = false;
        StartCoroutine(MoveCameraDelay());
        return true;
    }

    public virtual bool TriggerDeactivated() {    
        return true;
    }

    IEnumerator MoveCameraDelay() {
        yield return new WaitForSeconds(cameraMoveDelaySeconds);
        CameraFollow.GetInstance().enabled = false;
        startPos = Camera.main.transform.position;
        endPos = focusPoint;
        CameraFollow.GetInstance().enabled = false;
        StartCoroutine(MoveCamera());
        yield return new WaitForSeconds(cameraMoveSeconds + stayOnFocusSeconds);
        endPos = startPos;
        startPos = focusPoint;
        StartCoroutine(MoveCamera());
        yield return new WaitForSeconds(cameraMoveSeconds);
        InputController.GetInstance().moveInputEnabled = true;
        CameraFollow.GetInstance().enabled = true;
    }

    IEnumerator MoveCamera() {
        float t = 0.0f;
        while (t <= 1.0) {
            t += Time.deltaTime / cameraMoveSeconds;
            float v = t;
            v = EasingFunction.EaseInOutQuad(0.0f, 1.0f, t);
            Vector3 newPosition = Vector3.Lerp(startPos, endPos, v);

            Vector2 pixelPerfectMoveAmount = Utils.MakePixelPerfect(newPosition);
            Vector3 newPos = new Vector3(pixelPerfectMoveAmount.x, pixelPerfectMoveAmount.y, Camera.main.transform.position.z);
            Camera.main.transform.position = newPos;

            yield return new WaitForEndOfFrame();
        }
    }
}

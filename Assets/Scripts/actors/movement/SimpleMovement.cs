﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour {
    

    public float seconds;
    public Vector2 distance;
    public bool biDirectional = false;
    public bool easeInOut = false;
    public bool autoStart = true;

    [HideInInspector]
    public Vector3 endpos;
    private Vector3 startPos;

    [HideInInspector]
    public bool isMoving = false;
    private float t = 0.0f;

    // Use this for initialization
    public void Start () {
        startPos = transform.position;
        endpos = new Vector3(transform.position.x + distance.x, transform.position.y + distance.y, transform.position.z);
        if (autoStart) {
            StartCoroutine(SmoothMove());
        }
    }

    public bool StartMoving() {

        if (!isMoving) {
            StartCoroutine(SmoothMove());
            return true;
        }
        return false;
    }

    public void Update() {
        if (autoStart && biDirectional && !isMoving) {
            StartCoroutine(SmoothMove());
        }
    }

    IEnumerator SmoothMove() {
        isMoving = true;
        t = 0.0f;
        while (t <= 1.0) {
            t += Time.deltaTime / seconds;
            float v = t;
            if (easeInOut) {
                v = EasingFunction.EaseInOutQuad(0.0f, 1.0f, t);
            }
            Vector3 newPosition = Vector3.Lerp(startPos, endpos, v);

            Vector2 pixelPerfectMoveAmount = Utils.MakePixelPerfect(newPosition);
            transform.position = pixelPerfectMoveAmount;

            yield return new WaitForEndOfFrame();
        }
        transform.position = endpos;
        if (biDirectional) {
            endpos = startPos;
            startPos = transform.position;
        }
        isMoving = false;
    }

    public bool IsInEndposition() {
        return transform.position == endpos;
    }
}

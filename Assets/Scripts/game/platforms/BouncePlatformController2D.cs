using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePlatformController2D: AbstractPlatformController2D {

    public enum EASING_FUNCTION {LINEAR, INOUTCUBIC, BOUNCE, ELASTIC, EXPO};
    public EASING_FUNCTION easing;
    public float seconds;
    public Vector2 distance;
    public float waitTime = 0;

    private Vector3 endpos;
    private Vector3 startPos;

    private Vector3 currentEndpos;
    private Vector3 currentStartPos;
    private float currentSeconds;
    private float currentTime;

    private float distancePos;

    private bool isMoving = false;
    private float t;

    public override void Start() {
        base.Start();
        startPos = transform.position;
        endpos = new Vector3(transform.position.x + distance.x, transform.position.y + distance.y, transform.position.z);
        distancePos = Vector3.Distance(startPos, endpos);
    }

    public override Vector3 CalculatePlatformMovement() {
        if (isMoving) {
            if (waitTime > 0 && currentTime + waitTime < Time.time) {
                return SmoothMove();
            }
        } 
        return Vector3.zero;        
    }

    private Vector3 SmoothMove() {

        if (t <= 1.0) {

            t += Time.deltaTime / currentSeconds;
            if (t > 1) {
                t = 1;
            }
            float easeFactor = t;

            //EasingFunction.EaseOutBounce
            //EaseOutElastic
            //EaseInExpo

            if (easing == EASING_FUNCTION.INOUTCUBIC) {
                easeFactor = EasingFunction.EaseInOutCubic(0.0F, 1.0F, t);
            }
            if (easing == EASING_FUNCTION.BOUNCE) {
                easeFactor = EasingFunction.EaseOutBounce(0.0F, 1.0F, t);
            }
            if (easing == EASING_FUNCTION.ELASTIC) {
                easeFactor = EasingFunction.EaseOutElastic(0.0F, 1.0F, t);
            }
            if (easing == EASING_FUNCTION.EXPO) {
                easeFactor = EasingFunction.EaseInExpo(0.0F, 1.0F, t);
            }

            Vector3 newPosition = Vector3.Lerp(currentStartPos, currentEndpos, easeFactor);

            return newPosition - transform.position;
        }
        isMoving = false;
        return Vector3.zero;
    }

    public void OnTriggerEnter2D(Collider2D collider) {
        // react to hit
        if (passengerMask == (passengerMask | (1 << collider.gameObject.layer))) {
            isMoving = true;
            currentTime = Time.time;
            currentStartPos = transform.position;
            currentEndpos = endpos;
            CalculateCurrentSeconds();
            t = 0.0f;
        }
    }

    public void OnTriggerExit2D(Collider2D collider) {
        // react to trigger
        if (passengerMask == (passengerMask | (1 << collider.gameObject.layer))) {
            isMoving = true;
            currentTime = Time.time - waitTime;
            currentStartPos = transform.position;
            currentEndpos = startPos;
            CalculateCurrentSeconds();
            t = 0.0f;
        }
    }

    private void CalculateCurrentSeconds() {
        float currentDistance = Vector3.Distance(currentStartPos, currentEndpos);
        float x = currentDistance / distancePos;
        currentSeconds = seconds * x;
    }
}

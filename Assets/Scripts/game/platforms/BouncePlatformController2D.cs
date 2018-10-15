using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePlatformController2D: AbstractPlatformController2D {

    public enum EASING_FUNCTION {LINEAR, INOUTCUBIC, BOUNCE, ELASTIC, EXPO};
    public EASING_FUNCTION easing;
    public float seconds;
    public Vector2 distance;
    public float waitTime = 0;
    public GameObject prefabDustEffect;

    private Vector3 endpos;
    private Vector3 startPos;

    private Vector3 currentEndpos;
    private Vector3 currentStartPos;
    private float currentSeconds;
    private float currentTime;

    private float distancePos;    

    private bool isMoving = false;
    private float t;

    private bool isInitialShake = false;
    private Vector3 shakePos;
    private bool shakeBack = false;

    public override void Start() {
        base.Start();        
        startPos = transform.position;
        endpos = new Vector3(transform.position.x + distance.x, transform.position.y + distance.y, transform.position.z);
        distancePos = Vector3.Distance(startPos, endpos);
        shakePos = startPos + Vector3.down * (Constants.WorldUnitsPerPixel() * 5);
    }

    public override Vector3 CalculatePlatformMovement() {
        Vector3 result = Vector3.zero;

        if (isMoving) {
            if (isInitialShake) {
                // initial shake on impact
                result = InitialShake();                
            } else {
                if (waitTime > 0 && currentTime + waitTime < Time.time) {
                    result = SmoothMove();
                }
            }
        }
        
        return result;        
    }
    private Vector3 InitialShake() {
        if (t <= 1.0) {

            t += Time.deltaTime / 0.05F; 
            float newTime = t;
            if (newTime > 1) {
                newTime = 1;
            }
            Debug.Log("> NEW TIME:" + newTime) ;
            Vector3 newPosition;
            if (shakeBack) {
                newPosition = Vector3.Lerp(shakePos, startPos, newTime);
            } else {
                newPosition = Vector3.Lerp(startPos, shakePos, newTime);
            }

            Vector3 pixelPerfectMoveAmount = Utils.MakePixelPerfect(newPosition);
            return pixelPerfectMoveAmount - transform.position;
        }
        if (!shakeBack) {
            shakeBack = true;            
        } else {
            shakeBack = false;
            isInitialShake = false;
            InstantiateDustEffects();
        }
        t = 0.0F;
        return Vector3.zero;
    }

    private Vector3 SmoothMove() {

        if (t <= 1.0) {

            t += Time.deltaTime / currentSeconds;
            float newTime = t;
            if (newTime > 1) {
                newTime = 1;
            }
            float easeFactor = newTime;

            //EasingFunction.EaseOutBounce
            //EaseOutElastic
            //EaseInExpo

            if (easing == EASING_FUNCTION.INOUTCUBIC) {
                easeFactor = EasingFunction.EaseInOutCubic(0.0F, 1.0F, newTime);
            }
            if (easing == EASING_FUNCTION.BOUNCE) {
                easeFactor = EasingFunction.EaseOutBounce(0.0F, 1.0F, newTime);
            }
            if (easing == EASING_FUNCTION.ELASTIC) {
                easeFactor = EasingFunction.EaseOutElastic(0.0F, 1.0F, newTime);
            }
            if (easing == EASING_FUNCTION.EXPO) {
                easeFactor = EasingFunction.EaseInExpo(0.0F, 1.0F, newTime);
            }

            Vector3 newPosition = Vector3.Lerp(currentStartPos, currentEndpos, easeFactor);
            Vector3 pixelPerfectMoveAmount = Utils.MakePixelPerfect(newPosition);
            return pixelPerfectMoveAmount - transform.position;
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
            if (!isInitialShake) {
                t = 0.0f;
            }
            if (startPos == transform.position && !isInitialShake) {
                isInitialShake = true;
            } else {
                if (startPos != transform.position && endpos != transform.position) {
                    currentTime = Time.time - waitTime;
                }
            }

            
        }
    }

    public void OnTriggerExit2D(Collider2D collider) {
        // react to trigger
        if (passengerMask == (passengerMask | (1 << collider.gameObject.layer))) {
            isMoving = true;
            currentTime = Time.time;
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


    private void InstantiateDustEffects() {
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).tag == "Effect") {
                GameObject effect = (GameObject)Instantiate(prefabDustEffect);
                effect.transform.parent = EffectCollection.GetInstance().transform;
                effect.transform.position = transform.GetChild(i).position;
            }
        }
    }
}

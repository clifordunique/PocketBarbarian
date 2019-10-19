using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformController2D : AbstractPlatformController2D
{
    public float delay = 0F;
    public enum EASING_FUNCTION { LINEAR, INOUTCUBIC, BOUNCE, ELASTIC, INEXPO, OUTEXPO };
    [Header("Easing Functions")]
    public EASING_FUNCTION easingAction;
    public EASING_FUNCTION easingBase;
    public EASING_FUNCTION easingPrewarm;
    [Header("Speed")]
    public float secondsBase;
    public float secondsAction;
    public float secondsPrewarm;

    [Header("Distances")]
    public Vector2 distance;

    [Header("Wait Times")]
    public float waitTimeBasePosition;
    public float waitTimeActionPosition;
    public float waitTimePrewarmPosition;

    [Header("Effects")]
    public GameObject dustEffect;
    public GameObject fallingEffect;

    private Vector3 endpos;
    private Vector3 startPos;
    private Vector3 prewarmPos;
    private Vector3 moveVector;

    private Vector3 currentEndpos;
    private Vector3 currentStartPos;

    private float t;
    private bool isMoving = false;
    private bool isMovingAction = false;
    private bool isMovingPrewarm = true;

    private bool shakeBack = false;

    private float prewarmTime = 0;
    private float waitUntil = 0;

    private bool actionCompleteEffectPlayed = false;
    
    //private HitBox hitbox;

    public override void Start() {
        base.Start();
        startPos = transform.position;
        endpos = new Vector3(transform.position.x + distance.x, transform.position.y + distance.y, transform.position.z);
        prewarmPos = startPos;

        currentStartPos = startPos;
        currentEndpos = endpos;

        moveVector = endpos - startPos;

        waitUntil = Time.timeSinceLevelLoad + delay;

        /*hitbox = GetComponent<HitBox>();
        if (!hitbox) {
            // search in childs
            hitbox = transform.GetComponentInChildren<HitBox>();
        }
        if (hitbox) {
            hitbox.gameObject.SetActive(false);
        }*/
    }

    public override Vector3 CalculatePlatformMovement() {
        Vector3 result = Vector3.zero;
        //Debug.Log("WaitUntil_" + waitUntil);
        if (isMoving) {
            if (isMovingPrewarm) {
                result = Prewarm();

            } else {
                result = SmoothMove();
            }
            
        } else {
            float time = Time.timeSinceLevelLoad;
            if (waitUntil  <= time) {
                
                isMoving = true;
                t = 0F;
                //waitUntil = 0F;

              /*  if (hitbox) {
                    // enable / disable hitbox
                    if (isMovingAction) {
                        hitbox.gameObject.SetActive(true);
                    } else {
                        hitbox.gameObject.SetActive(false);
                    }
                }*/
            }
        }
        return result;
    }


    private Vector3 Prewarm() {
        if (prewarmTime <= 0) {
            // Prewarm Start
            prewarmTime = waitUntil + secondsPrewarm;
        } else {
            if (prewarmTime <= Time.timeSinceLevelLoad) {
                // Prewarm finished!
                isMoving = false;
                isMovingAction = true;
                isMovingPrewarm = false;                         
                waitUntil = prewarmTime + waitTimePrewarmPosition;
                prewarmTime = 0F;
                // end of Prewarm, endpos immer = startPos!
                Vector3 pixelPerfectMoveAmount = Utils.MakePixelPerfect(startPos);
                return pixelPerfectMoveAmount - transform.position;
            }
        }


        if (t <= 1.0) {

            t += Time.deltaTime / 0.05F;
            float newTime = t;
            if (newTime > 1) {
                newTime = 1;
            }
            Vector3 newPosition;
            if (shakeBack) {
                newPosition = Vector3.Lerp(prewarmPos, startPos, newTime);
            } else {
                newPosition = Vector3.Lerp(startPos, prewarmPos, newTime);
            }

            Vector3 pixelPerfectMoveAmount = Utils.MakePixelPerfect(newPosition);
            return pixelPerfectMoveAmount - transform.position;
        }
        if (!shakeBack) {
            shakeBack = true;
            prewarmPos = new Vector3(startPos.x + Utils.PixelToWorldunits(1), startPos.y, startPos.z);
        } else {
            shakeBack = false;
            prewarmPos = new Vector3(startPos.x - Utils.PixelToWorldunits(1), startPos.y, startPos.z);
        }
        t = 0.0F;
        return Vector3.zero;
    }

    private float GetCurrentSeconds() {
        if (isMovingAction) {
            return secondsAction;
        }
        return secondsBase;
    }

    private EASING_FUNCTION GetCurrentEasing() {
        if (isMovingAction) {
            return easingAction;
        }
        return easingBase;
    }

    private Vector3 SmoothMove() {

        if (t <= 1.0) {

            t += Time.deltaTime / GetCurrentSeconds();
            float newTime = t;
            if (newTime > 1) {
                newTime = 1;
            }
            float easeFactor = newTime;

            EASING_FUNCTION easing = GetCurrentEasing();

            if (easing == EASING_FUNCTION.INOUTCUBIC) {
                easeFactor = EasingFunction.EaseInOutCubic(0.0F, 1.0F, newTime);
            }
            if (easing == EASING_FUNCTION.BOUNCE) {
                easeFactor = EasingFunction.EaseOutBounce(0.0F, 1.0F, newTime);
            }
            if (easing == EASING_FUNCTION.ELASTIC) {
                easeFactor = EasingFunction.EaseOutElastic(0.0F, 1.0F, newTime);
            }
            if (easing == EASING_FUNCTION.INEXPO) {
                easeFactor = EasingFunction.EaseInExpo(0.0F, 1.0F, newTime);
            }
            if (easing == EASING_FUNCTION.OUTEXPO) {
                easeFactor = EasingFunction.EaseOutExpo(0.0F, 1.0F, newTime);
            }

            if (isMovingAction && Time.frameCount % 2 == 0) {
                InstantiateEffect(fallingEffect, transform.position);
            }

            Vector3 newPosition = Vector3.Lerp(currentStartPos, currentEndpos, easeFactor);
            Vector3 pixelPerfectMoveAmount = Utils.MakePixelPerfect(newPosition);

            if (!actionCompleteEffectPlayed && 
                ((easing != EASING_FUNCTION.BOUNCE && newPosition == endpos) || (easing == EASING_FUNCTION.BOUNCE && (newPosition.y > transform.position.y)))) {
                ActionCompleteEffect();
            }

            return pixelPerfectMoveAmount - transform.position;
        }



        isMoving = false;
        isMovingAction = false;
        isMovingPrewarm = false;

        if (currentEndpos == endpos) {
            waitUntil = waitUntil + secondsAction + waitTimeActionPosition;
            currentStartPos = endpos;
            currentEndpos = startPos;
            actionCompleteEffectPlayed = false;

        } else {
            isMovingPrewarm = true;
            waitUntil = waitUntil + secondsBase + waitTimeBasePosition;
            currentStartPos = startPos;
            currentEndpos = endpos;
        }        
        
        return Vector3.zero;
    }

 

    private void ActionCompleteEffect() {
        Vector3 effectPosition1 = BoundUtils.GetMinMaxFromBoundVector(moveVector, myCollider.bounds, true, +0.2F );
        effectPosition1 += Vector3.down * Utils.PixelToWorldunits(4);
        Vector3 effectPosition2 = BoundUtils.GetMinMaxFromBoundVector(moveVector, myCollider.bounds, false, -0.2F );
        effectPosition2 += Vector3.down * Utils.PixelToWorldunits(4);
        InstantiateEffect(dustEffect, effectPosition1, BoundUtils.GetEffectRotation(moveVector, false));
        InstantiateEffect(dustEffect, effectPosition2, BoundUtils.GetEffectRotation(moveVector, false));

        actionCompleteEffectPlayed = true;
    }
    

    public void InstantiateEffect(GameObject effectToInstanciate, Vector2 position, float rotateAngel = 0F, Transform parent = null) {
        GameObject effect = (GameObject)Instantiate(effectToInstanciate);
        if (parent == null) {
            effect.transform.parent = EffectCollection.GetInstance().transform;
        } else {
            effect.transform.parent = parent;
        }
        effect.transform.position = position;
        if (rotateAngel != 0) {
            effect.transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, rotateAngel));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquishPlatformController2D : AbstractPlatformController2D
{
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
    public Vector2 prewarmDistance;

    [Header("Wait Times")]
    public float waitTimeBasePosition;
    public float waitTimeActionPosition;
    public float waitTimePrewarmPosition;

    [Header("Effects")]
    public bool shakeCam = true;
    public GameObject dustEffect;
    public GameObject dustDownEffect;
    public GameObject squishEffect;

    private Vector3 endpos;
    private Vector3 startPos;
    private Vector3 prewarmPos;
    private Vector3 moveVector;

    private Vector3 currentEndpos;
    private Vector3 currentStartPos;

    private float t;
    private bool isMoving = true;
    private bool isMovingAction = false;
    private bool isMovingPrewarm = true;

    private float waitUntil;

    private SpriteRenderer myRenderer;

    public override void Start() {
        base.Start();
        myRenderer = GetComponent<SpriteRenderer>();
        startPos = transform.position;
        endpos = new Vector3(transform.position.x + distance.x, transform.position.y + distance.y, transform.position.z);
        prewarmPos = new Vector3(transform.position.x + prewarmDistance.x, transform.position.y + prewarmDistance.y, transform.position.z);

        currentStartPos = startPos;
        currentEndpos = prewarmPos;

        moveVector = endpos - startPos;
    }

    public override Vector3 CalculatePlatformMovement() {
        Vector3 result = Vector3.zero;

        if (isMoving) {
            result = SmoothMove();
        } else {

            if (waitUntil  < Time.timeSinceLevelLoad) {
                isMoving = true;
                t = 0F;
                waitUntil = 0F;
            }
        }
        return result;
    }

    private float GetCurrentSeconds() {
        if (isMovingPrewarm) {
            return secondsPrewarm;
        }
        if (isMovingAction) {
            return secondsAction;
        }
        return secondsBase;
    }

    private EASING_FUNCTION GetCurrentEasing() {
        if (isMovingPrewarm) {
            return easingPrewarm;
        }
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

            Vector3 newPosition = Vector3.Lerp(currentStartPos, currentEndpos, easeFactor);
            Vector3 pixelPerfectMoveAmount = Utils.MakePixelPerfect(newPosition);


            return pixelPerfectMoveAmount - transform.position;
        }

        if (currentEndpos == endpos && myRenderer.isVisible) {
            ActionCompleteEffect();
        }

        isMoving = false;
        isMovingAction = false;
        isMovingPrewarm = false;

        if (currentEndpos == endpos) {
            waitUntil = Time.timeSinceLevelLoad + waitTimeActionPosition;
            currentStartPos = endpos;
            currentEndpos = startPos;

        } else {
            if (currentEndpos == prewarmPos) {
                PrewarmEffect();
                isMovingAction = true;
                waitUntil = Time.timeSinceLevelLoad + waitTimePrewarmPosition;
                currentStartPos = prewarmPos;
                currentEndpos = endpos;
            } else {
                isMovingPrewarm = true;
                waitUntil = Time.timeSinceLevelLoad + waitTimeBasePosition;
                currentStartPos = startPos;
                currentEndpos = prewarmPos;
            }
        }        
        
        return Vector3.zero;
    }

    private void PrewarmEffect() {
        if (moveVector.y < 0) {
            // runter movement also effect
            Vector3 effectPosition1 = new Vector3(myCollider.bounds.min.x, myCollider.bounds.min.y - prewarmDistance.y, startPos.z);
            Vector3 effectPosition2 = new Vector3(myCollider.bounds.max.x, myCollider.bounds.min.y -prewarmDistance.y, startPos.z);
            InstantiateEffect(dustDownEffect, effectPosition1);
            InstantiateEffect(dustDownEffect, effectPosition2);
        }
    }

    private void ActionCompleteEffect() {
        if (shakeCam) {
            CameraFollow.GetInstance().ShakeSmall();
        }
        Vector3 effectPosition1 = BoundUtils.GetMinMaxFromBoundVector(moveVector, myCollider.bounds, true, +0.2F );
        Vector3 effectPosition2 = BoundUtils.GetMinMaxFromBoundVector(moveVector, myCollider.bounds, false, -0.2F );
        InstantiateEffect(dustEffect, effectPosition1, BoundUtils.GetEffectRotation(moveVector, false));
        InstantiateEffect(dustEffect, effectPosition2, BoundUtils.GetEffectRotation(moveVector, false));

        if (moveVector.y > 0 || moveVector.x != 0) {
            Vector3 dustEffectPosition1 = BoundUtils.GetMinMaxFromBoundVector(moveVector, myCollider.bounds, false, +0.2F);
            InstantiateEffect(dustDownEffect, dustEffectPosition1);
            if (moveVector.y > 0) {
                Vector3 dustEffectPosition2 = BoundUtils.GetMinMaxFromBoundVector(moveVector, myCollider.bounds, true, +0.2F);                
                InstantiateEffect(dustDownEffect, dustEffectPosition2);
            }
        }
    }


    public override void ReactToPassenger(Vector3 velocity, PassengerMovement passenger) {
        bool squisch = false;
        Vector3 hitSourcePosition = transform.position;

        if ((!passenger.standingOnPlatform && velocity.x < 0 && passengerDictionary[passenger.transform].IsLeft()) ||
            (!passenger.standingOnPlatform && velocity.x > 0 && passengerDictionary[passenger.transform].IsRight())) {
            // sideways squisch!
            squisch = true;
            hitSourcePosition = new Vector3(transform.position.x, passenger.transform.position.y, transform.position.z);
        }

        if ((passenger.standingOnPlatform && velocity.y > 0 && passengerDictionary[passenger.transform].IsBelow() && passengerDictionary[passenger.transform].IsAbove()) ||
            (!passenger.standingOnPlatform && passengerDictionary[passenger.transform].IsBelow() && velocity.y < 0)) {
            // vertical squisch!
            squisch = true;
            hitSourcePosition = new Vector3(passenger.transform.position.x, transform.position.y, transform.position.z);
        }

        if (squisch) {
            HurtBox hurtBox = passenger.transform.GetComponent<HurtBox>();
            if (!hurtBox) {
                hurtBox = passenger.transform.GetComponentInChildren<HurtBox>();
            }

            if (hurtBox) {
                hurtBox.ReceiveHit(true, 100, HitBox.DAMAGE_TYPE.SQUISH, hitSourcePosition);

                // create squish effect on platform
                Vector3 positionEffectPlatform = BoundUtils.GetPositionOnBounds(velocity, passenger.transform.position, myCollider.bounds, 26);
                InstantiateEffect(squishEffect, positionEffectPlatform, BoundUtils.GetEffectRotation(velocity, true), transform);

                // create squish effect on ground
                BoxCollider2D collider2D = passenger.transform.GetComponent<BoxCollider2D>();
                if (collider2D) {
                    Vector3 positionEffectGround = BoundUtils.GetPositionOnBounds(velocity, positionEffectPlatform, collider2D.bounds);
                    InstantiateEffect(squishEffect, positionEffectGround, BoundUtils.GetEffectRotation(velocity, false));
                }

            }
        }
    }


    public void InstantiateEffect(GameObject effectToInstanciate, Vector2 position, float rotateAngel = 0F, Transform parent = null) {
        if (effectToInstanciate != null) {
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
}

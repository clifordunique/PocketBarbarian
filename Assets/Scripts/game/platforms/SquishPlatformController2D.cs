using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquishPlatformController2D : AbstractPlatformController2D
{
    public enum EASING_FUNCTION { LINEAR, INOUTCUBIC, BOUNCE, ELASTIC, EXPO };
    public EASING_FUNCTION easingAction;
    public EASING_FUNCTION easingBase;
    public float secondsBase;
    public float secondsAction;
    public Vector2 distance;
    public float waitTimeBasePosition;
    public float waitTimeActionPosition;

    public GameObject dustEffect;
    public GameObject dustDownEffect;

    private Vector3 endpos;
    private Vector3 startPos;

    private Vector3 currentEndpos;
    private Vector3 currentStartPos;

    private float t;
    private bool isMoving = true;
    private bool actionMoveCircle = true;

    private float waitUntil;
    private Vector3 shakePos;

    private SpriteRenderer myRenderer;

    public override void Start() {
        base.Start();
        myRenderer = GetComponent<SpriteRenderer>();
        startPos = transform.position;
        endpos = new Vector3(transform.position.x + distance.x, transform.position.y + distance.y, transform.position.z);
        shakePos = startPos + Vector3.down * (Constants.WorldUnitsPerPixel() * 5);
        currentStartPos = startPos;
        currentEndpos = endpos;
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


    private Vector3 SmoothMove() {

        if (t <= 1.0) {

            t += Time.deltaTime / (actionMoveCircle ? secondsAction : secondsBase);
            float newTime = t;
            if (newTime > 1) {
                newTime = 1;
            }
            float easeFactor = newTime;

            EASING_FUNCTION easing = (actionMoveCircle ? easingAction : easingBase);

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
        if (currentEndpos == endpos) {
            actionMoveCircle = false;
            waitUntil = Time.timeSinceLevelLoad + waitTimeActionPosition;
        } else {
            actionMoveCircle = true;
            waitUntil = Time.timeSinceLevelLoad + waitTimeBasePosition;
        }

        if (currentEndpos == endpos && myRenderer.isVisible) {
            ActionCompleteEffect();
        }

        Vector3 dummy = currentStartPos;
        currentStartPos = currentEndpos;
        currentEndpos = dummy;
        
        return Vector3.zero;
    }

    private void ActionCompleteEffect() {
        CameraFollow.GetInstance().ShakeSmall();
        Vector3 effectPosition1 = new Vector3(myRenderer.bounds.min.x + 0.2F, myRenderer.bounds.max.y, myRenderer.bounds.min.z);
        Vector3 effectPosition3 = new Vector3(myRenderer.bounds.max.x - 0.2F, myRenderer.bounds.max.y, myRenderer.bounds.min.z);
        InstantiateEffect2(dustEffect, effectPosition1, 180F);
        InstantiateEffect2(dustEffect, effectPosition3, 180F);
        Vector3 dustEffectPosition1 = new Vector3(myRenderer.bounds.max.x + 0.2F, myRenderer.bounds.max.y, myRenderer.bounds.min.z);
        Vector3 dustEffectPosition2 = new Vector3(myRenderer.bounds.min.x , myRenderer.bounds.max.y, myRenderer.bounds.min.z);
        InstantiateEffect2(dustDownEffect, dustEffectPosition1);
        InstantiateEffect2(dustDownEffect, dustEffectPosition2);
    }


    public void InstantiateEffect2(GameObject effectToInstanciate, Vector2 position, float rotateAngel = 0F, Transform parent = null) {
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

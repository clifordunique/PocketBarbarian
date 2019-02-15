using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMoveController2D : MonoBehaviour, ITriggerReactor {

    public float seconds;
    public float distanceUp;
    public float waitUnderWater;

    public Transform waterSurface;
    public GameObject waterEffectIn;
    public GameObject waterEffectOut;

    public bool reactToTrigger;
    private bool triggerMoveUp = false;

    private float t;
    private Vector3 currentStartPos;
    private Vector3 currentEndpos;
    private Vector3 positionUp;
    private float waterSurfaceY;

    private Animator animator;
    private bool moveUp = true;
    private bool waterEffectPlayed = false;

    private float reachedBottomTime;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        currentStartPos = Utils.MakePixelPerfect(transform.position);
        currentEndpos = Utils.MakePixelPerfect(new Vector3(transform.position.x, transform.position.y + distanceUp, transform.position.z));
        positionUp = currentEndpos;
        waterSurfaceY = waterSurface.position.y;
    }
	
	// Update is called once per frame
	void Update () {
        
        if (moveUp && t == 0F && StayDown()) {
            // wait under water
        } else {
            Vector2 velocity = SmoothMove();
            Vector2 pixelPerfectVelocity = Utils.MakePixelPerfect(velocity);
            transform.Translate(pixelPerfectVelocity);

        }

        if (moveUp) {
            // check if surfaceBreached Up
            if (transform.position.y > waterSurfaceY && !waterEffectPlayed) {
                InstantiateEffect(waterEffectOut);
                waterEffectPlayed = true;
            }
        } else {
            // check if surfaceBreached Down
            if (transform.position.y < waterSurfaceY && !waterEffectPlayed) {
                InstantiateEffect(waterEffectIn);
                waterEffectPlayed = true;
            }
        }
    }

    private bool StayDown() {
        if (reactToTrigger) {
            return !triggerMoveUp;
        } else {
            return Time.time < (reachedBottomTime + waitUnderWater);
        }
    }


    private Vector3 SmoothMove() {

        if (t <= 1.0) {

            t += Time.deltaTime / seconds;
            float newTime = t;
            if (newTime > 1) {
                newTime = 1;
            }
            float easeFactor = EasingFunction.EaseInOutCubic(0.0F, 1.0F, newTime);
            Vector3 newPosition = Vector3.Lerp(currentStartPos, currentEndpos, easeFactor);

            return newPosition - transform.position;

        } else {
            if (transform.position == positionUp) {

                animator.SetBool("UP", false);
                animator.SetBool("TURN", true);
                moveUp = false;
            } else {
                animator.SetBool("UP", true);
                animator.SetBool("TURN", false);
                moveUp = true;
                reachedBottomTime = Time.time;
                triggerMoveUp = false;
            }

            t = 0F;
            
            Vector3 dummy = currentStartPos;
            currentStartPos = currentEndpos;
            currentEndpos = dummy;
            waterEffectPlayed = false;
        }
        return Vector3.zero;
    }



    public void InstantiateEffect(GameObject effectToInstanciate) {
        GameObject effect = (GameObject)Instantiate(effectToInstanciate);
        effect.transform.parent = EffectCollection.GetInstance().transform;
        effect.transform.position = new Vector3(transform.position.x, waterSurfaceY, transform.position.z);
    }

    public bool TriggerActivated() {
        Debug.Log("Fish trigger Activated");
        triggerMoveUp = true;
        return true;
    }

    public bool TriggerDeactivated() {
        //nix zu tun
        return true;
    }
}

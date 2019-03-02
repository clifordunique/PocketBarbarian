using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveAirController2D : MonoBehaviour, IEnemyMoveController2D {
    
    public bool moveSinus = false;
    [ConditionalHideAttribute("moveSinus", true)]
    public float frequency = 20.0f;  // Speed of sine movement
    [ConditionalHideAttribute("moveSinus", true)]
    public float magnitude = 0.5f;   // Size of sine movement

    public float speed = 4f;
    public float smoothTime = 0.1f;
    public float pushForce = 15F;
    public float pushDuration = 0.2F;


    private bool isPushed = false;
    private float timePushed;

    private Vector3 targetPosition;
    private Vector3 velocity = Vector3.zero;    

    float velocityXSmoothing;
    float velocityYSmoothing;


    void Start() {
        targetPosition = transform.position;
    }

	
	void Update () {

        if (isPushed) {
            if (timePushed < Time.timeSinceLevelLoad) {
                targetPosition = transform.position;
                isPushed = false;
                velocity = Vector3.zero;
                return;
            }
        }

        if (targetPosition != transform.position) {
            velocity.x = Mathf.SmoothDamp(velocity.x, targetPosition.x, ref velocityXSmoothing, smoothTime);
            velocity.y = Mathf.SmoothDamp(velocity.y, targetPosition.y, ref velocityYSmoothing, smoothTime);
            transform.position += velocity * Time.deltaTime;
        }
    }

    public void OnPush(float pushDirectionX, float pushDirectionY, bool dash) {
        isPushed = true;
        timePushed = Time.timeSinceLevelLoad + pushDuration;
        velocity = Vector3.zero;
        targetPosition = new Vector3(pushDirectionX * pushForce, pushDirectionY * pushForce * 0.5F, transform.position.z);
    }


    public float MoveTo(Vector3 target, RectangleBound rectangleBound = null) {

        // no movement if pushed
        if (isPushed) return 0;

        float directionX = 0;
        bool stopMoving = false;
        
        if ( MoveUtils.TargetReachedXY(transform, target, speed, smoothTime)) { 
            // Target already reached
            stopMoving = true;
        }

        // Check if actor ist still in bounds
        if (!stopMoving && rectangleBound != null && !rectangleBound.IsInBoundX(transform.position)) {
            stopMoving = true;
        }

        if (stopMoving) {
            return 0;
        } else if (target != Vector3.positiveInfinity) {
            if (transform.position.x > target.x) directionX = -1;
            if (transform.position.x < target.x) directionX = 1;
        }


        // perform moving action
        //targetPosition = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        targetPosition = target - transform.position;
        targetPosition = targetPosition / targetPosition.magnitude * speed;
        if (moveSinus) {
            targetPosition = targetPosition + Vector3.up * (Mathf.Sin(Time.timeSinceLevelLoad * frequency) * magnitude);
        }
        return directionX;
    }

    public void StopMoving() {
        if (!isPushed) {
            targetPosition = transform.position;
            velocity = Vector3.zero;
        }
    }

    public bool IsGrounded() {
        return false;
    }

    public void OnJumpInputDown() {
        //no jump in flight
    }

    public float GetPushDuration() {
        return pushDuration;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveAirController2D : MonoBehaviour, IEnemyMoveController2D {

    public bool moveSinus = false;
    public float frequency = 20.0f;  // Speed of sine movement
    public float magnitude = 0.5f;   // Size of sine movement

    public float speed = 4f;

    private bool isPushed = false;
    private float timePushed;

    private Vector3 targetPosition;
    private Vector3 velocity;    

    float velocityXSmoothing;
    float velocityYSmoothing;


    void Start() {
        targetPosition = transform.position;
    }

	
	void Update () {        
        
        if (isPushed) {
            if (timePushed < Time.time) {
                targetPosition = transform.position;
                isPushed = false;
            } else {

                velocity.x = Mathf.SmoothDamp(velocity.x, targetPosition.x, ref velocityXSmoothing, 0.1F);
                velocity.y = Mathf.SmoothDamp(velocity.y, targetPosition.y, ref velocityYSmoothing, 0.1F);
                transform.position += velocity * Time.deltaTime;
            }
        } else {

            velocity.x = Mathf.SmoothDamp(velocity.x, targetPosition.x, ref velocityXSmoothing, 0.1F);
            velocity.y = Mathf.SmoothDamp(velocity.y, targetPosition.y, ref velocityYSmoothing, 0.1F);
            transform.position += velocity * Time.deltaTime; 
        }
    }

    public void OnPush(float pushDirectionX, float pushDirectionY, float pushForce, float pushDuration) {
        isPushed = true;
        timePushed = Time.time + pushDuration;
        targetPosition = new Vector3(pushDirectionX * pushForce, pushDirectionY * pushForce * 0.5F, transform.position.z);
    }


    public float MoveTo(Vector3 target, RectangleBound rectangleBound = null) {

        // no movement if pushed
        if (isPushed) return 0;

        float directionX = 0;
        bool stopMoving = false;

        float distance = Vector3.Distance(transform.position, target);
        if ( MoveUtils.TargetReachedXY(transform, target)) { //(moveSinus && distance < magnitude)
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
            targetPosition = targetPosition + Vector3.up * (Mathf.Sin(Time.time * frequency) * magnitude);
        }
        return directionX;
    }

    public void StopMoving() {
        if (!isPushed) {
            targetPosition = transform.position;
        }
    }

    public bool IsGrounded() {
        return false;
    }

    public void OnJumpInputDown() {
        //no jump in flight
    }
}

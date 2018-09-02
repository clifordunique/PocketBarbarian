﻿using UnityEngine;
using System.Collections;


public class CharacterController2D : Controller2D {

	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    public bool doubleJumpAllowed = false;
    public float comicFallFactor = 1.08f;
    public float stampingFallFactor = 2f;
    
    public float moveSpeed = 6;
    public float moveSpeedDash = 12;
    public float dashDuration = 0.5F;

    public float accelerationTimeAirborne = 0F;//.2f;
    public float accelerationTimeGrounded = 0F;//.05f;

    float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
    float targetVelocityX;
    float velocityXSmoothing;

    bool isStamping = false;
    bool isPushed = false;
    float endTimePush;
    int jumpCounter = 0;


    public override void Start() {
        base.Start();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
	}

	void Update() {
		CalculateVelocity ();

		Move (velocity * Time.deltaTime);

		if (collisions.above || collisions.below) {			
			velocity.y = 0;
            isStamping = false;
            jumpCounter = 0;
        }
        
        if (isPushed && endTimePush < Time.time) {
            isPushed = false;
        }
	}

	public void OnMove (float moveDirectionX, float moveDirectionY) {		
        if (!isPushed) {
            this.moveDirectionX = moveDirectionX;
            this.moveDirectionY = moveDirectionY;
            targetVelocityX = moveDirectionX * moveSpeed;
        }
    }

	public void OnJumpInputDown() {
        if (collisions.below || (doubleJumpAllowed && jumpCounter == 1)) {
            velocity.y = maxJumpVelocity;
            jumpCounter++;
        }
	}

	public void OnJumpInputUp() {
		if (velocity.y > minJumpVelocity) {
			velocity.y = minJumpVelocity;
		}
	}

    public void OnStamp() {
        if (!collisions.below) {
            isStamping = true;
            velocity.y = 0;
        }
    }

    public void OnDash() {
        if (collisions.below) {
            isPushed = true;
            targetVelocityX = moveDirectionX * moveSpeedDash;
            endTimePush = Time.time + dashDuration;
        }
    }

    public void OnPush(float pushDirectionX, float pushDirectionY, float pushForce, float pushDuration) {
        isPushed = true;
        targetVelocityX = pushDirectionX * pushForce;
        if (pushDirectionY > 0) {
            velocity.y = minJumpVelocity;
        }
        endTimePush = Time.time + pushDuration;
    }

    public bool IsFalling () {
        return (!collisions.below && velocity.y <= 0);
    }


    void CalculateVelocity() {
        
        velocity.y += gravity * Time.deltaTime;

        if (isStamping && IsFalling() && !isPushed) {
            // if stamping, no movement!
            velocity.x = 0;
            velocity.y *= stampingFallFactor;
        } else {
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

            if (IsFalling()) {
                // is falling, comic Fall Factor
                velocity.y *= comicFallFactor;
            }
        }
	}
}

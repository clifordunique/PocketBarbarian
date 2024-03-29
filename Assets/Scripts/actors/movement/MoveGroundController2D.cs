﻿using UnityEngine;
using System.Collections;


public class MoveGroundController2D: MoveController2D {

    [Header("In Air Settings")]
    public float maxGravitySpeed = -20F;
    public bool jumpingAllowed = false;
    [ConditionalHide("jumpingAllowed", true)]
    public float maxJumpHeight = 0;
    [ConditionalHide("jumpingAllowed", true)]
    public float minJumpHeight = 0;
    [ConditionalHide("jumpingAllowed", true)]
    public float timeToJumpApex = 0;
    [ConditionalHide("jumpingAllowed", true)]
    public float accelerationTimeAirborne = 0F;//.2f;
    [ConditionalHide("jumpingAllowed", true)]
    public float comicFallFactor = 1.08f;
    [ConditionalHide("jumpingAllowed", true)]
    public bool doubleJumpAllowed = false;

    

    [Header("On Ground Settings")]
    public float moveSpeed = 6;
    public float accelerationTimeGrounded = 0F;//.05f;

    [Header("Push Settings")]
    public float pushForce = 25F;
    public float pushDuration = 0.15F;
    public bool jumpOnPush = false;

    [HideInInspector]
    public Vector3 velocity;
    [HideInInspector]
    public float targetVelocityX;
    [HideInInspector]
    public float gravity;
    [HideInInspector]
    public float maxJumpVelocity;
    [HideInInspector]
    public float minJumpVelocity;	
    
    float velocityXSmoothing;
    
    [HideInInspector]
    public bool isPushed = false;
    [HideInInspector]
    public bool isWallJump = false;
    [HideInInspector]
    public float endTimePush;

    

    int jumpCounter = 0;


    public override void Start() {
        base.Start();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
	}

	public virtual void Update() {
        CalculateVelocity();
        Move(velocity * Time.deltaTime);

        if (collisions.above || collisions.below) {
            velocity.y = 0;
            jumpCounter = 0;
        }

        if (isPushed && endTimePush < Time.timeSinceLevelLoad) {
            isPushed = false;
            targetVelocityX = 0;
        }
        //moveDirectionX = 0;
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
    
    public void OnPush(float pushDirectionX, float pushForce, float pushDuration) {
        isPushed = true;
        targetVelocityX = pushDirectionX * pushForce;
        if (jumpOnPush) {
            velocity.y = minJumpVelocity;
        }
        endTimePush = Time.timeSinceLevelLoad + pushDuration;
    }

    public void OnPush(float pushDirectionX, float pushDirectionY) {
        OnPush(pushDirectionX, pushForce, pushDuration);
    }

    public void OnPush(float pushDirectionX, float pushDirectionY, bool dash) {
        if (dash) {
            OnPush(pushDirectionX, pushForce * 2, pushDuration);
        } else {
            OnPush(pushDirectionX, pushForce, pushDuration);
        }
    }

    public float GetPushDuration() {
        return pushDuration;
    }

    public bool IsFalling () {
        return (collisions.initialised && !collisions.below && velocity.y < 0);
    }

    public bool IsGrounded() {
        return (collisions.below && velocity.y == 0);
    }



    public virtual float GetFallFactor() {
        return comicFallFactor;
    }

    public virtual void CalculateVelocity() {
        if (IsFalling()) {
            // is falling, comic Fall Factor
            velocity.y += gravity * Time.deltaTime * GetFallFactor();
            if (velocity.y < maxGravitySpeed * GetFallFactor()) {
                velocity.y = maxGravitySpeed * GetFallFactor();
            }            
        } else {
            velocity.y += gravity * Time.deltaTime;
        }        
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);        
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController2D : MoveGroundController2D {

    [Header("Wall Jump Settings")]
    public bool wallJumpingAllowed = false;
    [ConditionalHide("wallJumpingAllowed", true)]
    public float maxWallSlideSpeed;
    [ConditionalHide("wallJumpingAllowed", true)]
    public float wallSlideBeginTime;
    [ConditionalHide("wallJumpingAllowed", true)]
    public float wallJumpTime;

    [Header("Dash Settings")]
    public bool dashAllowed = false;
    [ConditionalHide("dashAllowed", true)]
    public float moveSpeedDash = 12;
    [ConditionalHide("dashAllowed", true)]
    public float dashDuration = 0.5F;
    [ConditionalHide("dashAllowed", true)]
    public float dashPushbackForce = 10;
    [ConditionalHide("dashAllowed", true)]
    public float dashPushbackDuration = 0.1F;

    [Header("Stamping Settings")]
    public bool stampingAllowed;
    [ConditionalHide("stampingAllowed", true)]
    public float stampingFallFactor = 2f;

    bool isStamping = false;

    private bool wallSliding = false;
    private float wallContactTime = 0;
    private float wallJumpStartTime = 0;


    public override void Update() {
        base.Update();
        // if contact with surface, stop walljump
        if (isPushed || collisions.above || collisions.below || collisions.left || collisions.right || wallJumpStartTime + wallJumpTime < Time.timeSinceLevelLoad) {
            isWallJump = false;
        }

        if (collisions.below) {
            // Stop stamping
            isStamping = false;
        }
    }


    

    public void OnStamp() {
        if (stampingAllowed) { // && !collisions.below) {
            isStamping = true;
            if (velocity.y > 0) {
                velocity.y = 0;
            }
        }
    }

    public void OnWallJumpInputDown() {
        if (wallSliding && jumpingAllowed && wallJumpingAllowed) {

            velocity.y = maxJumpVelocity;
            if (collisions.left) {
                targetVelocityX = moveSpeed;
            }
            if (collisions.right) {
                targetVelocityX = -moveSpeed;
            }
            moveDirectionX = Mathf.Sign(targetVelocityX);
            isWallJump = true;
            wallJumpStartTime = Time.timeSinceLevelLoad;
        }
    }

    public bool IsWallSliding() {
        return wallSliding;
    }

    public override float GetFallFactor() {
        //if (IsFalling() && isStamping) {
        if (isStamping) {
            return stampingFallFactor;
        } else {
            return comicFallFactor;
        }
    }

    public void OnMove(float moveDirectionX, float moveDirectionY, bool dashMove = false) {
        if (!isPushed && !isWallJump) {
            this.moveDirectionX = moveDirectionX;
            this.moveDirectionY = moveDirectionY;
            if (dashMove) {
                targetVelocityX = moveDirectionX * moveSpeedDash;
            } else {
                targetVelocityX = moveDirectionX * moveSpeed;
            }
        }
    }



    public override void CalculateVelocity() {
        base.CalculateVelocity();
        
        // Wall jumping settings
        if (IsFalling()) {
            if (IsWallSlidePossible()) {
                wallContactTime += Time.deltaTime;

                if (wallContactTime >= wallSlideBeginTime) {
                    wallSliding = true;
                    if (velocity.y < -maxWallSlideSpeed) {
                        velocity.y = -maxWallSlideSpeed;
                    }
                }
            } else {
                wallSliding = false;
                wallContactTime = 0;
            }
        } else {
            wallSliding = false;
            wallContactTime = 0;
        }
        if (isWallJump && !isPushed) {
            velocity.x = targetVelocityX;
        } 

        // Stamping Settings
        if (isStamping && IsFalling() && !isPushed) {
            velocity.x = 0;
        }
    }


    private bool IsWallSlidePossible() {
        if (wallJumpingAllowed) {
            if ((moveDirectionX < 0 && collisions.left) || (moveDirectionX > 0 && collisions.right)) {
                return true;
            }
        }
        return false;
    }
}

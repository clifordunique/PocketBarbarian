using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController2D : MoveGroundController2D {    

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

    public override void Update() {
        base.Update();

        if (collisions.below) {
            // Stop stamping
            isStamping = false;
        }
    }

    public override void CalculateVelocity() {
        base.CalculateVelocity();

        // Stamping Settings
        if (isStamping && IsFalling() && !isPushed) {
            velocity.x = 0;
        }
    }
    

    public void OnStamp() {
        if (stampingAllowed && !collisions.below) {
            isStamping = true;
            velocity.y = 0;
        }
    }

    public override float GetFallFactor() {
        if (IsFalling() && isStamping) {
            return stampingFallFactor;
        } else {
            return comicFallFactor;
        }
    }

    public void OnMove(float moveDirectionX, float moveDirectionY, bool dashMove = false) {
        if (!isPushed) {
            this.moveDirectionX = moveDirectionX;
            this.moveDirectionY = moveDirectionY;
            if (dashMove) {
                targetVelocityX = moveDirectionX * moveSpeedDash;
            } else {
                targetVelocityX = moveDirectionX * moveSpeed;
            }
        } else {
            Debug.Log("Not Stopping, still pushed!");
        }
    }
}

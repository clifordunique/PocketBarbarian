using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveGroundController2D : MoveGroundController2D, IEnemyMoveController2D {

    public float pushForce = 25F;
    public float pushDuration = 0.15F;

    public override void Update() {
        base.Update();
    }


    // Moves GameObject in the direction of the target. Returns direction it is heading.
    public float MoveTo(Vector3 target, RectangleBound rectangleBound = null) {

        bool stopMoving = false;
        float directionX = 0;

        // Check if actor ist still in bounds
        if (rectangleBound != null && !rectangleBound.IsInBoundX(transform.position)) {
                stopMoving = true;
        }

        // check if target is reached
        if (MoveUtils.TargetReachedX(transform, target)) {
            stopMoving = true;            
        }

        if (stopMoving) {
            StopMoving();
        } else if (target != Vector3.positiveInfinity) {
            if (transform.position.x > target.x) directionX = -1;
            if (transform.position.x < target.x) directionX = 1;
        }
        
        OnMove(directionX, 0F);        
        return directionX;
    }

    public void StopMoving() {
        OnMove(0F, 0F);
    }

    public void OnPush(float pushDirectionX, float pushDirectionY) {
        OnPush(pushDirectionX, pushDirectionY, pushForce, pushDuration);
    }
}

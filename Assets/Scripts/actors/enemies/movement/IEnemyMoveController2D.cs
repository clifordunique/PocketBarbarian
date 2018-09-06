using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IEnemyMoveController2D {

    bool IsGrounded();

    void OnJumpInputDown();

    void OnPush(float pushDirectionX, float pushDirectionY, float pushForce, float pushDuration);

    float MoveTo(Vector3 target, RectangleBound rectangleBound = null);

    void StopMoving();
 }

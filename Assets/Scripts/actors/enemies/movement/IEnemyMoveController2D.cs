using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IEnemyMoveController2D {

    float GetPushDuration();

    bool IsGrounded();

    void OnJumpInputDown();

    void OnPush(float pushDirectionX, float pushDirectionY);

    float MoveTo(Vector3 target, RectangleBound rectangleBound = null);

    void StopMoving();
 }

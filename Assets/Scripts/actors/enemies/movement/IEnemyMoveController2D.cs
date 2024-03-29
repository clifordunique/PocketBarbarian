﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IEnemyMoveController2D {

    float GetPushDuration();

    bool IsGrounded();

    bool IsFalling();

    void OnJumpInputDown();

    void OnPush(float pushDirectionX, float pushDirectionY, bool dash);

    float MoveTo(Vector3 target, RectangleBound rectangleBound = null);

    void StopMoving();
 }

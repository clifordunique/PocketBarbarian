﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : AbstractState {

    public MoveState(PlayerController playerController) : base(ACTION.MOVE, playerController) {
    }

    public override void OnEnter() {
        playerController.animator.SetBool(RUNNING_PARAM, true);
        Move(input.GetDirectionX(), input.GetDirectionY());
    }

    public override void OnExit() {
        playerController.animator.SetBool(RUNNING_PARAM, false);
        // last move to stop movement
        Move(input.GetDirectionX(), input.GetDirectionY());
    }

    public override AbstractState UpdateState() {
        base.UpdateState();

        if (input.GetDirectionX() == 0) {
            return new IdleState(playerController);
        }
        if (input.IsJumpKeyDown()) {
            return new JumpState(playerController);
        }
        if (playerController.moveController.IsFalling()) {
            return new FallingState(playerController);
        }
        Move(input.GetDirectionX(), input.GetDirectionY());
        return null;
    }


}

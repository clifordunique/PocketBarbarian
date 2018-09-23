﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : AbstractState {

    public JumpState(PlayerController playerController) : base(ACTION.JUMP, playerController) {
    }

    public override void OnEnter() {
        playerController.animator.SetBool(JUMPING_PARAM, true);
        playerController.InstantiateEffect(playerController.prefabEffectJump);
        playerController.moveController.OnJumpInputDown();
        Move(input.GetDirectionX(), input.GetDirectionY());
    }

    public override void OnExit() {
        playerController.animator.SetBool(JUMPING_PARAM, false);
        Move(input.GetDirectionX(), input.GetDirectionY());
    }

    public override AbstractState UpdateState() {
        base.UpdateState();

        if (playerController.moveController.IsGrounded()) {
            playerController.InstantiateEffect(playerController.prefabEffectLanding);
            if (input.GetDirectionX() == 0) {
                return new IdleState(playerController);
            } else {
                return new MoveState(playerController);
            }
        }
        if (input.IsJumpKeyUp()) {
            playerController.moveController.OnJumpInputUp();
        }

        Move(input.GetDirectionX(), input.GetDirectionY());
        return null;
    }


}

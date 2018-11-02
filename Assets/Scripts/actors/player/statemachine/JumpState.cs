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
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }

        if (playerController.moveController.IsGrounded()) {
            return new LandingState(playerController);
        }

        if (input.IsJumpKeyUp()) {
            playerController.moveController.OnJumpInputUp();
        }

        if (playerController.moveController.IsFalling() && input.DownKeyDown()) {
            // check if Stomping possible
            if (playerController.HasEnoughStaminaForStomp()) {
                return new StompingState(playerController);
            }
        }

        if (input.IsAttack1KeyDown()) {
            return new JumpAttackState(playerController);
        }

        if (input.IsAttack2KeyDown() && playerController.statistics.ammo > 0) {
            return new ThrowJumpState(playerController);
        }

        Move(input.GetDirectionX(), input.GetDirectionY());
        return null;
    }


}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpState : AbstractState {

    public WallJumpState(PlayerController playerController) : base(ACTION.JUMP, playerController) {
    }

    public override void OnEnter() {
        playerController.animator.SetBool(WALL_JUMP_PARAM, true);
        playerController.InstantiateEffect(playerController.prefabEffectWallJump);
        playerController.moveController.OnWallJumpInputDown();
        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());
    }

    public override void OnExit() {
        playerController.animator.SetBool(WALL_JUMP_PARAM, false);
        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());
    }

    public override AbstractState UpdateState() {
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }



        if (playerController.moveController.IsGrounded()) {
            return new LandingState(playerController);
        }

        if (playerController.moveController.IsWallSliding()) {
            return new WallSlidingState(playerController);
        }

        if (playerController.moveController.IsFalling()) {
            return new FallingState(playerController);
        }

        if (playerController.input.IsJumpKeyUp()) {
            playerController.moveController.OnJumpInputUp();
        }        

        if (playerController.hasWeapon && playerController.input.IsAttack1KeyDown()) {
            return new JumpAttackState(playerController);
        }

        if (playerController.hasWeapon && playerController.input.IsAttack2KeyDown()) { // hat ammo && playerController.statistics.ammo > 0) {
            return new ThrowJumpState(playerController);
        }
        

        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());
        return null;
    }


}

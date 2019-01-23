﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : AbstractState {

    public IdleState(PlayerController playerController) : base(ACTION.IDLE, playerController) {
    }

    public override void OnEnter() {
        playerController.animator.SetBool(IDLE_PARAM, true);
    }

    public override void OnExit() {
        playerController.animator.SetBool(IDLE_PARAM, false);
    }

    public override AbstractState UpdateState() {
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }

        if (playerController.moveController.IsFalling()) {
            return new FallingState(playerController);
        }

        if (playerController.input.IsJumpKeyDown()) {
            return new JumpState(playerController);
        }
        if (playerController.input.GetDirectionX() != 0) {
            return new MoveState(playerController);
        }

        if (playerController.input.GetDirectionY() == 1 && playerController.interactableInRange) {
            return new ActionState(playerController);
        }

        if (playerController.hasWeapons && playerController.input.IsAttack1KeyDown()) {
            return new Attack1State(playerController);
        }

        if (playerController.hasWeapons && playerController.input.IsAttack2KeyDown()) { // hat ammo && playerController.statistics.ammo > 0) {
            return new ThrowIdleState(playerController);
        }

        Move(0, playerController.input.GetDirectionY());
        return null;
    }


}

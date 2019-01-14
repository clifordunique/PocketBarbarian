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
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }

        if (input.GetDirectionX() == 0) {
            return new IdleState(playerController);
        }
        if (input.IsJumpKeyDown()) {
            return new JumpState(playerController);
        }
        if (playerController.moveController.IsFalling()) {
            return new FallingState(playerController);
        }
        if (playerController.hasWeapons && input.IsAttack1KeyDown()) {
            return new Attack2State(playerController);
        }
        if (playerController.hasWeapons && input.IsAttack2KeyDown() && playerController.statistics.ammo > 0) {
            return new ThrowRunningState(playerController);
        }
        if (playerController.hasWeapons && input.IsDashing()) {
            // check if Dashing possible
            if (playerController.statistics.HasEnoughStaminaForAction()) {
                return new DashingState(playerController);
            }
        }
        Move(input.GetDirectionX(), input.GetDirectionY());
        return null;
    }

    public override void HandleEvent(string parameter) {
        // Instanciate step effect
        playerController.InstantiateEffect(playerController.prefabEffectStep);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowJumpState : AbstractState {

    private bool attackFinished = false;

    public ThrowJumpState(PlayerController playerController) : base(ACTION.SHOOT, playerController) {
    }

    public override void OnEnter() {
        playerController.animator.SetBool(THROW_JUMP_PARAM, true);
        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());
    }

    public override void OnExit() {
        playerController.animator.SetBool(THROW_JUMP_PARAM, false);
        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());
    }

    public override AbstractState UpdateState() {
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }

        if (attackFinished) {
            if (playerController.moveController.IsGrounded()) {
                return new MoveState(playerController);
            } else {
                return new FallingState(playerController);
            }
        }

        if (playerController.moveController.IsGrounded()) {
            return new LandingState(playerController);
        }


        if (!playerController.moveController.IsFalling() && playerController.input.IsJumpKeyUp()) {
            playerController.moveController.OnJumpInputUp();
        }        

        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());
        return null;
    }

    public override void HandleEvent(string parameter) {
        if (parameter == EVENT_PARAM_ANIMATION_END) {
            attackFinished = true;
        }
        if (parameter == EVENT_PARAM_THROW) {
            playerController.ShootProjectile();
            playerController.statistics.ModifyAmmo(-1);
        }
    }
}

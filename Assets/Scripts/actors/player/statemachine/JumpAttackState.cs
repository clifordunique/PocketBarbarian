using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttackState : AbstractState {

    private bool attackFinished = false;

    public JumpAttackState(PlayerController playerController) : base(ACTION.JUMP, playerController) {
    }

    public override void OnEnter() {
        playerController.animator.SetBool(JUMPING_ATTACK_PARAM, true);
        Move(input.GetDirectionX(), input.GetDirectionY());
    }

    public override void OnExit() {
        playerController.animator.SetBool(JUMPING_ATTACK_PARAM, false);
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

        if (attackFinished) {
            if (!playerController.moveController.IsGrounded()) {
                return new LandingState(playerController);
            } else {
                return new FallingState(playerController);
            }
        }


        if (!playerController.moveController.IsFalling() && input.IsJumpKeyUp()) {
            playerController.moveController.OnJumpInputUp();
        }        

        Move(input.GetDirectionX(), input.GetDirectionY());
        return null;
    }

    public override void HandleEvent(string parameter) {
        if (parameter == EVENT_PARAM_ANIMATION_END) {
            attackFinished = true;
        }
    }
}

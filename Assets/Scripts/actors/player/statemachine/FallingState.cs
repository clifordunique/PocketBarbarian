using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : AbstractState {

    public FallingState(PlayerController playerController) : base(ACTION.JUMP, playerController) {
    }

    public override void OnEnter() {
        playerController.animator.SetBool(JUMPING_PARAM, true);
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

        if (playerController.moveController.IsFalling() && Input.GetKeyDown(KeyCode.DownArrow)) {
            return new StompingState(playerController);
        }

        Move(input.GetDirectionX(), input.GetDirectionY());
        return null;
    }


}

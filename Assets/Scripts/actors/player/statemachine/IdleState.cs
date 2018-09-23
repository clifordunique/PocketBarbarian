using System.Collections;
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
        base.UpdateState();

        if (input.IsJumpKeyDown()) {
            return new JumpState(playerController);
        }
        if (input.GetDirectionX() != 0) {
            return new MoveState(playerController);
        }
        if (input.GetDirectionY() == -1) {
            // try to move down
            Move(0, input.GetDirectionY());
        }
        if (playerController.moveController.IsFalling()) {
            return new FallingState(playerController);
        }
        return null;
    }


}

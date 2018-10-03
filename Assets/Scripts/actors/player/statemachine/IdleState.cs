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
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }

        if (playerController.moveController.IsFalling()) {
            return new FallingState(playerController);
        }

        if (input.IsJumpKeyDown()) {
            return new JumpState(playerController);
        }
        if (input.GetDirectionX() != 0) {
            return new MoveState(playerController);
        }
        if (input.IsAttack1KeyDown()) {
            return new Attack1State(playerController);
        }

        if (input.GetDirectionY() == 1 && playerController.interactableInRange) {
            return new ActionState(playerController);
        }

        Move(0, input.GetDirectionY());
        return null;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : AbstractState {

    public JumpState(PlayerController playerController) : base(ACTION.JUMP, playerController) {
    }

    public override void OnEnter() {
        playerController.animator.SetBool(JUMPING_PARAM, true);
        playerController.InstantiateEffect(playerController.prefabEffectJump);
        playerController.moveController.OnJumpInputDown();
        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());
    }

    public override void OnExit() {
        playerController.animator.SetBool(JUMPING_PARAM, false);
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

        if (playerController.DoAttack()) {
            return new JumpAttackState(playerController);
        }

        if (playerController.DoThrow()) {
            return new ThrowJumpState(playerController);
        }


        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());
        return null;
    }


}

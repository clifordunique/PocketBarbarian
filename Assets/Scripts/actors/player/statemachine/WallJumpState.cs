using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpState : AbstractState {

    public WallJumpState(PlayerController playerController) : base(ACTION.JUMP, playerController) {
    }

    public override void OnEnter() {
        playerController.animator.SetBool(JUMPING_PARAM, true);
        playerController.InstantiateEffect(playerController.prefabEffectWallJump);
        playerController.moveController.OnWallJumpInputDown();
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

        if (playerController.moveController.IsWallSliding()) {
            return new WallSlidingState(playerController);
        }

        if (playerController.moveController.IsFalling()) {
            return new FallingState(playerController);
        }

        if (input.IsJumpKeyUp()) {
            playerController.moveController.OnJumpInputUp();
        }        

        if (playerController.hasWeapons && input.IsAttack1KeyDown()) {
            return new JumpAttackState(playerController);
        }

        if (playerController.hasWeapons && input.IsAttack2KeyDown() && playerController.statistics.ammo > 0) {
            return new ThrowJumpState(playerController);
        }

        if (Time.frameCount % 6 == 0) {
            playerController.InstantiateEffect(playerController.prefabEffectWallJump);
        }

        Move(input.GetDirectionX(), input.GetDirectionY());
        return null;
    }


}

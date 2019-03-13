using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : AbstractState {

    private const float HIGH_FALL_DISTANCE = 9F;
    private float startFallingY;

    public FallingState(PlayerController playerController) : base(ACTION.JUMP, playerController) {
    }

    public override void OnEnter() {
        startFallingY = playerController.transform.position.y;
        playerController.animator.SetBool(FALLING_PARAM, true);
        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());
    }

    public override void OnExit() {
        playerController.animator.SetBool(FALLING_PARAM, false);
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

        if (playerController.statistics.stompAllowed && playerController.moveController.IsFalling() && playerController.input.DownKeyDown()) {
            // check if Stomping possible
            if (playerController.statistics.HasEnoughStaminaForAction()) {
                return new StompingState(playerController);
            }
        }

        if (startFallingY - playerController.transform.position.y >= HIGH_FALL_DISTANCE) {
            // Fast falling!
            return new FastFallingState(playerController);
        }

        if (playerController.hasWeapon && playerController.input.IsAttack1KeyDown()) {
            return new JumpAttackState(playerController);
        }

        if (playerController.hasWeapon && playerController.input.IsAttack2KeyDown()) { // hat ammo! && playerController.statistics.ammo > 0) {
            return new ThrowJumpState(playerController);
        }

        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());        
        return null;
    }


}

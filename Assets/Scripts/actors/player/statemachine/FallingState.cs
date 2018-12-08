using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : AbstractState {

    private const float HIGH_FALL_DISTANCE = 6F;
    private float startFallingY;

    public FallingState(PlayerController playerController) : base(ACTION.JUMP, playerController) {
    }

    public override void OnEnter() {
        startFallingY = playerController.transform.position.y;
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

        if (playerController.hasWeapons && playerController.moveController.IsFalling() && input.DownKeyDown()) {
            // check if Stomping possible
            if (playerController.statistics.HasEnoughStaminaForStomp()) {
                return new StompingState(playerController);
            }
        }

        if (startFallingY - playerController.transform.position.y >= HIGH_FALL_DISTANCE) {
            // Fast falling!
            Debug.Log("FastFalling!");
            return new FastFallingState(playerController);
        }

        if (playerController.hasWeapons && input.IsAttack1KeyDown()) {
            return new JumpAttackState(playerController);
        }

        if (playerController.hasWeapons && input.IsAttack2KeyDown() && playerController.statistics.ammo > 0) {
            return new ThrowJumpState(playerController);
        }

        Move(input.GetDirectionX(), input.GetDirectionY());
        return null;
    }


}

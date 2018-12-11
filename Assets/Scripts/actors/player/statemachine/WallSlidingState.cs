using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlidingState: AbstractState {
    

    public WallSlidingState(PlayerController playerController) : base(ACTION.JUMP, playerController) {
    }

    public override void OnEnter() {
        playerController.sparkParticle.Play();
        playerController.animator.SetBool(WALL_SLIDING_PARAM, true);
        Move(input.GetDirectionX(), input.GetDirectionY());
    }

    public override void OnExit() {
        playerController.sparkParticle.Stop();
        playerController.animator.SetBool(WALL_SLIDING_PARAM, false);
        Move(input.GetDirectionX(), input.GetDirectionY());
    }

    public override AbstractState UpdateState() {
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }

        if (input.IsJumpKeyDown()) {
            return new WallJumpState(playerController);
        }

        if (playerController.moveController.IsGrounded()) {
            return new LandingState(playerController);
        }

        if (!playerController.moveController.IsWallSliding() && playerController.moveController.IsFalling()) {
            return new FallingState(playerController);
        }
        
        playerController.moveController.OnMove(input.GetDirectionX(), input.GetDirectionY(), false);
        return null;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlidingState: AbstractState {

    bool swordTouchWall = false;

    public WallSlidingState(PlayerController playerController) : base(ACTION.JUMP, playerController) {
    }

    public override void OnEnter() {        
        playerController.animator.SetBool(WALL_SLIDING_PARAM, true);
        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());
    }

    public override void OnExit() {
        playerController.sparkParticle.Stop();
        playerController.animator.SetBool(WALL_SLIDING_PARAM, false);
        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());
    }

    public override AbstractState UpdateState() {
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }

        if (swordTouchWall && !playerController.sparkParticle.isPlaying) {
            playerController.sparkParticle.Play();            
        }

        if (playerController.input.IsJumpKeyDown()) {
            return new WallJumpState(playerController);
        }

        if (playerController.moveController.IsGrounded()) {
            return new LandingState(playerController);
        }

        if (!playerController.moveController.IsWallSliding() && playerController.moveController.IsFalling()) {
            return new FallingState(playerController);
        }
        
        playerController.moveController.OnMove(playerController.input.GetDirectionX(), playerController.input.GetDirectionY(), false);
        return null;
    }


    public override void HandleEvent(string parameter) {
        if (parameter == EVENT_PARAM_ANIMATION_END) {
            swordTouchWall = true;
        }
    }


}

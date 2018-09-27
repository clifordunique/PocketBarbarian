using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkOutState : AbstractState {


    private bool animationEnded = false;

    public WalkOutState(PlayerController playerController) : base(ACTION.ACTION, playerController) {
    }

    public override void OnEnter() {
        playerController.animator.SetBool(WALK_OUT_PARAM, true);
    }

    public override void OnExit() {
        playerController.animator.SetBool(WALK_OUT_PARAM, false);
    }

    public override AbstractState UpdateState() {
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }

        if (playerController.moveController.IsFalling()) {
            return new FallingState(playerController);
        }

        if (animationEnded) {
            return new IdleState(playerController);
        }
        

        Move(0, 0);
        return null;
    }

    public override void HandleEvent(string parameter) {
        if (parameter == EVENT_PARAM_ANIMATION_END) {
            animationEnded = true;
        }
    }
}

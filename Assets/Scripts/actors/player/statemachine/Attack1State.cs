using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1State : AbstractState {

    private bool attackFinished = false;
    private bool hitSomething = false;

    public Attack1State(PlayerController playerController) : base(ACTION.ATTACK1, playerController) {
    }

    public override void OnEnter() {
        playerController.animator.SetBool(ATTACK1_PARAM, true);
    }

    public override void OnExit() {
        playerController.animator.SetBool(ATTACK1_PARAM, false);
    }

    public override AbstractState UpdateState() {
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }

        if (hitSomething) {
            // small cam Shake
            CameraFollow.GetInstance().ShakeSmall();
            hitSomething = false;
        }

        if (attackFinished) {
            return new IdleState(playerController);
        }
        return null;
    }

    public override void HandleEvent(string parameter) {
        if (parameter == EVENT_PARAM_ANIMATION_END) {
            attackFinished = true;
        }
        if (parameter == EVENT_PARAM_HIT) {
            hitSomething = true;
        }
    }
}

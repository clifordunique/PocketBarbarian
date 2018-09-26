using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1State : AbstractState {

    private bool attackFinished = false;

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

        if (attackFinished) {
            return new IdleState(playerController);
        }
        return null;
    }

    public override void HandleAnimEvent(string parameter) {
        attackFinished = true;
    }

}

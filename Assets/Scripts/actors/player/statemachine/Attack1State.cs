using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1State : AbstractState {

    private bool attackFinished = false;
    private bool comboAttack = false;
    private bool exitCombo = false;
    private bool hitSomething = false;
    private float moveTime = 0.13F;
    private float checkMoveStarted;

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

        if (input.IsAttack1KeyDown()) {
            comboAttack = true;
        }

        if (hitSomething) {
            // small cam Shake
            CameraFollow.GetInstance().ShakeSmall();
            hitSomething = false;
        }

        if (attackFinished || (!comboAttack && exitCombo)) {
            return new IdleState(playerController);
        }

        if (comboAttack && exitCombo) {
            if (checkMoveStarted > Time.time) {
                Move(playerController.dirX, input.GetDirectionY());
            } else {
                Move(0, input.GetDirectionY());
                checkMoveStarted = 0;
            }
        }
        return null;
    }

    public override void HandleEvent(string parameter) {
        if (parameter == EVENT_PARAM_ANIMATION_END) {
            attackFinished = true;
        }
        if (parameter == "exit_combo") {
            exitCombo = true;
            checkMoveStarted = Time.time + moveTime;
        }
        if (parameter == EVENT_PARAM_HIT) {
            hitSomething = true;
        }
    }
}

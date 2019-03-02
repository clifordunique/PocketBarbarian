using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2State : AbstractState {

    private bool attackFinished = false;
    private bool comboAttach = false;
    private bool exitCombo = false;
    private bool hitSomething = false;
    private bool doJump = false;
    private float moveTime1 = 0.4F;
    private float jumpTime = 0.02F;
    private float moveTime2 = 0.2F;
    private float checkMoveStarted;
    private float checkJumpStarted = -1F;

    public Attack2State(PlayerController playerController) : base(ACTION.ATTACK2, playerController) {
    }

    public override void OnEnter() {
        playerController.animator.SetBool(ATTACK2_PARAM, true);
        //Move(0, playerController.input.GetDirectionY());
    }

    public override void OnExit() {
        playerController.animator.SetBool(ATTACK2_PARAM, false);
    }

    public override AbstractState UpdateState() {
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }

        if (playerController.comboAllowed && playerController.input.IsAttack1KeyDown()) {
            comboAttach = true;
        }

        if (hitSomething) {
            // small cam Shake
            CameraFollow.GetInstance().ShakeSmall();
            hitSomething = false;
        }

        if (attackFinished || (!comboAttach && exitCombo)) {
            return new MoveState(playerController);
        }

        if (doJump) {
            if (checkJumpStarted == -1F) {
                if (playerController.moveController.IsGrounded()) {
                    playerController.moveController.OnJumpInputDown();
                    checkJumpStarted = Time.timeSinceLevelLoad + jumpTime;
                } else {
                    return new MoveState(playerController);
                }
            } else {
                if (checkJumpStarted < Time.timeSinceLevelLoad) {
                    playerController.moveController.OnJumpInputUp();
                }
            }

            if (checkMoveStarted > Time.timeSinceLevelLoad) {
                Move(playerController.dirX, playerController.input.GetDirectionY());
            } else {
                Move(0, playerController.input.GetDirectionY());
                //doJump = false;
            }
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
        if (parameter == "exit_combo") {
            exitCombo = true;
        }
        if (parameter == "jump") {
            doJump = true;
            checkMoveStarted = Time.timeSinceLevelLoad + moveTime1;
            playerController.InstantiateEffect(playerController.prefabEffectJump);
        }
        if (parameter == "move_forward2") {
            doJump = true;
            checkMoveStarted = Time.timeSinceLevelLoad + moveTime2;
        }
        if (parameter == "smash") {
            playerController.InstantiateEffect(playerController.prefabEffectLanding);
            CameraFollow.GetInstance().ShakeSmall();
        }
    }
}

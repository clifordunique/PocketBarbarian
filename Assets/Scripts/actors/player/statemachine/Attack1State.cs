using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1State : AbstractState {

    private bool attackFinished = false;
    private bool comboAttack = false;
    private bool exitCombo = false;
    private bool combo2 = false;
    private bool hitSomething = false;
    private float moveTime1 = 0.1F;
    private float moveTime2 = 0.25F;
    private float checkMoveStarted1;
    private float checkMoveStarted2;
    private bool effectPlayed = false;

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

        if (playerController.comboAllowed && playerController.input.IsAttack1KeyDown()) {
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
            if (checkMoveStarted1 > Time.timeSinceLevelLoad) {
               Move(playerController.dirX, playerController.input.GetDirectionY());
            } else {
                Move(0, playerController.input.GetDirectionY());
                checkMoveStarted1 = 0;
            }
        }
        if (comboAttack && combo2) {
            if (checkMoveStarted2 > Time.timeSinceLevelLoad) {
                Move(playerController.dirX, playerController.input.GetDirectionY());
            } else {
                Move(0, playerController.input.GetDirectionY());
                checkMoveStarted2 = 0;
                if (!effectPlayed) {
                    playerController.InstantiateEffect(playerController.prefabEffectGroundHitOneSided, playerController.dirX * 0.75F);
                    effectPlayed = true;
                }
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
            checkMoveStarted1 = Time.timeSinceLevelLoad + moveTime1;
        }
        if (parameter == "combo2") {
            combo2 = true;
            checkMoveStarted2 = Time.timeSinceLevelLoad + moveTime2;
            
        }
        if (parameter == EVENT_PARAM_HIT) {
            hitSomething = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1State : AbstractState {

    private bool attackFinished = false;

    private bool animCombo1 = false;
    private bool animCombo2 = false;

    private bool activateCombo1 = false;
    private bool activateCombo2 = false;

    private bool hitSomething = false;
    private float moveTime1 = 0.2F;
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

        // check if combo1 
        if (!animCombo1 && playerController.input.IsAttack1KeyDown() && playerController.statistics.comboLevel >= 1) {
            activateCombo1 = true;
        }
        // check if combo1 
        if (animCombo1 && !animCombo2 && playerController.input.IsAttack1KeyDown() && playerController.statistics.comboLevel >= 2) {
            activateCombo2 = true;
        }

        if (hitSomething) {
            // small cam Shake
            CameraFollow.GetInstance().ShakeSmall();
            hitSomething = false;
        }

        // combo1 move
        if (activateCombo1 && animCombo1 && !animCombo2) {
            if (checkMoveStarted1 > Time.timeSinceLevelLoad) {
               Move(playerController.dirX, playerController.input.GetDirectionY());
            } else {
                Move(0, playerController.input.GetDirectionY());
                checkMoveStarted1 = 0;
            }
        }

        // combo2 move
        if (activateCombo2 && animCombo2) {
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

        if ((animCombo1 && !activateCombo1) || (animCombo2 && !activateCombo2)) {
            attackFinished = true;
        }

        // End attack!
        if (attackFinished) {
            playerController.lastAttackTime = Time.timeSinceLevelLoad;
            return new IdleState(playerController);
        }
        return null;
    }

    public override void HandleEvent(string parameter) {
        if (parameter == EVENT_PARAM_ANIMATION_END) {
            attackFinished = true;
        }
        if (parameter == "combo1") {
            animCombo1 = true;
            checkMoveStarted1 = Time.timeSinceLevelLoad + moveTime1;
        }
        if (parameter == "combo2") {
            animCombo2 = true;
            checkMoveStarted2 = Time.timeSinceLevelLoad + moveTime2;
            
        }
        if (parameter == "shake") {
            CameraFollow.GetInstance().ShakeSmall();
        }
        if (parameter == EVENT_PARAM_HIT) {
            hitSomething = true;
        }
    }
}

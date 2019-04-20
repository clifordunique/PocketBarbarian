using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2State : AbstractState {

    private bool attackFinished = false;
    private bool animCombo1 = false;
    private bool animCombo2 = false;
    private bool hitSomething = false;
    private bool switchState = false;

    private bool activateCombo1 = false;
    private bool activateCombo2 = false;


    public Attack2State(PlayerController playerController) : base(ACTION.ATTACK2, playerController) {
    }

    public override void OnEnter() {
        playerController.animator.SetBool(ATTACK2_PARAM, true);
        Debug.Log("Enter MoveAttack");
    }

    public override void OnExit() {
        playerController.animator.SetBool(ATTACK2_PARAM, false);
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
        // check if combo2 
        if (animCombo1 && !animCombo2 && playerController.input.IsAttack1KeyDown() && playerController.statistics.comboLevel >= 2) {
            activateCombo2 = true;
        }

        if ((animCombo1 && !activateCombo1) || (animCombo2 && !activateCombo2)) {            
            attackFinished = true;
        }

        if (switchState) {
            if (playerController.input.GetDirectionX() == 0) {
                attackFinished = true;
            }
            switchState = false;
        }


        if (attackFinished) {
            playerController.lastAttackTime = Time.timeSinceLevelLoad;
            if (playerController.input.GetDirectionX() == 0) {
                return new IdleState(playerController);
            } else {
                return new MoveState(playerController);
            }                
        }        


        if (hitSomething) {
            // small cam Shake
            CameraFollow.GetInstance().ShakeSmall();
            hitSomething = false;
        }

        if (playerController.input.GetDirectionX() == 0) {
            return new IdleState(playerController);
        }

        Move(playerController.dirX, playerController.input.GetDirectionY());

        return null;
    }

    public override void HandleEvent(string parameter) {
        if (parameter == EVENT_PARAM_ANIMATION_END) {
            attackFinished = true;
        }
        if (parameter == EVENT_PARAM_HIT) {
            hitSomething = true;
        }
        if (parameter == "combo1") {
            switchState = true;
            animCombo1 = true;
        }
        if (parameter == "combo2") {
            switchState = true;
            animCombo2 = true;
        }
        if (parameter == "shake") {
            CameraFollow.GetInstance().ShakeSmall();
        }
    }
}

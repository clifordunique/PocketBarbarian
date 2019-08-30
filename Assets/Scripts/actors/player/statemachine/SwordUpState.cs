using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordUpState : AbstractState {

    private bool animationEnded = false;
    private bool animStarted = false;
    private bool startFlash = false;
    private bool flashExecuted = false;

    public SwordUpState(PlayerController playerController) : base(ACTION.SWORD_UP, playerController) {
    }

    public override void OnEnter() {
        if (playerController.moveController.IsGrounded()) {
            playerController.animator.SetBool(SWORD_UP_PARAM, true);
            
            animStarted = true;
        }
    }

    public override void OnExit() {
        playerController.animator.SetBool(SWORD_UP_PARAM, false);
    }

    public override AbstractState UpdateState() {
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }        

        if (!animStarted && playerController.moveController.IsGrounded()) {
            playerController.animator.SetBool(SWORD_UP_PARAM, true);
            animStarted = true;
        }

        if (startFlash && !flashExecuted) {
            playerController.FlashOutline();
            flashExecuted = true;
            startFlash = false;
        }

        if (animationEnded) {
            if (playerController.input.GetDirectionX() == 0) {
                return new IdleState(playerController);
            } else {
                return new MoveState(playerController);
            }
        }

        Move(0, playerController.input.GetDirectionY());
        return null;
    }


    public override void HandleEvent(string parameter) {
        if (animStarted && parameter == EVENT_PARAM_ANIMATION_END) {
            animationEnded = true;
        }

        if (animStarted && parameter == "start_flashing") {
            startFlash = true;
        }

    }


}

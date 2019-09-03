using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PommesgabelState: AbstractState {
    
    private bool animStarted = false;
    private bool animEnded = false;

    public PommesgabelState(PlayerController playerController) : base(ACTION.POMMESGABEL, playerController) {
    }

    public override void OnEnter() {
        if (playerController.moveController.IsGrounded()) {
            playerController.animator.SetBool(POMMESGABEL, true);
            animStarted = true;
        }
        Move(0, playerController.input.GetDirectionY());
    }

    public override void OnExit() {
        playerController.animator.SetBool(POMMESGABEL, false);
    }

    public override AbstractState UpdateState() {
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }     
        
        if (animStarted) {            
            playerController.animator.SetBool(POMMESGABEL, false);
        }

        if (!animStarted && playerController.moveController.IsGrounded()) {
            playerController.animator.SetBool(POMMESGABEL, true);
            animStarted = true;
        }



        if (animEnded) {
            if (playerController.input.GetDirectionX() == 0) {
                return new IdleState(playerController);
            } else {
                return new MoveState(playerController);
            }
        }

        
        return null;
    }

    public override void HandleEvent(string parameter) {
        if (parameter == EVENT_PARAM_ANIMATION_END) {
            animEnded = true;
        }
    }
}

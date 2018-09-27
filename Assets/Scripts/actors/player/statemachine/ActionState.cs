using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionState : AbstractState {


    private bool animationEnded = false;
    private bool actionTriggered = false;
    private Door interactable;

    public ActionState(PlayerController playerController) : base(ACTION.ACTION, playerController) {
    }

    public override void OnEnter() {
        playerController.animator.SetBool(ACTION_PARAM, true);
        if (playerController.interactable) {
            interactable = playerController.interactable.GetComponent<Door>();
        }
    }

    public override void OnExit() {
        playerController.animator.SetBool(ACTION_PARAM, false);
    }

    public override AbstractState UpdateState() {
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }

        if (playerController.moveController.IsFalling()) {
            return new FallingState(playerController);
        }

        if (animationEnded && playerController.interactable && !actionTriggered) {
            if (interactable) {
                // door detected!
                interactable.Activate();
                actionTriggered = true;
            }
        }

        if (actionTriggered && interactable.actionFinished) {
            return new WalkOutState(playerController);
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

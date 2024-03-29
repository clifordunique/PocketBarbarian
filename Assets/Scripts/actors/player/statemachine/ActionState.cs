﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionState : AbstractState {
    
    private bool animationEnded = false;
    private bool actionTriggered = false;
    private AbstactInteractable interactable;
    private bool levelEnded = false;

    public ActionState(PlayerController playerController) : base(ACTION.ACTION, playerController) {
    }

    public override void OnEnter() {
        playerController.animator.SetBool(ACTION_PARAM, true);
        if (playerController.interactable) {
            interactable = playerController.interactable.GetComponent<AbstactInteractable>();
        }
        if (interactable is Door || interactable is DoorLocked) {
            playerController.hurtBox.EnableCollider(false);
        }
    }

    public override void OnExit() {
        playerController.animator.SetBool(ACTION_PARAM, false);
        if (interactable is Door || interactable is DoorLocked) {
            playerController.hurtBox.EnableCollider(true);
        }
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
                // AbstactInteractable detected!
                interactable.Activate();
                actionTriggered = true;
            }
        }

        if (actionTriggered && interactable.actionFinished && !levelEnded) {
            if (interactable is Door || interactable is DoorLocked) {

                if (interactable is DoorExitLevel) {
                    // exit level
                    playerController.EndLevel();
                    levelEnded = true;
                } else {
                    // nur bei aufgeschlossener Tuer durchgehen!
                    return new WalkOutState(playerController);
                }
            } else {
                return new ActionOutState(playerController);
            }
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

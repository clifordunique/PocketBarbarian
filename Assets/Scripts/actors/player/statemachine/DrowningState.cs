using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrowningState : AbstractState {
    

    public DrowningState(PlayerController playerController) : base(ACTION.DEATH, playerController) {
    }

    public override void OnEnter() {
        playerController.animator.SetBool(DROWNING_PARAM, true);
        Move(0F, playerController.input.GetDirectionY());
    }

    public override void OnExit() {
        playerController.animator.SetBool(DROWNING_PARAM, false);
        Move(0F, playerController.input.GetDirectionY());
    }

    public override AbstractState UpdateState() {
        // can not be interrupted


        return null;
    }

    public override void HandleEvent(string parameter) {
        if (parameter == EVENT_PARAM_ANIMATION_END) {
            GuiController.GetInstance().InstanciateDiedEffect();
        }
    }

    }

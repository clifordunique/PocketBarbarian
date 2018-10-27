using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingState : AbstractState {
    

    public DyingState(PlayerController playerController) : base(ACTION.DEATH, playerController) {
    }

    public override void OnEnter() {
        playerController.animator.SetBool(DYING_PARAM, true);
        Move(0F, input.GetDirectionY());
    }

    public override void OnExit() {
        playerController.animator.SetBool(DYING_PARAM, false);
        Move(0F, input.GetDirectionY());
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

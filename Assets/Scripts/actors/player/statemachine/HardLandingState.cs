using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardLandingState : AbstractState {

    private bool animationEnded = false;

    public HardLandingState(PlayerController playerController) : base(ACTION.LANDING, playerController) {
    }

    public override void OnEnter() {
        CameraFollow.GetInstance().ShakeSmall();
        playerController.InstantiateEffect(playerController.prefabEffectHardLanding);
        playerController.animator.SetBool(HARD_LANDING_PARAM, true);
        SoundManager.PlaySFX(playerController.soundController.land_hard);
        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());
    }

    public override void OnExit() {
        playerController.animator.SetBool(HARD_LANDING_PARAM, false);
        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());
    }

    public override AbstractState UpdateState() {
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }

        if (animationEnded) {
            if (playerController.input.GetDirectionX() == 0) {
                return new IdleState(playerController);
            } else {
                return new MoveState(playerController);
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

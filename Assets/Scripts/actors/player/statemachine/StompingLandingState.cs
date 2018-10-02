using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompingLandingState : AbstractState {

    private bool animationEnded = false;
    private bool shake = false;

    public StompingLandingState(PlayerController playerController) : base(ACTION.LANDING, playerController) {
    }

    public override void OnEnter() {        
        playerController.InstantiateEffect(playerController.prefabEffectStompingSilhouette);
        
        playerController.animator.SetBool(STOMPING_LANDING_PARAM, true);
        Move(0, input.GetDirectionY());
    }

    public override void OnExit() {        
        playerController.animator.SetBool(STOMPING_LANDING_PARAM, false);
        Move(input.GetDirectionX(), input.GetDirectionY());

        // Switch HurtBox Layers back
        playerController.hurtBox.SwitchToOriginLayer();
    }

    public override AbstractState UpdateState() {
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }
        

        if (playerController.moveController.IsGrounded() && shake) {
            CameraFollow.GetInstance().ShakeStamp();
            playerController.InstantiateEffect(playerController.prefabEffectStompingGround);
            shake = false;
        }

        if (animationEnded) {
            if (input.GetDirectionX() == 0) {
                return new IdleState(playerController);
            } else {
                return new MoveState(playerController);
            }
        }

        return null;
    }


    public override void HandleEvent(string parameter) {
        if (parameter == EVENT_PARAM_ANIMATION_END) {
            animationEnded = true;
        }
        if (parameter == "shake") {
            shake = true;
        }
    }

}

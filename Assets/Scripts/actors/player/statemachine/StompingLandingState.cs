using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompingLandingState : AbstractState {

    private bool animationEnded = false;
    private bool lastFrameFalling = true;
    private bool hitSomething = false;

    private int waitFrame = 0;

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

        waitFrame++;

        if (hitSomething) {
            playerController.moveController.OnStamp();

            hitSomething = false;
        }

        if (waitFrame > 1) {



            if ((playerController.moveController.IsGrounded() && lastFrameFalling)) {
                
                lastFrameFalling = false;
                CameraFollow.GetInstance().ShakeStamp();
                playerController.InstantiateEffect(playerController.prefabEffectStompingGround);
            }
        
            //if (playerController.moveController.IsFalling() && lastFrameGrounded) {
            //    playerController.moveController.OnStamp();
            //}

            if (playerController.moveController.IsFalling()) {
                lastFrameFalling = true;
            } else {
                lastFrameFalling = false;
            }

            if (animationEnded && playerController.moveController.IsGrounded()) {
                if (input.GetDirectionX() == 0) {
                    return new IdleState(playerController);
                } else {
                    return new MoveState(playerController);
                }
            }
        }
        return null;
    }


    public override void HandleEvent(string parameter) {
        if (parameter == EVENT_PARAM_ANIMATION_END) {
            animationEnded = true;
        }
        if (parameter == EVENT_PARAM_HIT) {
            hitSomething = true;
        }
    }

}

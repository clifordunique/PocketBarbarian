using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingState : AbstractState {
    
    private float directionX;
    private float directionY;
    private float timeDash;
    private bool hitSomething = false;

    public DashingState(PlayerController playerController) : base(ACTION.DASH, playerController) {
    }

    public override void OnEnter() {
        playerController.hitBoxDash.gameObject.SetActive(true);
        playerController.animator.SetBool(DASHING_PARAM, true);
        //playerController.InstantiateEffect(playerController.prefabEffectDashing);
        directionX = input.GetDirectionX();
        directionY = input.GetDirectionY();
        Move(directionX, directionY, true);
        timeDash = Time.time;

        // Switch HurtBox Layers
        playerController.hurtBox.SwitchToDashLayer();

        // Reduce Stamina
        playerController.statistics.ReduceStaminaForDash();
    }

    public override void OnExit() {
        playerController.hitBoxDash.gameObject.SetActive(false);
        playerController.animator.SetBool(DASHING_PARAM, false);
        Move(input.GetDirectionX(), input.GetDirectionY());

        // Switch HurtBox Layers back
        playerController.hurtBox.SwitchToOriginLayer();
    }

    public override AbstractState UpdateState() {
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {            
            return interrupt;
        }

        if (playerController.moveController.IsFalling()) {
            return new FallingState(playerController);
        }

        if (hitSomething) {
            // hit something
            // Play effect
            playerController.InstantiateEffect(playerController.prefabEffectDashingHit);
            // small push back
            playerController.moveController.OnPush(-1 * directionX, playerController.moveController.dashPushbackForce, playerController.moveController.dashPushbackDuration);
            // small cam Shake
            CameraFollow.GetInstance().ShakeMedium();
            return new IdleState(playerController);
        }

        if (Time.time - timeDash >= playerController.moveController.dashDuration) {
            // end dashing
            return new IdleState(playerController);
        }

        if (Time.frameCount % 5 == 0) { 
            playerController.InstantiateEffect(playerController.prefabEffectDashingSilhouette);
            playerController.InstantiateEffect(playerController.prefabEffectStep);
        }

        Move(directionX, directionY, true);
        return null;
    }

    public override void HandleEvent(string parameter) {
        if (parameter == EVENT_PARAM_HIT) {
            hitSomething = true;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingState : AbstractState {

    private float directionX;
    private float directionY;
    private float timeDash;

    public DashingState(PlayerController playerController) : base(ACTION.DASH, playerController) {
    }

    public override void OnEnter() {
        playerController.hitBoxDash.SetActive(true);
        playerController.animator.SetBool(DASHING_PARAM, true);
        playerController.InstantiateEffect(playerController.prefabEffectDashing);
        directionX = input.GetDirectionX();
        directionY = input.GetDirectionY();
        Move(directionX, directionY, true);
        timeDash = Time.time;
    }

    public override void OnExit() {
        playerController.hitBoxDash.SetActive(false);
        playerController.animator.SetBool(DASHING_PARAM, false);
        Move(input.GetDirectionX(), input.GetDirectionY());
    }

    public override AbstractState UpdateState() {
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            // end dashing with interrupt
            // small push back
            playerController.moveController.OnPush(-1 * directionX, playerController.moveController.dashPushbackForce, playerController.moveController.dashPushbackDuration);
            return interrupt;
        }

        if (playerController.moveController.IsFalling()) {
            return new FallingState(playerController);
        }

        if (Time.time - timeDash >= playerController.moveController.dashDuration) {
            // end dashing
            return new IdleState(playerController);
        }
        
        Move(directionX, directionY, true);
        return null;
    }

}

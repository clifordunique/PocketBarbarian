using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : AbstractState {

    private float startTime;

    public HitState(PlayerController playerController) : base(ACTION.HIT, playerController) {
    }

    public override void OnEnter() {
        startTime = Time.time;

        playerController.animator.SetBool(JUMPING_PARAM, true);
        
        Vector3 hitDirection = Utils.GetHitDirection(playerController.lastHitSource, playerController.transform);
        playerController.moveController.OnPush(hitDirection.x, hitDirection.y);
    }

    public override void OnExit() {
        playerController.animator.SetBool(JUMPING_PARAM, false);
        Move(0F, input.GetDirectionY());
    }

    public override AbstractState UpdateState() {
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }

        if ((Time.time - startTime) > (playerController.hurtBox.hitTime / 2)) {
            if (playerController.hurtBox.currentHealth <= 0) {
                return new DyingState(playerController);
            } else {
                return new IdleState(playerController);
            }
        }
        Move(input.GetDirectionX(), input.GetDirectionY());
        return null;
    }


}

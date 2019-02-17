using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : AbstractState {

    private float startTime;

    public HitState(PlayerController playerController) : base(ACTION.HIT, playerController) {
    }

    public override void OnEnter() {
        startTime = Time.time;
        if (playerController.lastHit.push && !playerController.lastHit.instakill) {
            playerController.animator.SetBool(JUMPING_PARAM, true);
            Vector3 hitDirection = Utils.GetHitDirection(playerController.lastHit.hitSource, playerController.transform);
            playerController.moveController.OnPush(hitDirection.x, hitDirection.y);
        }
    }

    public override void OnExit() {
        if (playerController.lastHit.push && !playerController.lastHit.instakill) {
            playerController.animator.SetBool(JUMPING_PARAM, false);
        }
        Move(0F, playerController.input.GetDirectionY());
    }

    public override AbstractState UpdateState() {
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }

        if (playerController.lastHit.push && !playerController.lastHit.instakill) {
            if ((Time.time - startTime) > (playerController.hurtBox.hitTime / 2)) {
                if (playerController.statistics.health <= 0) {
                    return new DyingState(playerController.lastHit.damageType, playerController);
                } else {
                    return new IdleState(playerController);
                }
            }
        } else {
            if (playerController.statistics.health <= 0) {
                return new DyingState(playerController.lastHit.damageType, playerController);
            } else {
                return new IdleState(playerController);
            }
        }
        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());
        return null;
    }


}

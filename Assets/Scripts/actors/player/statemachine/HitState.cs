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
        // can not be interrupted

        if ((Time.time - startTime) > playerController.hurtBox.hitTime) {            
            return new IdleState(playerController);
        }
        return null;
    }


}

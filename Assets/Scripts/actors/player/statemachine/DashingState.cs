using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingState : AbstractState {

    public DashingState(PlayerController playerController) : base(ACTION.DASH, playerController) {
    }

    public override void OnEnter() {
        playerController.animator.SetBool(DASHING_PARAM, true);
        //playerController.InstantiateEffect(playerController.prefabEffectJump);        
        playerController.moveController.OnDash();
    }

    public override void OnExit() {
        playerController.animator.SetBool(DASHING_PARAM, false);
        Move(input.GetDirectionX(), input.GetDirectionY());
    }

    public override AbstractState UpdateState() {
        base.UpdateState();

        if (playerController.moveController.IsFalling()) {
            return new FallingState(playerController);
        }

        if (!playerController.moveController.IsDashing()) {
            return new IdleState(playerController);
        }
 
        return null;
    }


}

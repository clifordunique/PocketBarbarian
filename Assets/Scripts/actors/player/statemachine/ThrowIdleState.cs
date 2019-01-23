using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowIdleState: AbstractState {

    private bool throwFinished = false;

    public ThrowIdleState(PlayerController playerController) : base(ACTION.SHOOT, playerController) {
    }

    public override void OnEnter() {
        playerController.animator.SetBool(THROW_IDLE_PARAM, true);        
    }

    public override void OnExit() {
        playerController.animator.SetBool(THROW_IDLE_PARAM, false);
    }

    public override AbstractState UpdateState() {
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }

        if (playerController.moveController.IsFalling()) {
            return new FallingState(playerController);
        }
        
        if (throwFinished) {
            return new IdleState(playerController);
        }
        

        Move(0, playerController.input.GetDirectionY());
        return null;
    }

    public override void HandleEvent(string parameter) {
        if (parameter == EVENT_PARAM_ANIMATION_END) {
            throwFinished = true;
        }
        if (parameter == EVENT_PARAM_THROW) {
            playerController.ShootProjectile();
            // ammo benutzen playerController.statistics.ModifyAmmo(-1);
        }
    }
}

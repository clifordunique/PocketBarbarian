using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowRunningState: AbstractState {

    private bool animationFinished = false;
    private bool throwFinished = false;

    public ThrowRunningState(PlayerController playerController) : base(ACTION.SHOOT, playerController) {
    }

    public override void OnEnter() {
        playerController.animator.SetBool(THROW_RUNNING_PARAM, true);
        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());
    }

    public override void OnExit() {
        playerController.animator.SetBool(THROW_RUNNING_PARAM, false);
        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());
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
            if (playerController.input.GetDirectionX() == 0) {
                return new IdleState(playerController);
            }
        }

        if (animationFinished) {
            if (playerController.input.GetDirectionX() == 0) {
                return new IdleState(playerController);
            } else {
                return new MoveState(playerController);
            }
        }
        

        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());
        return null;
    }

    public override void HandleEvent(string parameter) {
        if (parameter == EVENT_PARAM_ANIMATION_END) {
            animationFinished = true;
        }
        if (parameter == EVENT_PARAM_THROW) {
            playerController.ShootProjectile();
            playerController.statistics.ModifyAmmo(-1);
            throwFinished = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompingState : AbstractState {

    bool wallSlidingTmp;

    public StompingState(PlayerController playerController) : base(ACTION.JUMP, playerController) {
    }

    public override void OnEnter() {
        // wall sliding while stomping not allowed
        wallSlidingTmp = playerController.moveController.wallJumpingAllowed;
        playerController.moveController.wallJumpingAllowed = false;

        // Switch HurtBox Layers
        playerController.hurtBox.SwitchToDashLayer();

        playerController.animator.SetBool(STOMPING_PARAM, true);
        playerController.moveController.OnStamp();

        // Reduce Stamina
        playerController.statistics.ReduceStaminaForAction();
    }

    public override void OnExit() {
        playerController.moveController.wallJumpingAllowed = wallSlidingTmp;
        playerController.animator.SetBool(STOMPING_PARAM, false);
        Move(input.GetDirectionX(), input.GetDirectionY());
    }

    public override AbstractState UpdateState() {
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }

        if (playerController.moveController.IsGrounded()) {
            return new StompingLandingState(playerController);
        }

        if (Time.frameCount % 3 == 0) {
            playerController.InstantiateEffect(playerController.prefabEffectStompingSilhouette);
        }

        Move(0, input.GetDirectionY());
        return null;
    }


}

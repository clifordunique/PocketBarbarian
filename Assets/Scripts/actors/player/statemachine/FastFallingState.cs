﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastFallingState : AbstractState {
    
    public FastFallingState(PlayerController playerController) : base(ACTION.JUMP, playerController) {
    }

    public override void OnEnter() {
        playerController.animator.SetBool(FAST_FALLING_PARAM, true);
        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());
    }

    public override void OnExit() {
        playerController.animator.SetBool(FAST_FALLING_PARAM, false);
        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());
    }

    public override AbstractState UpdateState() {
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }

        if (Time.frameCount % 3 == 0) {
            playerController.InstantiateEffect(playerController.prefabEffectFallingSilhouette);
        }

        if (playerController.moveController.IsWallSliding()) {
            return new WallSlidingState(playerController);
        }

        if (playerController.moveController.IsGrounded()) {
            return new HardLandingState(playerController);
        }

        if (playerController.moveController.stampingAllowed && playerController.moveController.IsFalling() && playerController.input.DownKeyDown()) {
            // check if Stomping possible
            if (playerController.statistics.HasEnoughStaminaForAction()) {
                return new StompingState(playerController);
            }
        } 

        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());
        return null;
    }


}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttackState : AbstractState {

    private bool attackFinished = false;
    private bool hitSomething = false;

    public JumpAttackState(PlayerController playerController) : base(ACTION.JUMP, playerController) {
    }

    public override void OnEnter() {
        playerController.animator.SetBool(JUMPING_ATTACK_PARAM, true);
        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());
    }

    public override void OnExit() {
        playerController.animator.SetBool(JUMPING_ATTACK_PARAM, false);
        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());
    }

    public override AbstractState UpdateState() {
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }
        

        if (attackFinished || playerController.moveController.IsGrounded()) {
            playerController.lastAttackTime = Time.timeSinceLevelLoad;
            if (playerController.moveController.IsGrounded()) {
                return new LandingState(playerController);
            } else {
                return new FallingState(playerController);
            }
        }


        if (!playerController.moveController.IsFalling() && playerController.input.IsJumpKeyUp()) {
            playerController.moveController.OnJumpInputUp();
        }

        if (hitSomething) {
            // small cam Shake
            CameraFollow.GetInstance().ShakeSmall();
            hitSomething = false;
        }

        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());
        return null;
    }

    public override void HandleEvent(string parameter) {
        if (parameter == EVENT_PARAM_ANIMATION_END) {
            attackFinished = true;
        }
        if (parameter == EVENT_PARAM_HIT) {
            hitSomething = true;
        }
    }
}

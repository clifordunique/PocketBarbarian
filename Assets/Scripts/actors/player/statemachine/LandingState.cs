using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingState : AbstractState {

    private bool animationEnded = false;

    public LandingState(PlayerController playerController) : base(ACTION.LANDING, playerController) {
    }

    public override void OnEnter() {
        playerController.InstantiateEffect(playerController.prefabEffectLanding);
        playerController.animator.SetBool(LANDING_PARAM, true);
        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());
    }

    public override void OnExit() {
        playerController.animator.SetBool(LANDING_PARAM, false);
        Move(playerController.input.GetDirectionX(), playerController.input.GetDirectionY());
    }

    public override AbstractState UpdateState() {
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }

        if (playerController.hasWeapon && playerController.input.IsAttack1KeyPressed() && playerController.AttackAvailable()) {
            return new Attack1State(playerController);
        }

        if (playerController.hasWeapon && playerController.input.IsAttack2KeyPressed()) { // hat ammo && playerController.statistics.ammo > 0) {
            return new ThrowIdleState(playerController);
        }

        if (playerController.input.IsJumpKeyDown()) {
            return new JumpState(playerController);
        }

        if (animationEnded) {
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
            animationEnded = true;
        }
    }

}

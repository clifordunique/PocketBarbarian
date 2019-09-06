using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : AbstractState {

    public IdleState(PlayerController playerController) : base(ACTION.IDLE, playerController) {
    }

    public override void OnEnter() {
        playerController.animator.SetBool(IDLE_PARAM, true);
    }

    public override void OnExit() {
        playerController.animator.SetBool(IDLE_PARAM, false);
    }

    public override AbstractState UpdateState() {
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }

        if (playerController.moveController.IsFalling()) {
            return new FallingState(playerController);
        }

        if (playerController.input.IsJumpKeyDown()) {
            return new JumpState(playerController);
        }
        if (playerController.input.GetDirectionX() != 0) {
            return new MoveState(playerController);
        }

        if (playerController.input.GetDirectionY() == 1 && playerController.interactableInRange) {
            // Check if interactable activatable
            AbstactInteractable interactable = playerController.interactable.GetComponent<AbstactInteractable>();
            if (interactable && !interactable.permanentDisabled) {
                return new ActionState(playerController);
            }
        }

        if (playerController.DoAttack()) {
            return new AttackLightState(playerController);
        }

        if (playerController.DoThrow()) {
            return new ThrowIdleState(playerController);
        }

        if (playerController.input.IsWarcry()) {
            return new WarcryStat(playerController);
        }
        Move(0, playerController.input.GetDirectionY());
        return null;
    }


}

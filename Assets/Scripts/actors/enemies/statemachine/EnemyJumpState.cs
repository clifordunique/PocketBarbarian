using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumpState : AbstractEnemyState {
    
    public EnemyJumpState(EnemyStateMachine stateMachine) : base(stateMachine) {
    }

    public override void OnEnter() {
        stateMachine.moveController.OnJumpInputDown();
    }

    public override AbstractEnemyState UpdateState() {

        if (stateMachine.currentAction.actionEvent != EnemyAction.ACTION_EVENT.JUMP) {
            // Interrupt current Action
            return GetEnemyState(stateMachine.currentAction.actionEvent);
        }

        if (stateMachine.currentAction.HasMoveTarget()) {
            moveController.MoveTo(stateMachine.currentAction.moveTarget);
        }

        if (stateMachine.moveController.IsGrounded()) {
            Debug.Log("Jump finished. Next action");
            // stop if we moved while jumping
            moveController.StopMoving();
            stateMachine.RequestNextAction();
            return GetEnemyState(stateMachine.currentAction.actionEvent);
        }
        return null;
    }

    public override void OnExit() {
        // nothing to do
    }

}

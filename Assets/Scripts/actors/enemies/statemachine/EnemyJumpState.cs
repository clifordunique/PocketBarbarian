using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumpState : AbstractEnemyState {
    
    public EnemyJumpState(EnemyController enemyController) : base(enemyController) {
    }

    public override void OnEnter() {
        enemyController.moveController.OnJumpInputDown();
    }

    public override AbstractEnemyState UpdateState() {

        if (enemyController.currentAction.actionEvent != EnemyAction.ACTION_EVENT.JUMP) {
            // Interrupt current Action
            return GetEnemyState(enemyController.currentAction.actionEvent);
        }

        if (enemyController.currentAction.HasMoveTarget()) {
            moveController.MoveTo(enemyController.currentAction.moveTarget);
        }

        if (enemyController.moveController.IsGrounded()) {
            // stop if we moved while jumping
            moveController.StopMoving();
            enemyController.RequestNextAction();
            return GetEnemyState(enemyController.currentAction.actionEvent);
        }
        return null;
    }

    public override void OnExit() {
        // nothing to do
    }

}

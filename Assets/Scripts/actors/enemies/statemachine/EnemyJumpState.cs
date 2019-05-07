using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumpState : AbstractEnemyState {

    private bool jumped = false;

    public EnemyJumpState(EnemyController enemyController) : base(enemyController) {
    }

    public override void OnEnter() {
        if (enemyController.animator) {
            enemyController.animator.SetBool("JUMP", true);
        }
    }

    public override AbstractEnemyState UpdateState() {
        if (enemyController.currentAction.actionEvent != EnemyAction.ACTION_EVENT.JUMP) {
            // Interrupt current Action
            return GetEnemyState(enemyController.currentAction.actionEvent);
        }

        if (enemyController.currentAction.HasMoveTarget()) {
            float directionX = moveController.MoveTo(enemyController.currentAction.moveTarget);
            if (directionX == 0) { // angekommen              
                moveController.StopMoving();                
            }
        }

        if (jumped && enemyController.moveController.IsGrounded()) {
            // stop if we moved while jumping
            moveController.StopMoving();
            //enemyController.RequestNextAction();
            //return GetEnemyState(enemyController.currentAction.actionEvent);
            return new EnemyLandingState(enemyController);
        }
        return null;
    }

    public override void OnExit() {
        if (enemyController.animator) {
            enemyController.animator.SetBool("JUMP", false);
        }
    }

    public override void HandleAnimEvent(string parameter) {
        if (parameter == "JUMP" && !jumped) {
            jumped = true;
            enemyController.moveController.OnJumpInputDown();
        }
    }


}

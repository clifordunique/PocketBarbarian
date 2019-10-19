using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLandingState : AbstractEnemyState {

    private bool finished = false;

    public EnemyLandingState(EnemyController enemyController) : base(enemyController) {
    }

    public override void OnEnter() {
        moveController.StopMoving();
        if (enemyController.animator) {
            enemyController.animator.SetBool("LANDING", true);
        }
    }

    public override AbstractEnemyState UpdateState() {

        if (enemyController.currentAction.actionEvent == EnemyAction.ACTION_EVENT.HIT) {
            // Interrupt current Action
            return GetEnemyState(enemyController.currentAction.actionEvent);
        }
        
        if (finished) {
            if (enemyController.currentAction.HasMoveTarget()) {

                float directionX = moveController.MoveTo(enemyController.currentAction.moveTarget);
                if (directionX == 0) { // angekommen              
                    moveController.StopMoving();
                    enemyController.RequestNextAction();
                    return GetEnemyState(enemyController.currentAction.actionEvent);
                }
            }
            enemyController.RequestNextAction();
            return GetEnemyState(enemyController.currentAction.actionEvent);
        }
        return null;
    }

    public override void OnExit() {
        if (enemyController.animator) {
            enemyController.animator.SetBool("LANDING", false);
        }
    }

    public override void HandleAnimEvent(string parameter) {
        if (parameter == "ANIM_END") {
            finished = true;
        }
    }
}

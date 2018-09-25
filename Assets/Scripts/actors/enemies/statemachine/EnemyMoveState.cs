using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : AbstractEnemyState {
    

    public EnemyMoveState(EnemyController enemyController) : base(enemyController) {
    }

    public override void OnEnter() {
    }

    public override AbstractEnemyState UpdateState() {

        if (enemyController.currentAction.actionEvent != EnemyAction.ACTION_EVENT.MOVE) {
            // Interrupt current Action
            return GetEnemyState(enemyController.currentAction.actionEvent);
        }

        if (enemyController.currentAction.HasMoveTarget()) {

            float directionX = moveController.MoveTo(enemyController.currentAction.moveTarget);
            if (directionX == 0) { // angekommen              
                moveController.StopMoving();
                enemyController.RequestNextAction();
                return GetEnemyState(enemyController.currentAction.actionEvent);
            }
        } else {
            // kein MoveTarget, also nächste Action holen
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

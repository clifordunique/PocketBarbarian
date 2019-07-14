using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : AbstractEnemyState {

    float lastDirX = 1;

    public EnemyMoveState(EnemyController enemyController) : base(enemyController) {
    }

    public override void OnEnter() {
        if (enemyController.animator) {
            enemyController.animator.SetBool("MOVE", true);
        }
        lastDirX = enemyController.transform.localScale.x;
    }

    public override AbstractEnemyState UpdateState() {

        if (enemyController.currentAction.actionEvent != EnemyAction.ACTION_EVENT.MOVE) {
            // Interrupt current Action
            return GetEnemyState(enemyController.currentAction.actionEvent);
        }

        float directionX = lastDirX;
        if (enemyController.currentAction.HasMoveTarget()) {

            directionX = moveController.MoveTo(enemyController.currentAction.moveTarget);
            if (directionX == 0) { // angekommen              
                moveController.StopMoving();
                enemyController.RequestNextAction();
                return GetEnemyState(enemyController.currentAction.actionEvent);
            }
        } else {
            // kein MoveTarget, also einfach immer in die gleiche Ritchung laufen
            if (moveController is EnemyMoveGroundController2D) {
                if (lastDirX >= 0 && ((EnemyMoveGroundController2D)moveController).collisions.right) {
                    // rechts ende, Richtung aendern
                    lastDirX = -1;
                }
                if (lastDirX < 0 && ((EnemyMoveGroundController2D)moveController).collisions.left) {
                    // links ende, Richtung aendern
                    lastDirX = 1;
                }
            }
            directionX = moveController.MoveTo(new Vector3(enemyController.transform.position.x + lastDirX, enemyController.transform.position.y, enemyController.transform.position.z));
            
        }

        enemyController.SetDirection(directionX);
        return null;
    }


    public override void OnExit() {
        if (enemyController.animator) {
            enemyController.animator.SetBool("MOVE", false);
        }
    }

}

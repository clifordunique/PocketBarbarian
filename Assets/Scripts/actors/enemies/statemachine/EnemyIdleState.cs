using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : AbstractEnemyState {

    private float startTime;

    public EnemyIdleState(EnemyController enemyController) : base(enemyController) {
    }

    public override void OnEnter() {
        enemyController.moveController.StopMoving();
        startTime = Time.timeSinceLevelLoad;
        if (enemyController.animator) {
            enemyController.animator.SetBool("IDLE", true);
        }
    }

    public override AbstractEnemyState UpdateState() {
        if (enemyController == null) { Debug.Log("EnemyController null"); }
        if (enemyController.currentAction == null) { Debug.Log("EnemyController currentAction null"); }
        if (enemyController.currentAction.actionEvent != EnemyAction.ACTION_EVENT.IDLE) {
            // Interrupt current Action
            return GetEnemyState(enemyController.currentAction.actionEvent);
        }

        if ((Time.timeSinceLevelLoad - startTime) > enemyController.currentAction.amount) {
            enemyController.RequestNextAction();
            if (enemyController.currentAction.actionEvent != EnemyAction.ACTION_EVENT.IDLE) {
                // only change when not idle
                return GetEnemyState(enemyController.currentAction.actionEvent);
            }
        }
        return null;
    }

    public override void OnExit() {
        if (enemyController.animator) {
            enemyController.animator.SetBool("IDLE", false);
        }
    }

}

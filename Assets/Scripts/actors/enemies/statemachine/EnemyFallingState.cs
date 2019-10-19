using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFallingState : AbstractEnemyState {
    

    public EnemyFallingState(EnemyController enemyController) : base(enemyController) {
    }

    public override void OnEnter() {
        if (enemyController.animator) {
            enemyController.animator.SetBool("FALLING", true);
        }
        moveController.StopMoving();
    }

    public override AbstractEnemyState UpdateState() {
        if (enemyController.currentAction.actionEvent != EnemyAction.ACTION_EVENT.FALLING) {
            // Interrupt current Action
            return GetEnemyState(enemyController.currentAction.actionEvent);
        }        

        if (enemyController.moveController.IsGrounded()) {
            return new EnemyLandingState(enemyController);
        }
        return null;
    }

    public override void OnExit() {
        if (enemyController.animator) {
            enemyController.animator.SetBool("FALLING", false);
        }
    }


}

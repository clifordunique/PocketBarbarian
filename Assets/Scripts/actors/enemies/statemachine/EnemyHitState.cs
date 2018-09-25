using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitState : AbstractEnemyState {

    private float startTime;

    public EnemyHitState(EnemyController enemyController) : base(enemyController) {
    }

    public override void OnEnter() {
        startTime = Time.time;

        if (enemyController.animator) {
            enemyController.animator.SetBool("HIT", true);
        }

        if (enemyController.currentAction.HasHitTarget()) {
            Vector3 hitDirection = Utils.GetHitDirection(enemyController.currentAction.hitTarget, enemyController.transform);
            moveController.OnPush(hitDirection.x, hitDirection.y);
        }
        
    }

    public override AbstractEnemyState UpdateState() {

        // can not be interrupted

        if ((Time.time - startTime) > moveController.GetPushDuration()) {
            moveController.StopMoving();
            enemyController.RequestNextAction();
            return GetEnemyState(enemyController.currentAction.actionEvent);
        }
        return null;
    }

    public override void OnExit() {
        if (enemyController.animator) {
            enemyController.animator.SetBool("HIT", false);
        }
    }



}

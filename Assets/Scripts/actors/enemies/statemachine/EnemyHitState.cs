using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitState : AbstractEnemyState {

    private float startTime;

    public EnemyHitState(EnemyController enemyController) : base(enemyController) {
    }

    public override void OnEnter() {
        startTime = Time.timeSinceLevelLoad;

        if (enemyController.animator) {
            enemyController.animator.SetBool("HIT", true);
        }

        if (enemyController.currentAction.HasHitTarget()) {
            Vector3 hitDirection = Utils.GetHitDirection(enemyController.currentAction.hitTarget, enemyController.transform);
            if (enemyController.lastDamageType == HitBox.DAMAGE_TYPE.DASH) {
                moveController.OnPush((hitDirection.x == 0F ? -1F : hitDirection.x), hitDirection.y, true);
            } else {
                moveController.OnPush((hitDirection.x == 0F ? -1F : hitDirection.x), hitDirection.y, false);
            }
        }
        
    }

    public override AbstractEnemyState UpdateState() {

        if (enemyController.currentAction.actionEvent != EnemyAction.ACTION_EVENT.HIT) {
            // Interrupt current Action
            return GetEnemyState(enemyController.currentAction.actionEvent);
        }

        if ((Time.timeSinceLevelLoad - startTime) > enemyController.hurtBox.hitTime) { 
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

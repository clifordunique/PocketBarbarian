using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitState : AbstractEnemyState {

    private float startTime;

    public EnemyHitState(EnemyController enemyController) : base(enemyController) {
    }

    public override void OnEnter() {
        Debug.Log("Enter Enemy Hit!");
        startTime = Time.time;

        if (enemyController.animator) {
            enemyController.animator.SetBool("HIT", true);
        }

        if (enemyController.currentAction.HasHitTarget()) {
            Vector3 hitDirection = Utils.GetHitDirection(enemyController.currentAction.hitTarget, enemyController.transform);
            if (enemyController.lastDamageType == HitBox.DAMAGE_TYPE.DASH) {
                Debug.Log("DASH HIT");
                moveController.OnPush(hitDirection.x, hitDirection.y, true);
            } else {
                Debug.Log("NORMAL HIT");
                moveController.OnPush(hitDirection.x, hitDirection.y, false);
            }
        }
        
    }

    public override AbstractEnemyState UpdateState() {

        if (enemyController.currentAction.actionEvent != EnemyAction.ACTION_EVENT.HIT) {
            // Interrupt current Action
            return GetEnemyState(enemyController.currentAction.actionEvent);
        }

        if ((Time.time - startTime) > enemyController.hurtBox.hitTime) { 
            moveController.StopMoving();
            enemyController.RequestNextAction();
            return GetEnemyState(enemyController.currentAction.actionEvent);
        }
        return null;
    }

    public override void OnExit() {
        Debug.Log("Exit Enemy Hit!");
        if (enemyController.animator) {
            enemyController.animator.SetBool("HIT", false);
        }
    }



}

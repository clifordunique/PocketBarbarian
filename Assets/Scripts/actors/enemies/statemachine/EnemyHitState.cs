﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitState : AbstractEnemyState {

    private float startTime;
    private EnemyAction.ACTION_EVENT nextAction;

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

        enemyController.hitBox.boxCollider.enabled = false;

        enemyController.RequestNextAction();
        nextAction = enemyController.currentAction.actionEvent;        
    }

    public override AbstractEnemyState UpdateState() {

        if (enemyController.currentAction.actionEvent != EnemyAction.ACTION_EVENT.HIT && enemyController.isInterruptAction) {
            // Interrupt current Action
            return GetEnemyState(enemyController.currentAction.actionEvent);
        } else {
            if (enemyController.isInterruptAction) {
                // erneuter Hit!
                return new EnemyHitState(enemyController);
            }
        }

        if ((Time.timeSinceLevelLoad - startTime) > enemyController.hurtBox.flashTime) {
            moveController.StopMoving();
            return GetEnemyState(nextAction);
        }
        return null;
    }

    public override void OnExit() {
        if (enemyController.animator) {
            enemyController.animator.SetBool("HIT", false);
        }

        if (enemyController.hurtBox.currentHealth > 0) {
            enemyController.hitBox.boxCollider.enabled = true;
        }
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootState : AbstractEnemyState {

    private bool finished = false;
    private bool shootNow = false;
    private int shootCounter = 1;

    public EnemyShootState(EnemyController enemyController) : base(enemyController) {
    }

    public override void OnEnter() {
        moveController.StopMoving();
        if (enemyController.animator) {
            enemyController.animator.SetBool("SHOOT", true);
        } 
    }

    public override AbstractEnemyState UpdateState() {

        if (enemyController.currentAction.actionEvent != EnemyAction.ACTION_EVENT.SHOOT) {
            // Interrupt current Action
            return GetEnemyState(enemyController.currentAction.actionEvent);
        }

        if (enemyController.isShooter && shootNow) {

            if (shootCounter <= enemyController.currentAction.amount) {
                enemyController.ShootProjectile(enemyController.currentAction.hitTarget, enemyController.currentAction.hitTargetIsVector);
                shootNow = false;
                shootCounter++;
            } 
            
        }
        if (finished) {
            if (shootCounter > enemyController.currentAction.amount) {
                enemyController.RequestNextAction();
                return GetEnemyState(enemyController.currentAction.actionEvent);
            } else {
                // next shoot iteration
                finished = false;
            }
        }

        return null;
    }

    public override void HandleAnimEvent(string parameter) {
        if (parameter == "SHOOT") {
            shootNow = true;
        }
        if (parameter == "ANIM_END") {
            finished = true;
        }
    }

    public override void OnExit() {
        Debug.Log("Shoot State Exit");
        if (enemyController.animator) {
            enemyController.animator.SetBool("SHOOT", false);
        }
    }

}

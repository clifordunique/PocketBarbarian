using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDizzyState: AbstractEnemyState {

    private float startTime;
    private float waitForSeconds;
    private EnemyAction.ACTION_EVENT nextAction;

    public EnemyDizzyState(EnemyController enemyController) : base(enemyController) {
    }

    public override void OnEnter() {

        startTime = Time.timeSinceLevelLoad;

        if (enemyController.animator) {
            enemyController.animator.SetBool("DIZZY", true);
        }
        moveController.StopMoving();
        enemyController.dizzyEffect.SetActive(true);

        waitForSeconds = enemyController.currentAction.amount;
   
    }

    public override AbstractEnemyState UpdateState() {
        Debug.Log("In Dizzy!");
        if (enemyController.currentAction.actionEvent != EnemyAction.ACTION_EVENT.DIZZY && enemyController.isInterruptAction) {
            // Interrupt current Action
            return GetEnemyState(enemyController.currentAction.actionEvent);
        } 

        if ((startTime + waitForSeconds) < Time.timeSinceLevelLoad) {
            moveController.StopMoving();
            enemyController.RequestNextAction();
            return GetEnemyState(enemyController.currentAction.actionEvent);
        }
        return null;
    }

    public override void OnExit() {
        if (enemyController.animator) {
            enemyController.animator.SetBool("DIZZY", false);
        }
        enemyController.dizzyEffect.SetActive(false);
    }



}

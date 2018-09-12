using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitState : AbstractEnemyState {

    private float startTime;

    public EnemyHitState(EnemyStateMachine stateMachine) : base(stateMachine) {
    }

    public override void OnEnter() {
        startTime = Time.time;

        if (stateMachine.currentAction.HasHitTarget()) {
            Vector3 hitDirection = Utils.GetHitDirection(stateMachine.currentAction.hitTarget, stateMachine.transform);
            moveController.OnPush(hitDirection.x, hitDirection.y);
        }
        
    }

    public override AbstractEnemyState UpdateState() {

        // can not be interrupted

        if ((Time.time - startTime) > stateMachine.currentAction.amount) {
            moveController.StopMoving();
            stateMachine.RequestNextAction();
            return GetEnemyState(stateMachine.currentAction.actionEvent);
        }
        return null;
    }

    public override void OnExit() {
        // nothing to do
    }



}

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
            Vector3 hitDirection = GetHitDirection(stateMachine.currentAction.hitTarget);
            moveController.OnPush(hitDirection.x, hitDirection.y, 12, stateMachine.currentAction.duration);
        }
        
    }

    public override AbstractEnemyState UpdateState() {

        // can not be interrupted

        if ((Time.time - startTime) > stateMachine.currentAction.duration) {
            moveController.StopMoving();
            Debug.Log("Hit timed out. Next action");
            stateMachine.RequestNextAction();
            return GetEnemyState(stateMachine.currentAction.actionEvent);
        }
        return null;
    }

    public override void OnExit() {
        // nothing to do
    }


    public Vector3 GetHitDirection(Vector3 attacker) {
        Vector3 v = new Vector3(stateMachine.transform.position.x - attacker.x, stateMachine.transform.position.y - attacker.y, 1).normalized;
        Vector3 result = new Vector3();
        if (v.x > 0F) result.x = 1;
        if (v.x < -0F) result.x = -1;

        if (v.y > 0.5F) result.y = 1;
        if (v.y < -0.5F) result.y = -1;
        result.y = 0;
        return result;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootState : AbstractEnemyState {

    private float startTime;

    public EnemyShootState(EnemyStateMachine stateMachine) : base(stateMachine) {
    }

    public override void OnEnter() {
        startTime = Time.time;
        if (stateMachine.isShooter) {
            stateMachine.ShootProjectile(stateMachine.currentAction.hitTarget);
        }
    }

    public override AbstractEnemyState UpdateState() {

        if (stateMachine.currentAction.actionEvent != EnemyAction.ACTION_EVENT.SHOOT) {
            // Interrupt current Action
            return GetEnemyState(stateMachine.currentAction.actionEvent);
        }
        float directionX = -1000;
        if (stateMachine.moveWhileShooting && stateMachine.currentAction.HasMoveTarget()) {
            directionX = moveController.MoveTo(stateMachine.currentAction.moveTarget);
        }

        if (directionX == 0 || startTime + stateMachine.currentAction.duration < Time.time) {
            // stop if we moved while shooting
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

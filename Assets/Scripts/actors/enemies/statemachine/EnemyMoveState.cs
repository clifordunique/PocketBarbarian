using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : AbstractEnemyState {
    

    public EnemyMoveState(EnemyStateMachine stateMachine) : base(stateMachine) {
        Debug.Log("Move Constructor");
    }

    public override void OnEnter() {
    }

    public override AbstractEnemyState UpdateState() {

        if (stateMachine.currentAction.actionEvent != EnemyAction.ACTION_EVENT.MOVE) {
            // Interrupt current Action
            return GetEnemyState(stateMachine.currentAction.actionEvent);
        }

        if (stateMachine.currentAction.HasMoveTarget()) {

            float directionX = moveController.MoveTo(stateMachine.currentAction.moveTarget);
            if (directionX == 0) { // angekommen                
                Debug.Log("move timed out. Next action");
                stateMachine.RequestNextAction();
                return GetEnemyState(stateMachine.currentAction.actionEvent);
            }
        } else {
            // kein MoveTarget, also nächste Action holen
            stateMachine.RequestNextAction();
            return GetEnemyState(stateMachine.currentAction.actionEvent);
        }
        return null;
    }


    public override void OnExit() {
        // nothing to do
    }

}

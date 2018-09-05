using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : AbstractEnemyState {

    private EnemyMoveGroundController2D moveController;

    public EnemyMoveState(EnemyStateMachine stateMachine) : base(stateMachine) {
        Debug.Log("Move Constructor");
    }

    public override void OnEnter() {
        if (stateMachine.moveController is EnemyMoveGroundController2D) {
            moveController = (EnemyMoveGroundController2D)stateMachine.moveController;
        } else {
            Debug.Log("MovingState but no MoveController found!");
        }
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

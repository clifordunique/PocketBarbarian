using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : AbstractState {

    private EnemyMoveGroundController2D moveController;

    public EnemyMoveState(ACTION currentAction, StateMachineGameActor playerController, Vector3 target) : base(currentAction, playerController, target) {
        Debug.Log("Move Constructor");
    }

    public override void OnEnter() {
        Debug.Log("Move OnEnter");
        if (playerController.moveController is EnemyMoveGroundController2D) {
            moveController = (EnemyMoveGroundController2D)playerController.moveController;
        } else {
            Debug.Log("MovingState but no MoveController found!");
        }
        // test
        target = GameObject.FindObjectOfType<PlayerInput>().transform.position;
        Debug.Log("TARGET:" + target);
    }

    public override AbstractState UpdateState() {
        
        if (currentAction == ACTION.IDLE) {
            return new EnemyIdleState(ACTION.IDLE, playerController);
        }
        

        float directionX = moveController.MoveTo(target);
        if (directionX == 0) {

            return new EnemyIdleState(ACTION.IDLE, playerController);
        }
       
        return null;
    }


    public override void OnExit() {
        // nothing to do
    }

}

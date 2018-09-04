using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : AbstractState {

    public EnemyIdleState(ACTION currentAction, StateMachineGameActor playerController) : base(currentAction, playerController) {
    }

    public override void OnEnter() {
       // nothing to do
    }

    public override AbstractState UpdateState() {
        if (currentAction == ACTION.MOVE) {
            return new EnemyMoveState(currentAction, playerController, target);
        }
        return null;
    }

    public override void OnExit() {
        // nothing to do
    }

}

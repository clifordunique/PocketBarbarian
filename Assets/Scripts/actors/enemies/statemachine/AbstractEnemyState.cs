using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEnemyState : AbstractState {

    public AbstractEnemyState(ACTION currentAction, StateMachineGameActor playerController) : base(currentAction, playerController){
    }

    public AbstractEnemyState(ACTION currentAction, StateMachineGameActor playerController, Vector3 target) : base(currentAction, playerController, target) {
    }

}

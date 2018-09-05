using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractState {

    public EnemyStateMachine playerController;

    public enum ACTION {NA, IDLE, MOVE, JUMP, SHOOT, ACTION, HIT, DEATH };
    public ACTION currentAction = ACTION.NA;
    public Vector3 target = Vector3.zero;


    public AbstractState(ACTION currentAction, EnemyStateMachine playerController) {
        this.currentAction = currentAction;
        this.playerController = playerController;
        this.target = Vector3.zero;
    }

    public AbstractState(ACTION currentAction, EnemyStateMachine playerController, Vector3 target) {
        this.currentAction = currentAction;
        this.playerController = playerController;
        this.target = target;
    }

    public abstract void OnEnter();    
    public abstract void OnExit();

    public virtual AbstractState UpdateState() {
        // do nothing
        return null;
    }

    public virtual void HandleEvent(ACTION action) {
        HandleEvent(action, Vector3.zero);
    }
    public virtual void HandleEvent(ACTION action, Vector3 target) {
        this.currentAction = action;
        this.target = target;
    }

    public virtual void HandleAnimEvent(string parameter) {
        // do nothing
    }

    public void UpdateDirectionX() {

    }
}

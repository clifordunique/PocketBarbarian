using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEnemyState {

    public EnemyController enemyController;
    public IEnemyMoveController2D moveController;

    public AbstractEnemyState(EnemyController stateMachine) {
        this.enemyController = stateMachine;
        if (stateMachine.moveController is IEnemyMoveController2D) {
            moveController = (IEnemyMoveController2D)stateMachine.moveController;
        } 
    }

    public abstract void OnEnter();
    public abstract void OnExit();

    public virtual AbstractEnemyState UpdateState() {
        // do nothing
        return null;
    }

    public AbstractEnemyState GetEnemyState(EnemyAction.ACTION_EVENT requestedAction) {
        if (requestedAction == EnemyAction.ACTION_EVENT.ACTION) {
            return null;
        }
        if (requestedAction == EnemyAction.ACTION_EVENT.HIT) {
            return new EnemyHitState(enemyController);
        }
        if (requestedAction == EnemyAction.ACTION_EVENT.IDLE) {
            return new EnemyIdleState(enemyController);
        }
        if (requestedAction == EnemyAction.ACTION_EVENT.JUMP) {
            return new EnemyJumpState(enemyController);
        }
        if (requestedAction == EnemyAction.ACTION_EVENT.MOVE) {
            return new EnemyMoveState(enemyController);
        }
        if (requestedAction == EnemyAction.ACTION_EVENT.SHOOT) {
            Debug.Log("Return Shoot State!");
            return new EnemyShootState(enemyController);
        }
        return null;
    }
    

    public virtual void HandleAnimEvent(string parameter) {
        // do nothing
    }    
}

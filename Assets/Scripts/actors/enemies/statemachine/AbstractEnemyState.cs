﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEnemyState {

    public EnemyStateMachine stateMachine;
    public IEnemyMoveController2D moveController;

    public AbstractEnemyState(EnemyStateMachine stateMachine) {
        this.stateMachine = stateMachine;
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
            return new EnemyHitState(stateMachine);
        }
        if (requestedAction == EnemyAction.ACTION_EVENT.IDLE) {
            return new EnemyIdleState(stateMachine);
        }
        if (requestedAction == EnemyAction.ACTION_EVENT.JUMP) {
            return new EnemyJumpState(stateMachine);
        }
        if (requestedAction == EnemyAction.ACTION_EVENT.MOVE) {
            return new EnemyMoveState(stateMachine);
        }
        if (requestedAction == EnemyAction.ACTION_EVENT.SHOOT) {
            return new EnemyShootState(stateMachine);
        }
        return null;
    }
    

    public virtual void HandleAnimEvent(string parameter) {
        // do nothing
    }    
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : AbstractEnemyState {

    private float startTime;

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine) {
    }

    public override void OnEnter() {
        startTime = Time.time;
    }

    public override AbstractEnemyState UpdateState() {

        if (stateMachine.currentAction.actionEvent != EnemyAction.ACTION_EVENT.IDLE) {
            // Interrupt current Action
            return GetEnemyState(stateMachine.currentAction.actionEvent);
        }

        if ((Time.time - startTime) > stateMachine.currentAction.duration) {
            Debug.Log("Idle timed out. Next action");
            stateMachine.RequestNextAction();
            return GetEnemyState(stateMachine.currentAction.actionEvent);
        }
        return null;
    }

    public override void OnExit() {
        // nothing to do
    }

}

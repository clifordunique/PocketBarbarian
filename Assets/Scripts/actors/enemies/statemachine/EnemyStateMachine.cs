using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour {

    public EnemyAction.ACTION_EVENT defaultState;

    private AbstractEnemyState currentState;
    [HideInInspector]
    public EnemyAction currentAction;
    private bool isInterruptAction;
    [HideInInspector]
    public MoveGroundController2D moveController;
    [HideInInspector]
    public AiBehaviour aiBehaviour;

    public void Start() {
        moveController = GetComponent<MoveGroundController2D>();
        aiBehaviour = GetComponent<AiBehaviour>();

        if (aiBehaviour) {
            currentAction = aiBehaviour.GetCurrentAction();
        }

        // init ENEMY state
        currentState = new EnemyIdleState(this);
    }

    public void InterruptAction(EnemyAction interruptAction) {
        currentAction = interruptAction;
        isInterruptAction = true;
    }

    public void RequestNextAction() {
        if (aiBehaviour) {
            if (isInterruptAction) {
                // interruptAction finished
                currentAction = aiBehaviour.GetCurrentAction();
                isInterruptAction = false;
            } else {
                currentAction = aiBehaviour.GetNextAction();
            }
        }
    }

    private void Update() {
        // refresh AiBehaviour if not an interruptAction is finished
        if (aiBehaviour && !isInterruptAction) {
            currentAction = aiBehaviour.GetCurrentAction();
        }

        // handle StateMachine
        AbstractEnemyState newState = currentState.UpdateState();
        if (newState != null) {
            currentState.OnExit();
            currentState = newState;
            currentState.OnEnter();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineGameActor : GameActor {

    [Header("Behaviour")]
    public bool pushedOnHit;
    public enum ACTOR_TYPE {PLAYER, ENEMY};
    public ACTOR_TYPE actorType = ACTOR_TYPE.ENEMY;

    private AbstractState currentState;
    [HideInInspector]
    public MoveGroundController2D moveController;


    public override void Start() {
        base.Start();
        moveController = GetComponent<MoveGroundController2D>();

        if (actorType == ACTOR_TYPE.ENEMY) {
            // init ENEMY state
            currentState = new EnemyIdleState(AbstractState.ACTION.MOVE, this);
        } else {
            // init PLAYER state
            //currentState = new IdleTopState(AbstractState.ACTION.NA, this);
        }
    }

    private void Update() {
        // handle StateMachine
        AbstractState newState = currentState.UpdateState();
        if (newState != null) {
            currentState.OnExit();
            currentState = newState;
            currentState.OnEnter();
        }
    }
}

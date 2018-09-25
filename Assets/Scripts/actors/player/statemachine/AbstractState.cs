using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractState {

    public PlayerController playerController;

    public string IDLE_PARAM = "IDLE";
    public string RUNNING_PARAM = "RUNNING";
    public string JUMPING_PARAM = "JUMPING";
    public string DASHING_PARAM = "DASHING";
    public string ATTACK1_PARAM = "ATTACK1";

    public enum ACTION {NA, IDLE, MOVE, JUMP, SHOOT, DASH, ACTION, ATTACK1, HIT, DEATH };
    public ACTION myAction = ACTION.NA;
    public ACTION interruptAction = ACTION.NA;
    public InputController input;

    public AbstractState(ACTION myAction, PlayerController playerController) {
        this.myAction = myAction;
        this.playerController = playerController;
        this.input = InputController.GetInstance();
    }


    public abstract void OnEnter();    
    public abstract void OnExit();

    public virtual AbstractState UpdateState() {
        if (interruptAction != ACTION.NA && interruptAction != myAction) {
            // react to interrupter actions
            if (interruptAction == ACTION.IDLE) {
                return new IdleState(playerController);
            }
        }
        return null;
    }

    public virtual void InterruptEvent(ACTION action) {
        interruptAction = action;
    }

    public virtual void HandleAnimEvent(string parameter) {
        // do nothing
    }

    public void Move(float dirX, float dirY, bool dashMove = false) {
        playerController.updateSpriteDirection(dirX);
        playerController.moveController.OnMove(dirX, dirY, dashMove);
    }
}

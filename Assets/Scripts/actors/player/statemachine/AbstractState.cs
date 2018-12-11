using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractState {

    public PlayerController playerController;

    public static string IDLE_PARAM = "IDLE";
    public static string RUNNING_PARAM = "RUNNING";
    public static string JUMPING_PARAM = "JUMPING";
    public static string FAST_FALLING_PARAM = "FAST_FALLING";
    public static string JUMPING_ATTACK_PARAM = "JUMPING_ATTACK";
    public static string DASHING_PARAM = "DASHING";
    public static string ATTACK1_PARAM = "ATTACK1";
    public static string ATTACK2_PARAM = "ATTACK2";
    public static string ACTION_PARAM = "ACTION";
    public static string ACTION_OUT_PARAM = "ACTION_OUT";
    public static string WALK_OUT_PARAM = "WALK_OUT";
    public static string LANDING_PARAM = "LANDING";
    public static string HARD_LANDING_PARAM = "HARD_LANDING";
    public static string STOMPING_PARAM = "STOMPING";
    public static string STOMPING_LANDING_PARAM = "STOMPING_LANDING";
    public static string DYING_PARAM = "DYING";
    public static string DROWNING_PARAM = "DROWNING";
    public static string THROW_IDLE_PARAM = "THROW_IDLE";
    public static string THROW_JUMP_PARAM = "THROW_JUMP";
    public static string THROW_RUNNING_PARAM = "THROW_RUNNING";
    public static string WALL_SLIDING_PARAM = "WALL_SLIDING";
    public static string WALL_JUMP_PARAM = "WALL_JUMP";

    public static string EVENT_PARAM_HIT = "HIT";
    public static string EVENT_PARAM_ANIMATION_END = "animation_end";
    public static string EVENT_PARAM_THROW = "throw";    

    public enum ACTION {NA, IDLE, MOVE, JUMP, LANDING, SHOOT, DASH, ACTION, ATTACK1, ATTACK2, HIT, DEATH, DROWNING };
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
            if (interruptAction == ACTION.HIT) {
                return new HitState(playerController);
            }
            if (interruptAction == ACTION.DEATH) {
                return new DyingState(playerController);
            }
        }
        return null;
    }

    public virtual void InterruptEvent(ACTION action) {
        interruptAction = action;
    }

    public virtual void HandleEvent(string parameter) {
        // do nothing
    }

    public void Move(float dirX, float dirY, bool dashMove = false) {        
        playerController.moveController.OnMove(dirX, dirY, dashMove);
        playerController.updateSpriteDirection(playerController.moveController.moveDirectionX);
    }
}

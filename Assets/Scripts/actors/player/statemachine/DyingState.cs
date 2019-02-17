using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingState : AbstractState {

    private HitBox.DAMAGE_TYPE damageType;

    public DyingState(HitBox.DAMAGE_TYPE damageType, PlayerController playerController) : base(ACTION.DEATH, playerController) {
        this.damageType = damageType;
    }

    public override void OnEnter() {

        playerController.animator.SetBool(GetAnimParam(), true);
        Move(0F, playerController.input.GetDirectionY());
    }

    public override void OnExit() {
        playerController.animator.SetBool(GetAnimParam(), false);
        Move(0F, playerController.input.GetDirectionY());
    }

    public override AbstractState UpdateState() {
        // can not be interrupted
        return null;
    }

    public override void HandleEvent(string parameter) {
        if (parameter == EVENT_PARAM_ANIMATION_END) {
            GameManager.GetInstance().PlayerDied();
        }
    }


    private string GetAnimParam() {
        switch (damageType) {
            case HitBox.DAMAGE_TYPE.WATER:
                return DYING_DROWN_PARAM;
            default:
                return DYING_PARAM;
        }
    }
 }

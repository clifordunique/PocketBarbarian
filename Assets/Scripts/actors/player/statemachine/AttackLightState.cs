using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackLightState : AbstractState {

    private string attackParam = "";
    private bool attackFinished = false;

    private bool hitSomething = false;

    private bool nextCombo = false;
    private float move = 0F;
    private float moveTime = 0.1F;

    public AttackLightState(PlayerController playerController) : base(ACTION.ATTACK1, playerController) {
    }

    public override void OnEnter() {
        if (Random.value > 0.5f) {
            attackParam = ATTACK_LIGHT_1_PARAM;
        } else {
            attackParam = ATTACK_LIGHT_2_PARAM;
        }
        playerController.animator.SetBool(attackParam, true);
        Move(0, playerController.input.GetDirectionY());
        if (playerController.input.GetDirectionX() != 0) {
            move = Time.timeSinceLevelLoad + moveTime;
        }
    }

    public override void OnExit() {
        playerController.animator.SetBool(attackParam, false);
    }

    public override AbstractState UpdateState() {
        AbstractState interrupt = base.UpdateState();
        if (interrupt != null) {
            return interrupt;
        }

        if (move > Time.timeSinceLevelLoad) {
            Move(playerController.dirX, playerController.input.GetDirectionY());
        } else {
            Move(0, playerController.input.GetDirectionY());
            move = 0;
        }

        if (hitSomething) {
            // small cam Shake
            CameraFollow.GetInstance().ShakeSmall();
            hitSomething = false;
        }
        
        if (InputController.GetInstance().IsAttack1KeyDown()) {
            nextCombo = true;
            Debug.Log("COMBO!");
        }

        // End attack!
        if (attackFinished) {
            playerController.lastAttackTime = Time.timeSinceLevelLoad;

            if (nextCombo) {
                return new AttackHeavyState(playerController);
            } else {
                return new IdleState(playerController);
            }
        }
        return null;
    }

    public override void HandleEvent(string parameter) {
        if (parameter == EVENT_PARAM_ANIMATION_END) {
            attackFinished = true;
        }
        if (parameter == EVENT_PARAM_HIT) {
            hitSomething = true;
        }
    }
}

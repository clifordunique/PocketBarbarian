using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHeavyState : AbstractState {

    private string attackParam = "";
    private bool animationFinished = false;
    private bool attackFinished = false;

    private bool hitSomething = false;
    private bool nextCombo = false;
    private float move = 0F;
    private float moveTime = 0.1F;

    private int MAX_COMBO = 1;
    private int currentCombo = 0;

    public AttackHeavyState(PlayerController playerController) : base(ACTION.ATTACK1, playerController) {
    }

    public override void OnEnter() {
        attackParam = GetRandomAnimation();
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

        if ( InputController.GetInstance().IsAttack1KeyDown()) {
            nextCombo = true;
        }


        if (hitSomething) {
            // small cam Shake
            CameraFollow.GetInstance().ShakeSmall();
            hitSomething = false;
        }

        if (attackFinished && nextCombo && currentCombo < MAX_COMBO) {
            // switch states
            currentCombo++;
            attackFinished = false;
            nextCombo = false;
            playerController.animator.SetBool(attackParam, false);
            attackParam = GetRandomAnimation();
            playerController.animator.SetBool(attackParam, true);
            if (playerController.input.GetDirectionX() != 0) {
                move = Time.timeSinceLevelLoad + moveTime;
            }
        }
        

        // End attack!
        if (animationFinished) {
            if (nextCombo && currentCombo >= MAX_COMBO) {
                return new AttackSmashState(playerController);
            } else {
                playerController.lastAttackTime = Time.timeSinceLevelLoad;
                return new IdleState(playerController);
            }
        }
        return null;
    }

    private string GetRandomAnimation() {
        if (attackParam == "") {
            // first time
            int rand = Random.Range(0, 3);
            if (rand == 0) return ATTACK_HEAVY_1_PARAM;
            if (rand == 1) return ATTACK_HEAVY_2_PARAM;
            if (rand == 2) return ATTACK_HEAVY_3_PARAM;
        } else {
            if (attackParam == ATTACK_HEAVY_1_PARAM) {
                if (Random.value > 0.5f) {
                    return ATTACK_HEAVY_2_PARAM;
                } else {
                    return ATTACK_HEAVY_3_PARAM;
                }
            }
            if (attackParam == ATTACK_HEAVY_2_PARAM) {
                if (Random.value > 0.5f) {
                    return ATTACK_HEAVY_1_PARAM;
                } else {
                    return ATTACK_HEAVY_3_PARAM;
                }
            }
            if (attackParam == ATTACK_HEAVY_3_PARAM) {
                if (Random.value > 0.5f) {
                    return ATTACK_HEAVY_1_PARAM;
                } else {
                    return ATTACK_HEAVY_2_PARAM;
                }
            }
        }
        return "";
    }

    public override void HandleEvent(string parameter) {
        if (parameter == EVENT_PARAM_ANIMATION_END) {
            animationFinished = true;
        }
        if (parameter == EVENT_PARAM_ATTACK_END) {
            attackFinished = true;
        }
        if (parameter == EVENT_PARAM_HIT) {
            hitSomething = true;
        }
    }
}

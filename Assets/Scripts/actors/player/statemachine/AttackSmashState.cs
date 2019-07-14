using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSmashState : AbstractState {

    private string attackParam = "";
    private bool animationFinished = false;

    private bool hitSomething = false;
    private float move = 0F;
    private float moveTime = 0.2F;

    private bool playEffect = false;

    public AttackSmashState(PlayerController playerController) : base(ACTION.ATTACK1, playerController) {
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

        if (playEffect) {
            playerController.InstantiateEffect(playerController.prefabEffectGroundHitOneSided, playerController.dirX * 0.75F);
            CameraFollow.GetInstance().ShakeSmall();
            playEffect = false;
        }

        if (hitSomething) {
            // small cam Shake
            CameraFollow.GetInstance().ShakeSmall();
            hitSomething = false;
        }
        

        // End attack!
        if (animationFinished) {
            playerController.lastAttackTime = Time.timeSinceLevelLoad;
            return new IdleState(playerController);
        }
        return null;
    }

    private string GetRandomAnimation() {
        if (attackParam == "") {
            // first time
            int rand = Random.Range(0, 2);
            if (rand == 0) return ATTACK_SMASH_1_PARAM;
            if (rand == 1) return ATTACK_SMASH_2_PARAM;
        } else {
            if (attackParam == ATTACK_SMASH_1_PARAM) {
                return ATTACK_SMASH_2_PARAM;
            }
            if (attackParam == ATTACK_SMASH_2_PARAM) {
                return ATTACK_SMASH_1_PARAM;
            }
        }
        return "";
    }

    public override void HandleEvent(string parameter) {
        if (parameter == EVENT_PARAM_ANIMATION_END) {
            animationFinished = true;
        }
        if (parameter == EVENT_PARAM_HIT) {
            hitSomething = true;
        }

        if (parameter == "smash") {
            playEffect = true;
        }
    }
}

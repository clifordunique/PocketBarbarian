using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingState : AbstractState {

    private float pitfallTime = 1F;
    private float startTime = 0F;
    private HitBox.DAMAGE_TYPE damageType;
    private Vector3 hitDirection;
    private bool die = false;

    public DyingState(HitBox.DAMAGE_TYPE damageType, PlayerController playerController) : base(ACTION.DEATH, playerController) {
        this.damageType = damageType;
    }

    public override void OnEnter() {

        startTime = Time.timeSinceLevelLoad;

        if (damageType == HitBox.DAMAGE_TYPE.WATER || damageType == HitBox.DAMAGE_TYPE.PIT) {
            CameraFollow.GetInstance().enabled = false;
        }

        if (damageType == HitBox.DAMAGE_TYPE.SQUISH) {
            hitDirection = Utils.GetHitDirection(playerController.lastHit.hitSource, playerController.transform);

            playerController.InstantiateEffect(playerController.prefabEffectBloodSplatt);

            playerController.InstantiateEffect(playerController.prefabEffectSquishBarbarian, GetBoundsPosition(hitDirection, playerController.transform, playerController.boxCollider2D.bounds));
            //playerController.InstantiateEffect(playerController.prefabEffectSquish, GetBoundsPosition(hitDirection), GetEffectRotation(hitDirection));
            
            playerController.spriteRenderer.enabled = false;
            playerController.boxCollider2D.enabled = false;
           // CameraFollow.GetInstance().enabled = false;
        }

        playerController.animator.SetBool(GetAnimParam(), true);
        Move(0F, playerController.input.GetDirectionY());
    }

    public override void OnExit() {
        playerController.animator.SetBool(GetAnimParam(), false);
        Move(0F, playerController.input.GetDirectionY());
    }

    public override AbstractState UpdateState() {
        if (die || (damageType == HitBox.DAMAGE_TYPE.PIT && (startTime + pitfallTime) < Time.timeSinceLevelLoad) ) {
            GameManager.GetInstance().PlayerDied();
        }
        // can not be interrupted
        return null;
    }

    public override void HandleEvent(string parameter) {
        if (parameter == EVENT_PARAM_ANIMATION_END) {
            die = true;
        }
    }


    private string GetAnimParam() {
        switch (damageType) {
            case HitBox.DAMAGE_TYPE.WATER:
                return DYING_DROWN_PARAM;
            case HitBox.DAMAGE_TYPE.PIT:
                return FAST_FALLING_PARAM;
            default:
                return DYING_PARAM;
        }
    }

    private float GetEffectRotation(Vector3 hitDirection) {
        float angel = 0;
        if (hitDirection.x > 0) {
            angel = 90f;
        }
        if (hitDirection.x < 0) {
            angel = -90f;
        }
        if (hitDirection.y > 0) {
            angel = 180f;
        }
        return angel;
    }

    private Vector2 GetBoundsPosition(Vector3 hitDirection, Transform trans, Bounds bounds) {

        Vector2 effectPosition = Vector3.zero;
        
        if (hitDirection.x > 0) {
            float newX = bounds.center.x + bounds.extents.x;
            float newY = trans.position.y;
            effectPosition = new Vector2(newX, newY);
        }
        if (hitDirection.x < 0) {
            float newX = bounds.center.x - bounds.extents.x;
            float newY = trans.position.y;
            effectPosition = new Vector2(newX, newY);
        }

        if (hitDirection.y > 0) {
            float newX = trans.position.x;
            float newY = bounds.center.y + bounds.extents.y;
            effectPosition = new Vector2(newX, newY);
        }
        if (hitDirection.y < 0) {
            float newX = trans.position.x;
            float newY = bounds.center.y - bounds.extents.y;
            effectPosition = new Vector2(newX, newY);
        }

        return effectPosition;
    }
 }

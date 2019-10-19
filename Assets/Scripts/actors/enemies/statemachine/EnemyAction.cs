using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyAction {

    public enum ACTION_EVENT {NA, IDLE, MOVE, JUMP, SHOOT, ACTION, HIT, DIZZY, FALLING };
    public ACTION_EVENT actionEvent = ACTION_EVENT.NA;

    public float amount = 0; // duration to stay in state --> only for Idle, Move and Hit
    public Vector3 hitTarget = Vector3.positiveInfinity; // target to shoot at, or got hit from
    public bool hitTargetIsVector = true;

    [HideInInspector]
    public Vector3 moveTarget = Vector3.positiveInfinity; // target to move to, specified by waypoints
    

    public EnemyAction(ACTION_EVENT actionEvent) {
        this.actionEvent = actionEvent;
    }

    public bool HasMoveTarget() {
        if (moveTarget.Equals(Vector3.positiveInfinity)) {
            return false;
        }
        return true;
    }

    public bool HasHitTarget() {
        if (hitTarget.Equals(Vector3.positiveInfinity)) {
            return false;
        }
        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AiDetector))]
public class AiReactionBehaviour: AiBehaviour {

    public bool hasMaxFollowDistance = false;
    [ConditionalHideAttribute("hasMaxFollowDistance", true)]
    public float maxFollowDistance = 5F;
    [ConditionalHideAttribute("hasMaxFollowDistance", true)]
    public float ignoreTime = 4F;

    public AiReactionAction reactionAction;

    private AiDetector detector;

    private EnemyAction action;

    private Vector3 startPosition;
    private float startIgnore = -1;


    public override void Start() {
        base.Start();
        detector = GetComponent<AiDetector>();
        startPosition = transform.position;
    }


    public override EnemyAction GetCurrentAction() {

        if (startIgnore != -1 && Time.time - startIgnore > ignoreTime) {
            startIgnore = -1;
        }

        bool follow = false;
        if (detector.detectedTarget && (Vector3.Distance(startPosition, transform.position) > maxFollowDistance) ) {
            if (startIgnore == -1) {
                // zu weit weg vom Startpunkt, also ignorieren beginnen
                startIgnore = Time.time;            
            }
        }
        if (detector.detectedTarget && ((hasMaxFollowDistance && startIgnore == -1) || !hasMaxFollowDistance)) {
            follow = true;
        }

        if (follow) {             
            return CreateEnemyAction();
        } else {
            action = null;
            return base.GetCurrentAction();
        }
    }

    private bool IsInSecondActionRange() {
        
        if (Vector3.Distance(detector.detectedTarget.position, transform.position) < reactionAction.distanceSecondAction) {
            return true;
        }
        return false;
    }


    private EnemyAction CreateEnemyAction() {
        if (detector.detectedTarget != null) {
            if (IsInSecondActionRange()) {
                //prepare second Action
                if (reactionAction.action == AiReactionAction.REACTION_ACTION.MOVE_ACTION) {
                    return CreateEnemyAction(EnemyAction.ACTION_EVENT.ACTION, detector.detectedTarget.position);
                }
                if (reactionAction.action == AiReactionAction.REACTION_ACTION.MOVE_SHOOT ||
                    reactionAction.action == AiReactionAction.REACTION_ACTION.SHOOT) {
                    return CreateEnemyAction(EnemyAction.ACTION_EVENT.SHOOT, transform.position);
                }
            } else {
                if (reactionAction.action == AiReactionAction.REACTION_ACTION.MOVE ||
                    reactionAction.action == AiReactionAction.REACTION_ACTION.MOVE_SHOOT ||
                    reactionAction.action == AiReactionAction.REACTION_ACTION.MOVE_ACTION) {
                    return CreateEnemyAction(EnemyAction.ACTION_EVENT.MOVE, detector.detectedTarget.position);
                }
                if (reactionAction.action == AiReactionAction.REACTION_ACTION.SHOOT) {
                    return CreateEnemyAction(EnemyAction.ACTION_EVENT.SHOOT, detector.detectedTarget.position);
                }
            }
        }
        return null;
    }

    private EnemyAction CreateEnemyAction(EnemyAction.ACTION_EVENT actionEvent, Vector3 moveTarget) {
        EnemyAction result = new EnemyAction(actionEvent);
        result.moveTarget = moveTarget;
        result.hitTargetIsVector = reactionAction.hitTargetIsVector;
        if (reactionAction.hitTargetIsVector) {
            result.hitTargetIsVector = reactionAction.hitTargetIsVector;
            if (result.hitTargetIsVector) {
                if (detector.detectedTarget.position.x > transform.position.x) {
                    result.hitTarget = Vector3.right;
                } else {
                    result.hitTarget = Vector3.left;
                }
            } 
        } else {
            result.hitTarget = detector.detectedTarget.position;
        }
        return result;
    }

    public override EnemyAction GetNextAction() {
        action = null;
        return base.GetNextAction();
    }
}

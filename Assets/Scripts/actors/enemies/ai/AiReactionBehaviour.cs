using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AiDetector))]
public class AiReactionBehaviour: AiBehaviour {

    public bool locateTargetOnce = false;

    private AiDetector detector;

    private EnemyAction action;


    public override void Start() {
        base.Start();
        detector = GetComponent<AiDetector>();
    }


    public override EnemyAction GetCurrentAction() {
        if (detector.detectedTarget) {
            if (action == null) {
                Debug.Log("Neu detected!");
                action = new EnemyAction(EnemyAction.ACTION_EVENT.MOVE);
                action.moveTarget = detector.detectedTarget.position;
            }
            if (!locateTargetOnce) {
                action.moveTarget = detector.detectedTarget.position;
            }
            Debug.Log(action.moveTarget);
            return action;
        } else {
            action = null;
            return base.GetCurrentAction();
        }
    }

    public override EnemyAction GetNextAction() {
        action = null;
        return base.GetNextAction();
    }
}

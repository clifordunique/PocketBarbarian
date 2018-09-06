using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiWaypoint : MonoBehaviour {

    public EnemyAction action;

	// Use this for initialization
	void Awake () {
	    // convert current position to targetPosition in action
        if (action.actionEvent == EnemyAction.ACTION_EVENT.MOVE || action.actionEvent == EnemyAction.ACTION_EVENT.JUMP || 
            action.actionEvent == EnemyAction.ACTION_EVENT.SHOOT) {
            action.moveTarget = transform.position;
        }
	}
}

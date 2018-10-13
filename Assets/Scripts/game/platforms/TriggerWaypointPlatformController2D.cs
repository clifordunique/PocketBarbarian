using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWaypointPlatformController2D: WaypointPlatformController2D {

    public AnimationTrigger animationTrigger;

    public override void Start() {
        base.Start();
        if (!animationTrigger) {
            // wenn kein Trigger in public variable dann in eigenem gameobject suchen
            animationTrigger = GetComponent<AnimationTrigger>();
        } 
    }

    public override Vector3 CalculatePlatformMovement() {
        Vector3 result = base.CalculatePlatformMovement();
        if (result == Vector3.zero) {
            animationTrigger.StopTrigger();
        } else {
            animationTrigger.StartTrigger();
        }
        return result;
    }    
}

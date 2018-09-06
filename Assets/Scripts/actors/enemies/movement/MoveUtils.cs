using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUtils {

    public static bool TargetReachedX(Transform transform, Vector3 target) {
        if (Mathf.Abs(transform.position.x - target.x) <= Constants.WorldUnitsPerPixel()) {
            return true;
        } else {
            return false;
        }
    }

    public static bool TargetReachedXY(Transform transform, Vector3 target) {
        float distanceX = Mathf.Abs(transform.position.x - target.x);
        float distanceY = Mathf.Abs(transform.position.y - target.y);
        if (distanceX <= Constants.WorldUnitsPerPixel() && distanceY <= Constants.WorldUnitsPerPixel()) {
            return true;
        } else {
            return false;
        }
    }
}

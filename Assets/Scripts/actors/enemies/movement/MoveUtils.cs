using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUtils {

    public static bool TargetReachedX(Transform transform, Vector3 target) {
        if (Mathf.Abs(transform.position.x - target.x) <= Constants.WorldUnitsPerPixel() * 2) {
            return true;
        } else {
            return false;
        }
    }

    public static bool TargetReachedXY(Transform transform, Vector3 target, float speed, float smoothTime) {
        float distanceX = Mathf.Abs(transform.position.x - target.x);
        float distanceY = Mathf.Abs(transform.position.y - target.y);
        float multiplier = 1 + (smoothTime > 0 ? speed * smoothTime : 0);
        if (distanceX <= Constants.WorldUnitsPerPixel() * multiplier && distanceY <= Constants.WorldUnitsPerPixel() * multiplier) {
            return true;
        } else {
            return false;
        }
    }
}

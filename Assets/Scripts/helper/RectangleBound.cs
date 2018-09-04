using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleBound {

    float maxX = 0F;
    float minX = 0F;
    float maxY = 0F;
    float minY = 0F;

    public RectangleBound (Vector3[] allPoints) {

        foreach (Vector3 currentPoint in allPoints) {
            if (currentPoint.x > maxX) maxX = currentPoint.x;
            if (currentPoint.x < minX) minX = currentPoint.x;
            if (currentPoint.y > maxY) maxY = currentPoint.y;
            if (currentPoint.y < maxX) minY = currentPoint.y;
        }
    }

    public bool IsInBound(Vector3 checkPoint) {
        if (checkPoint.x <= maxX && checkPoint.x >= minX && checkPoint.y <= maxY && checkPoint.y >= minY) {
            return true;
        } else {
            return false;
        }
    }

    public bool IsInBoundX(Vector3 checkPoint) {
        if (checkPoint.x <= maxX && checkPoint.x >= minX) {
            return true;
        } else {
            return false;
        }
    }
}

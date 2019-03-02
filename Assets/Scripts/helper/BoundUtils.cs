using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundUtils 
{
    public static float GetEffectRotation(Vector3 velocity, bool reverse) {
        float angel = 0;
        if (velocity.x > 0) {
            angel = (reverse ? 90f : -90f);
        }
        if (velocity.x < 0) {
            angel = (reverse ? -90f : 90f);
        }
        if (velocity.y < 0) {
            angel = (reverse ? 180f : 0f);
        }
        if (velocity.y > 0) {
            angel = (reverse ? 0f : 180f);
        }
        return angel;
    }

    public static Vector2 GetPositionOnBounds(Vector3 velocity, Transform trans, Bounds bounds, int effectiveSize = 0) {

        Vector2 effectPosition = Vector3.zero;

        if (velocity.x > 0) {
            float newX = bounds.center.x + bounds.extents.x;
            float newY = trans.position.y;
            if (effectiveSize > 0) {
                newY = CorrectPositionOnBoundsY(trans.position, bounds, effectiveSize);
            }
            effectPosition = new Vector2(newX, newY);
        }
        if (velocity.x < 0) {
            float newX = bounds.center.x - bounds.extents.x;
            float newY = trans.position.y;
            if (effectiveSize > 0) {
                newY = CorrectPositionOnBoundsY(trans.position, bounds, effectiveSize);
            }
            effectPosition = new Vector2(newX, newY);
        }

        if (velocity.y > 0) {
            float newX = trans.position.x;
            if (effectiveSize > 0) {
                newX = CorrectPositionOnBoundsX(trans.position, bounds, effectiveSize);
            }
            float newY = bounds.center.y + bounds.extents.y;
            effectPosition = new Vector2(newX, newY);
        }
        if (velocity.y < 0) {
            float newX = trans.position.x;
            if (effectiveSize > 0) {
                newX = CorrectPositionOnBoundsX(trans.position, bounds, effectiveSize);
            }
            float newY = bounds.center.y - bounds.extents.y;
            effectPosition = new Vector2(newX, newY);
        }

        return effectPosition;
    }

    public static float CorrectPositionOnBoundsX(Vector3 position, Bounds bounds, int sizeInPixelX ) {
        float distanceEffectX = Utils.PixelToWorldunits(sizeInPixelX) / 2;
        float newX = position.x;

        if (bounds.min.x > (position.x - distanceEffectX)) {
            newX = bounds.min.x + distanceEffectX;
        }
        if (bounds.max.x < (position.x + distanceEffectX)) {
            newX = bounds.max.x - distanceEffectX;
        }
        return newX;
    }

    public static float CorrectPositionOnBoundsY(Vector3 position, Bounds bounds, int sizeInPixelY) {
        float distanceEffectY = Utils.PixelToWorldunits(sizeInPixelY) / 2;
        float newY = position.y;

        if (bounds.min.x > (position.x - distanceEffectY)) {
            newY = bounds.min.y + distanceEffectY;
        }
        if (bounds.max.x < (position.x + distanceEffectY)) {
            newY = bounds.max.y - distanceEffectY;
        }
        return newY;
    }
}

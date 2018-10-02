using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils {

    public static Vector2 MakePixelPerfect(Vector2 v) {
        Vector2 result;
        result.x = (Mathf.Round(v.x * Constants.PPU) / Constants.PPU);
        result.y = (Mathf.Round(v.y * Constants.PPU) / Constants.PPU);
        return result;
    }

    public static Vector3 GetHitDirection(Vector3 attacker, Transform transform) {
        Vector3 v = new Vector3(transform.position.x - attacker.x, transform.position.y - attacker.y, 1).normalized;
        Vector3 result = new Vector3();
        if (v.x > 0F) result.x = 1;
        if (v.x <= -0F) result.x = -1;
        Debug.Log("v.y:" + v.y);
        if (v.y > 0.5F) result.y = 1;
        if (v.y <= -0.5F) result.y = -1;
        return result;
    }



    public static Vector3 GetSpawnPositionProjectileStaticTarget(Vector3 target, Transform transform, BoxCollider2D boxCollider) {
        // calculate position where to spawn
        float directionX = 0;
        float directionY = 0;

        Vector3 dir = transform.position - target;
        dir = transform.InverseTransformDirection(dir);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if ((angle >= 0 && angle < 45) || (angle < 0 && angle > -45)) directionX = -1; //links
        if ((angle <= 180 && angle > 135) || (angle < -180 && angle < -135)) directionX = 1; //rechts
        if (angle <= -45 && angle >= -135) directionY = 1; //oben
        if (angle >= 45 && angle <= 135) directionY = -1; //unten
        
        return GetSpawnPosition(directionX, directionY, transform, boxCollider);
    }


    public static Vector3 GetSpawnPositionProjectileVector(Vector3 target, Transform transform, BoxCollider2D boxCollider) {
        // calculate position where to spawn
        float directionX = target.x;
        float directionY = target.y;

        return GetSpawnPosition(directionX, directionY, transform, boxCollider);

    }

    private static Vector3 GetSpawnPosition(float directionX, float directionY, Transform transform, BoxCollider2D boxCollider) {
        Vector3 spawnPosition = Vector3.zero;
        if (boxCollider) {
            // Object has box collider, so take the edge of the collider
            Vector3 center = boxCollider.bounds.center;
            if (directionY == 0) {
                float positionX = center.x + (directionX * boxCollider.bounds.extents.x);
                spawnPosition = new Vector3(positionX, center.y, transform.position.z);
            }
            if (directionX == 0 || (directionX != 0 && directionY != 0)) {
                float positionY = center.y + (directionY * boxCollider.bounds.extents.y);
                spawnPosition = new Vector3(center.x, positionY, transform.position.z);
            }
        } else {
            // no collider, then spawn from center
            spawnPosition = transform.position;
        }
        return spawnPosition;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovement : RaycastController2D {
    

    public float seconds;
    public float distance;

    private Vector3 endpos;
    private Vector3 startPos;

    // Use this for initialization
    public override void Start () {
        base.Start();

        startPos = transform.position;
        float scaleX = transform.localScale.x;
        endpos = new Vector3(transform.position.x + (scaleX * distance), transform.position.y, transform.position.z);

        StartCoroutine("SmoothMove");
    }
    
    IEnumerator SmoothMove() {
        float t = 0.0f;
        while (t <= 1.0) {
            UpdateRaycastOrigins();

            t += Time.deltaTime / seconds;

            Vector3 newPosition = Vector3.Lerp(startPos, endpos, EasingFunction.EaseOutQuint(0.0f, 1.0f, t));
            float dirX = (startPos.x < endpos.x ? 1 : -1);
            float moveX = transform.position.x - newPosition.x;
            float hitModifier = HorizontalCollisions(moveX, dirX);
            if (hitModifier != 0) {
                t = 2.0f;
                newPosition = new Vector3(newPosition.x - dirX * hitModifier, newPosition.y, newPosition.z);
            }

            Vector2 pixelPerfectMoveAmount = Utils.MakePixelPerfect(newPosition);
            transform.position = pixelPerfectMoveAmount;

            yield return new WaitForEndOfFrame();
        }
    }



    float HorizontalCollisions(float moveX, float dirX) {
        float directionX = dirX;
        float rayLength = Mathf.Abs(moveX) + skinWidth;
        
        if (Mathf.Abs(moveX) < skinWidth) {
            rayLength = 2 * skinWidth;
        }

        for (int i = 0; i < horizontalRayCount; i++) {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit) {
                
                if (hit.distance == 0) {
                    continue;
                }

                return Mathf.Abs(moveX) - (hit.distance - skinWidth);
            }
        }
        return 0;
    }
}

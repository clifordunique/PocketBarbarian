using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPhysicsSimulator : RaycastController2D {

    public float gravity = 5F;

    private bool above;
    private bool below;
    private Vector2 velocity;
    private BoxCollider2D boxCollider;

    public override void Start() {
        base.Start();
        boxCollider = GetComponent<BoxCollider2D>();
    }
	
	void Update () {

        UpdateRaycastOrigins();
        above = false;
        below = false;

        boxCollider.enabled = false;
        velocity.y += - (gravity * gravity) * Time.deltaTime;


        Vector2 moveAmount = velocity * Time.deltaTime;
        VerticalCollisions(ref moveAmount);
        transform.Translate(moveAmount);
        if (below || above) {
            velocity.y = 0;
        }
        boxCollider.enabled = true;
    }

    void VerticalCollisions(ref Vector2 moveAmount) {
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++) {

            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);



            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if (hit) {
                if (hit.transform.gameObject == this.gameObject) {
                    // is the same gameobject
                    Debug.Log("Is Same Gameobject: " + this.gameObject.name);
                    continue;
                }
                if (hit.collider.tag == "Through") {
                    if (directionY == 1 || hit.distance == 0) {
                        continue;
                    }
                }

                moveAmount.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                below = directionY == -1;
                above = directionY == 1;
            }
        }
    }
}

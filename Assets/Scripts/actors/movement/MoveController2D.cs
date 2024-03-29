﻿using UnityEngine;
using System.Collections;

public class MoveController2D : RaycastController2D, IMove {

    public bool movePixelPerfect = false;

    public CollisionInfo collisions;
	[HideInInspector]
    public float moveDirectionX = 0F;
    [HideInInspector]
    public float moveDirectionY = 0F;    


    public override void Start() {
		base.Start ();
		collisions.faceDir = 1;

	}

	public void Move(Vector2 moveAmount, bool standingOnPlatform = false) {
        UpdateRaycastOrigins ();

		collisions.Reset ();
		collisions.moveAmountOld = moveAmount;      


        if (moveAmount.x != 0) {
			collisions.faceDir = (int)Mathf.Sign(moveAmount.x);
		}

		HorizontalCollisions (ref moveAmount);
		if (moveAmount.y != 0) {
			VerticalCollisions (ref moveAmount);
		}

        if (movePixelPerfect) {
            moveAmount = Utils.MakePixelPerfect(moveAmount);
        }
        transform.Translate(moveAmount);

        if (standingOnPlatform) {
			collisions.below = true;
            collisions.onPlatform = true;
        }
        
	}

	void HorizontalCollisions(ref Vector2 moveAmount) {
		float directionX = collisions.faceDir;
		float rayLength = Mathf.Abs (moveAmount.x) + skinWidth;

		if (Mathf.Abs(moveAmount.x) < skinWidth) {
			rayLength = 2*skinWidth;
		}

		for (int i = 0; i < horizontalRayCount; i ++) {
			Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.right * directionX,Color.red);

			if (hit) {

				if (hit.distance == 0) {
					continue;
				}

                moveAmount.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;                
			}
		}
	}

	void VerticalCollisions(ref Vector2 moveAmount) {
		float directionY = Mathf.Sign (moveAmount.y);
		float rayLength = Mathf.Abs (moveAmount.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i ++) {

			Vector2 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            


            Debug.DrawRay(rayOrigin, Vector2.up * directionY,Color.red);

			if (hit) {
                if (hit.transform.gameObject == this.gameObject) {
                    // is the same gameobject
                    continue;
                }
				if (hit.collider.tag == "Through") {
					if (directionY == 1 || hit.distance == 0) {
						continue;
					}
					if (collisions.fallingThroughPlatform) {
						continue;
					}
					if (moveDirectionY == -1) {
						collisions.fallingThroughPlatform = true;
						Invoke("ResetFallingThroughPlatform",.1f);
						continue;
					}
				}

				moveAmount.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
			}
		}
	}


	void ResetFallingThroughPlatform() {
		collisions.fallingThroughPlatform = false;
	}

    public bool IsBelow() {
        return collisions.below;
    }

    public bool IsAbove() {
        return collisions.above;
    }

    public bool IsLeft() {
        return collisions.left;
    }

    public bool IsRight() {
        return collisions.right;
    }

    public struct CollisionInfo {
		public bool above, below;
		public bool left, right;

        public bool onPlatform;
        public bool initialised;

		public Vector2 moveAmountOld;
		public int faceDir;
		public bool fallingThroughPlatform;

		public void Reset() {
			above = below = false;
			left = right = false;
            onPlatform = false;
            initialised = true;

        }
	}

}

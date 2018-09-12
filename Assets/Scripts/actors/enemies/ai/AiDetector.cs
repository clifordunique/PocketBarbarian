using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDetector : MonoBehaviour {

    public LayerMask collisionMask;
    public LayerMask detectionMask;
    public bool xAxis;
    public bool yAxis;
    public float detectionRange;

    private int horizontalRayCount;
    private int verticalRayCount;
    private float dstBetweenRays = .25f;
    private float horizontalRaySpacing;
    private float verticalRaySpacing;

    private BoxCollider2D myCollider;
    private Vector2 rayOriginBottom;
    private Vector2 rayOriginleft;
    [HideInInspector]
    public Transform detectedTarget;

    // Use this for initialization
    void Start () {
        myCollider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    public void CalculateRaySpacing() {
        Bounds bounds = myCollider.bounds;

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        horizontalRayCount = Mathf.RoundToInt(boundsHeight / dstBetweenRays);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    // Update is called once per frame
    void Update () {
		if (Time.frameCount % 10 == 0) {
            UpdateRaycastOrigins();
            Transform hitTransform = null;

            // Calculate direction vectors
            if (!hitTransform && xAxis) {
                hitTransform = Detect(Vector2.left, true);
                if (!hitTransform) {
                    hitTransform = Detect(-1 * Vector2.left, true);
                }
            }
            
            if (!hitTransform && yAxis) {
                hitTransform = Detect(Vector2.up, false);
                if (!hitTransform) {
                    hitTransform = Detect(-1 * Vector2.up, false);
                }
            }

            if (!hitTransform && yAxis && xAxis) {
                Vector2[] vList1 = CalculateVectors(Vector2.up, Vector2.left);
                
                foreach(Vector2 v in vList1) {
                    hitTransform = Detect(v, true);
                    if (hitTransform) return;
                }
                Vector2[] vList2 = CalculateVectors(Vector2.down, Vector2.left);
                foreach (Vector2 v in vList2) {
                    hitTransform = Detect(v, true);
                    if (hitTransform) return;
                }
            }

            if (hitTransform) {
                detectedTarget = hitTransform.transform;
            } else {
                detectedTarget = null;
            }
        }
	}

    private Vector2[] CalculateVectors(Vector2 v1, Vector2 v2) {
        Vector2[] result = new Vector2[6];
        result[0] = (v1 + (v2 * 0.5F)).normalized;
        result[1] = (v1 + (v2 * 1F)).normalized;
        result[2] = (v1 + (v2 * 2F)).normalized;
        result[3] = (v1 - (v2 * 0.5F)).normalized;
        result[4] = (v1 - (v2 * 1F)).normalized;
        result[5] = (v1 - (v2 * 2F)).normalized;
        return result;
    }


    public void UpdateRaycastOrigins() {
        Bounds bounds = myCollider.bounds;

        rayOriginBottom = new Vector2(bounds.center.x, bounds.min.y + 0.1F);
        rayOriginleft = new Vector2(bounds.min.x, bounds.center.y);
    }


    private Transform Detect(Vector2 directionVector, bool verticalRayOrigin) {
        
        int counter = (verticalRayOrigin ? verticalRayCount : horizontalRayCount);
        for (int i = 0; i < counter; i++) {

            Vector2 rayOrigin = (verticalRayOrigin) ? rayOriginBottom : rayOriginleft;
            if (verticalRayOrigin) {
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            } else {
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
            }
            

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, directionVector, detectionRange, collisionMask);
            Debug.DrawRay(rayOrigin, directionVector * detectionRange, Color.blue);

            if (hit && (detectionMask == (detectionMask | (1 << hit.transform.gameObject.layer)))) {
                return hit.transform;
            }
        }
        return null;
    }
}

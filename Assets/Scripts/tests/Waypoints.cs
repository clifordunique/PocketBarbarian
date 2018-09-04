using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints: MonoBehaviour {

    public bool moveSinus = false;
    public float frequency = 20.0f;  // Speed of sine movement
    public float magnitude = 0.5f;   // Size of sine movement
    
    public bool moveThroughWaypoints = true;
    public Transform[] wayPointList;
    public bool circular = false;
    public float speed = 4f;

    public Vector3 testJumpTransform;
    private Vector3 jumpTarget;
    private Vector3 startPos;
    private float arcHeight;
    private bool dive = false;
    
    private int nextWayPointIndex = 0;
    private Vector3[] waypointsWorldSpace;

    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;


    // Use this for initialization
    void Start() {
        // waypoints liegen in child objekten. Diese würden mit wandern, daher zu begin die Vectoren im WorldSpace merken
        waypointsWorldSpace = new Vector3[wayPointList.Length];
        int i = 0;
        foreach (Transform wayPoint in wayPointList) {
            waypointsWorldSpace[i++] = wayPoint.position;
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            // toggleMoveThroughWaypoints
            moveThroughWaypoints = !moveThroughWaypoints;
            float targetX = Mathf.Abs(transform.position.x - testJumpTransform.x);
            arcHeight = -1 * Mathf.Abs(transform.position.y - testJumpTransform.y);
            jumpTarget = new Vector3(transform.position.x + 2 * targetX, transform.position.y, transform.position.z);
            startPos = transform.position;
            dive = true;
        }

        if (dive) {
            DiveTest();
        }

        // check if we have somewere to walk
        if (moveThroughWaypoints) {
            // check if end of waypoints
            if (nextWayPointIndex < this.wayPointList.Length) {                
                MoveNextWaypoint();
            }
        }
    }

    void DiveTest() {

        if (Vector3.Distance(transform.position, testJumpTransform) > 1) {
            transform.position = Vector3.MoveTowards(transform.position, testJumpTransform, speed * Time.deltaTime);
        } else {
            transform.position = Vector3.SmoothDamp(transform.position, testJumpTransform, ref velocity, smoothTime);
        }
    }

    void DiveTo() {
        // Compute the next position, with arc added in
        float x0 = startPos.x;
        float x1 = jumpTarget.x;
        float dist = x1 - x0;
        float nextX = Mathf.MoveTowards(transform.position.x, x1, speed * Time.deltaTime);
        float baseY = Mathf.Lerp(startPos.y, jumpTarget.y, (nextX - x0) / dist);
        float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);

        Vector3 nextPos = new Vector3(nextX, baseY + arc, transform.position.z);

        transform.position = nextPos;

        // Do something when we reach the target
        if (transform.position == jumpTarget) {
            dive = false;
            moveThroughWaypoints = true;
        }
    }

    void MoveNextWaypoint() {

        // move towards the target
        Vector3 nextWayPoint = waypointsWorldSpace[nextWayPointIndex];

        transform.position = Vector3.MoveTowards(transform.position, nextWayPoint, speed * Time.deltaTime);
        if (moveSinus) {
            transform.position = transform.position + Vector3.up * (Mathf.Sin(Time.time * frequency) * magnitude);
        }

        float distance = Vector3.Distance(transform.position, nextWayPoint);

        if ((moveSinus && distance < magnitude) || distance == 0) {
            if (nextWayPointIndex + 1 == wayPointList.Length && circular) {
                nextWayPointIndex = 0;
            } else {
                nextWayPointIndex++;
            }
        }
    }


    void OnDrawGizmos() {
        if (wayPointList != null) {
            Gizmos.color = Color.blue;
            float size = .3f;

            for (int i = 0; i < wayPointList.Length; i++) {
                Vector3 globalWaypointPos = (Application.isPlaying) ? waypointsWorldSpace[i] : wayPointList[i].position;
                Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
            }
        }
    }
}

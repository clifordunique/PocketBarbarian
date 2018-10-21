using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Curve: MonoBehaviour {


    [Tooltip("Horizontal speed, in units/sec")]
    public float speed = 10;

    [Tooltip("How high the arc should be, in units")]
    public float arcHeight = 1;

    public Transform targetTransform;

    
    private Vector3 targetPos;
    private Vector3 startPos;
    private bool moveBack = false;
    private float timeBack;

    Vector3[] waypointsBack = new Vector3[3];

    void Start() {
        // Cache our start position, which is really the only thing we need
        // (in addition to our current position, and the target).
        startPos = transform.position;
        targetPos = targetTransform.position;
    }

    void Update() {
        if (!moveBack) {
            // Compute the next position, with arc added in
            float x0 = startPos.x;
            float x1 = targetPos.x;
            float dist = x1 - x0;
            float nextX = Mathf.MoveTowards(transform.position.x, x1, speed * Time.deltaTime);
            float baseY = Mathf.Lerp(startPos.y, targetPos.y, (nextX - x0) / dist);
            float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);

            Vector3 nextPos = new Vector3(nextX, baseY + arc, transform.position.z);

            transform.position = nextPos;

            // Do something when we reach the target
            if (Mathf.Abs(arcHeight - arc) <= 0.1F) {
                Arrived();
            }
        } else {
            timeBack += Time.deltaTime / 2;
            transform.position = GetPointHelper(timeBack, waypointsBack);
        }
    }

    void Arrived() {
        //Instantiate(arrivedEffect, transform.position, transform.rotation);
        moveBack = true;
        waypointsBack[0] = transform.position;
        waypointsBack[1] = targetPos;
        waypointsBack[2] = startPos;
        timeBack = 0;
    }


    private Vector3 GetPointHelper(float t, Vector3[] verts) {
        if (verts.Length == 1) return verts[0];
        if (verts.Length == 2) return Vector3.Lerp(verts[0], verts[1], t);

        Vector3[] reducedArray = new Vector3[verts.Length - 1];
        for (int v = 0; v < reducedArray.Length; v++) {
            reducedArray[v] = Vector3.Lerp(verts[v], verts[v + 1], t);
        }
        return GetPointHelper(t, reducedArray);
    }


}
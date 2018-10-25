using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierTest : MonoBehaviour {

    public float baseSpeed;

    private Vector3[] waypoints = new Vector3[4];

    float t;
    float time;

    public bool startFlight = false;

    // Use this for initialization
    void Start () {

	}

    public void StartFlight(Transform target) {
        t = 0;
        float distance = Vector3.Distance(target.position, transform.position);
        
        time = baseSpeed + (distance * 2) * 0.02F;

        startFlight = true;
        waypoints[0] = transform.position;
        waypoints[3] = transform.position;

        float multiplier = -1F;
        if (target.position.x > transform.position.x) {
            multiplier = 1F;
        }

        float factorX = 1F + Mathf.Abs(target.position.x - transform.position.x) * 0.025F;
        float factorY = 1.4F;
        Vector3 doubleDistance1 = new Vector3((transform.position.x - multiplier * 5 + (target.position.x - transform.position.x) * factorX), (transform.position.y + (target.position.y - transform.position.y) * factorY), 1);
        Vector3 doubleDistance2 = new Vector3((transform.position.x + multiplier * 5 + (target.position.x - transform.position.x) * factorX), (transform.position.y + (target.position.y - transform.position.y) * factorY), 1);
        waypoints[1] = doubleDistance1;
        waypoints[2] = doubleDistance2;
    }


    public Vector3 GetPoint(float t) {
        return GetPointHelper(t, waypoints);
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

    // Update is called once per frame
    void Update () {
        if (startFlight) {
            t += Time.deltaTime / time;
            transform.position = GetPoint(t);
        }
    }
}

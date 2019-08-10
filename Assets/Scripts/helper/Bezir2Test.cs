using UnityEngine;
using System;
using System.Collections;

public class Bezir2Test: MonoBehaviour {
    public int NumPoints = 100;
    public Point[] points;

    public float speed = 10F;
    private Vector2 startPos;
    public Vector2 controlPoint;
    public Vector2 wayPoint;

    private float distance = 0;
    private float travelDistance = 0;

    private bool start = false;

    void Start() {
        startPos = transform.position;

        points = new Point[NumPoints + 1];
        Vector2 last = Vector2.zero;        
        for (int t = 0; t <= NumPoints; t++) {
            Vector2 current = GetBezierPoint((float)t / (float)NumPoints);
            if (t > 0) {
                points[t] = new Point();
                distance = points[t].SetPoint(last, current, distance);
            }
            last = current;
        }
    }


    void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            start = true;
        }

        if (start) {
            travelDistance += distance * Time.deltaTime * speed;
            Debug.Log("Distance:" + distance + " / travelDistance:" + travelDistance);
            transform.position = Travel(0.1F);
        }
    }

    public Vector2 Travel(float distance) {
        Point current = null;
        int index = 0;

        while (points.Length < index && points[index].distanceTraveled < distance) {
            current = points[index];
            index++;
        }

        if (current == null || distance < 0) return points[NumPoints].current;

        var amount = Mathf.InverseLerp(0, current.distance, distance - current.distanceTraveled);

        return Vector2.Lerp(current.current, current.next, amount);
    }

    private Vector2 GetBezierPoint(float t) {
        float x = (((1 - t) * (1 - t)) * startPos.x) + (2 * t * (1 - t) * controlPoint.x) + ((t * t) * wayPoint.x);
        float y = (((1 - t) * (1 - t)) * startPos.y) + (2 * t * (1 - t) * controlPoint.y) + ((t * t) * wayPoint.y);
        return new Vector2(x, y);
    }
}

public class Point {
    public Vector2 current;
    public Vector2 next;
    public float distance;
    public float distanceTraveled;

    public float SetPoint(Vector2 current, Vector2 next, float distanceTraveled) {
        this.current = current;
        this.next = next;
        this.distance = (next - current).magnitude;
        this.distanceTraveled = distanceTraveled + this.distance;
        return distanceTraveled + this.distance;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezirTest : MonoBehaviour
{
    private Vector2 startPos;
    private Vector2 endPos;
    private Vector2 controllPos;
    private float startTime;

    public Vector2 targetPos;
    
    public Vector2 lastPos;

    public float duration;
    private float time = 0.5F;

    private bool start = false;
    private bool secondPart = false;


    public float speed; // target linear speed

    // determine an initial value by checking where speedFactor converges
    float speedFactor;

    float targetStepSize; // divide by fixedUpdate frame rate
    float lastStepSize;

    // Start is called before the first frame update
    void Start()
    {

        startPos = transform.position;
        float plusX = targetPos.x - transform.position.x;
        endPos = new Vector2(transform.position.x + 2 * plusX, transform.position.y);
        
        //time = 0.5F;
        Debug.Log("Time:" + time);
        controllPos = Formular(time);        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) {
            start = true;
            startTime = Time.time;
        }

        if (start) {
            // Take a note of your previous position.
            Vector3 previousPosition = transform.position;

            var t = (Time.time - startTime) / duration;
            t += speedFactor * Time.deltaTime;
            if (t < 1.0f) {
                transform.position = Bezier(t, startPos, controllPos, endPos);
                /* if (t > time && !secondPart) {
                     secondPart = true;
                     t = 0;
                     startPos = transform.position;
                     controllPos = endPos;
                     endPos = lastPos;
                     startTime = Time.time;
                     duration *= 2;
                 }*/
            } else {
                transform.position = endPos; //1 or larger means we reached the end
            }
            // Measure your movement length
            lastStepSize = Vector3.Magnitude(transform.position - previousPosition);

            // Accelerate or decelerate according to your latest step size.
            if (lastStepSize < targetStepSize) {
                speedFactor *= 1.2f;
            } else {
                speedFactor *= 0.8f;
            }
            Debug.Log("SPEEDFACTOR:" + speedFactor * Time.deltaTime);
            
        }
    }



    public Vector2 Bezier(float t, Vector2 a, Vector2 b, Vector2 c) {
        var ab = Vector2.Lerp(a, b, t);
        var bc = Vector2.Lerp(b, c, t);
        return Vector2.Lerp(ab, bc, t);
    }

    private Vector2 Formular(float time) {
        float x = (targetPos.x - startPos.x * Mathf.Pow(time, 2) - endPos.x * Mathf.Pow(1 - time, 2)) / (2 * time * (1- time));
        float y = (targetPos.y - startPos.y * Mathf.Pow(time, 2) - endPos.y * Mathf.Pow(1 - time, 2)) / (2 * time * (1 - time));
        return new Vector2(x, y);
    }
}

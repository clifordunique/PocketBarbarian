using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCurveTest : MonoBehaviour
{
    public AnimationCurve curve;

    public Transform startPos;
    public Transform endPos;
    public float time;

    private bool start = false;
    private float startTime;

    private Vector3 startV;
    private Vector3 endV;
    // Start is called before the first frame update
    void Start()
    {
        startV = startPos.position;
        endV = endPos.position;
    }

    private float PercentTime() {
        float currentTime = Time.timeSinceLevelLoad - startTime;
        return currentTime / time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) {
            start = true;
            startTime = Time.timeSinceLevelLoad;
        }

        if (start) {
            float percent = PercentTime();
            if (percent >= 0 && percent <= 1) {
                Debug.Log("Percent: " + percent);
                transform.position = Vector3.Lerp(startV, endV, curve.Evaluate(percent));

            } else {
                start = false;
            }
        }

    }
}

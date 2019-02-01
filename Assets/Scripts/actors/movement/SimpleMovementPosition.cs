using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovementPosition : MonoBehaviour {
    

    public float seconds;    
    public bool autoStart = true;
    public float startDelay;

    
    public Vector3 endpos;
    private Vector3 startPos;

    private float t = 0.0f;
    public bool isMoving = false;
    

    // Use this for initialization
    public void Start () {        
    }

    public bool StartMoving() {
        startPos = transform.position;
        if (!isMoving) {
            StartCoroutine(SmoothMove());
            return true;
        }
        return false;
    }

    public void Update() {
        if (autoStart && !isMoving) {
            Debug.Log("Start SmoothMove");
            StartCoroutine(SmoothMove());
        }
    }

    IEnumerator SmoothMove() {
        isMoving = true;
        yield return new WaitForSeconds(startDelay);
        
        t = 0.0f;
        while (t <= 1.0) {
            t += Time.deltaTime / seconds;
            float v = t;

            v = EasingFunction.EaseOutBounce(0.0f, 1.0f, t);
            Vector3 newPosition = Vector3.Lerp(startPos, endpos, v);

            Vector2 pixelPerfectMoveAmount = Utils.MakePixelPerfect(newPosition);
            transform.position = pixelPerfectMoveAmount;

            yield return new WaitForEndOfFrame();
        }
        transform.position = endpos;
        isMoving = false;
    }

    
    public bool EndPosReached() {
        if (transform.position == endpos) {
            return true;
        }
        return false;
    }
}

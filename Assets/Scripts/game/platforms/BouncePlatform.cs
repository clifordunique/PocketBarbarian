using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePlatform : MonoBehaviour {

    public LayerMask reactLayer;

    public float seconds;
    public Vector2 distance;

    private Vector3 endpos;
    private Vector3 startPos;

    private bool isMoving = false;
    private float t;

    public void Start() {
        startPos = transform.position;
        endpos = new Vector3(transform.position.x + distance.x, transform.position.y + distance.y, transform.position.z);        
    }

    public void Update() {
        if (isMoving) {
            SmoothMove();
        }
    }

    private void SmoothMove() {
        
        if (t <= 1.0) {

            t += Time.deltaTime / seconds;
            float v = t;

            Vector3 newPosition = Vector3.Lerp(startPos, endpos, v);

            Vector2 pixelPerfectMoveAmount = Utils.MakePixelPerfect(newPosition);
            transform.parent.position = pixelPerfectMoveAmount;
            
        }
        
    }

    public void OnTriggerEnter2D(Collider2D collider) {
        // react to trigger
        if (reactLayer == (reactLayer | (1 << collider.gameObject.layer))) {
            Debug.Log("Trigger enter");
            isMoving = true;
            t = 0.0f;
        }
    }


    public void OnTriggerExit2D(Collider2D collider) {
        // react to trigger
        if (reactLayer == (reactLayer | (1 << collider.gameObject.layer))) {
            Debug.Log("Trigger exit");
           // isMoving = false;
        }
    }
    
}

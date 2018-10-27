using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiDiedTextController : MonoBehaviour {

    public float seconds;

    private SpriteRenderer spriteRenderer;
    private float t = 0;
    private bool isMoving = true;
    private Vector3 startPos;
    private Vector3 endPos;

    // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // InitPosition
        transform.position = GetPosition (0.5f, 1.0f, spriteRenderer.bounds.size.y);
        startPos = transform.position;
        endPos = GetPosition(0.5F, 0.5F, 0);
    }

    private Vector3 GetPosition(float x, float y, float offsetY) {
        Vector3 result;
        float z = transform.position.z;
        result = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0));
        result = new Vector3(result.x, result.y + offsetY, z);
        return result;
    }

    // Update is called once per frame
    void Update () {
        
        if (isMoving) {
            SmoothMove();
        }
    }


    private void SmoothMove() {

        if (t <= 1.0) {

            t += Time.deltaTime / seconds;
            float newTime = t;
            if (newTime > 1) {
                newTime = 1;
            }
            float easeFactor = EasingFunction.EaseOutBounce(0.0F, 1.0F, newTime);

            Vector2 newPosition = Vector2.Lerp(startPos, endPos, easeFactor);
            Vector3 pixelPerfectMoveAmount = Utils.MakePixelPerfect(newPosition);
            transform.Translate(pixelPerfectMoveAmount - transform.position);
        } else {
            isMoving = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiLevelCompleteTextController: MonoBehaviour {
    
    public float secondsBounce;
    
    private float t = 0;
    private Vector3 startPos;
    private Vector3 endPos;

    // Use this for initialization
    void Start () {
        startPos = transform.position;
        endPos = new Vector3(startPos.x, startPos.y+2, startPos.z);
    }

    // Update is called once per frame
    void Update () {
        
        SmoothMoveUpDown();
    }


    private void SmoothMoveUpDown() {

        if (t <= 1.0) {

            t += Time.deltaTime / secondsBounce;
            float newTime = t;
            if (newTime > 1) {
                newTime = 1;
            }
            float easeFactor = EasingFunction.EaseInOutQuad(0.0F, 1.0F, newTime);

            Vector2 newPosition = Vector2.Lerp(startPos, endPos, easeFactor);
            Vector3 pixelPerfectMoveAmount = Utils.MakePixelPerfect(newPosition);
            transform.Translate(pixelPerfectMoveAmount - transform.position);
        } else {
            t = 0;
            Vector3 dummy = startPos;
            startPos = endPos;
            endPos = dummy;
        }
    }
}

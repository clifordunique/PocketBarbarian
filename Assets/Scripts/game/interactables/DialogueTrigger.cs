using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Dialogue))]
public class DialogueTrigger : MonoBehaviour {

    
    public LayerMask layerMaskToAcitvate;   
    
    public float cameraMoveSeconds;

    private Dialogue dialogue;
    private bool alreadyDone = false;
    private bool inDialogue = false;
    private Vector3 startPos;
    private Vector3 endPos;

    private void Start() {
        dialogue = GetComponent<Dialogue>();
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (layerMaskToAcitvate == (layerMaskToAcitvate | (1 << collision.gameObject.layer))) {
            if (!alreadyDone) {
                InputController.GetInstance().moveInputEnabled = false;
                startPos = Camera.main.transform.position;
                endPos = transform.position;
                CameraFollow.GetInstance().enabled = false;
                StartCoroutine(SmoothMove());
                dialogue.StartDialogue();
                inDialogue = true;
            }
        }
    }

    public void Update() {
        if (!alreadyDone && inDialogue) {
            // check if Dialogue finished
            if (!dialogue.inDialogue) {
                // Dialogue finished, revert changes
                Vector3 dummy = startPos;
                startPos = endPos;
                endPos = dummy;
                StartCoroutine(SmoothMove());
                inDialogue = false;
                alreadyDone = true;
            }
        }
    }

    IEnumerator SmoothMove() {
        float t = 0.0f;
        while (t <= 1.0) {

            t += Time.deltaTime / cameraMoveSeconds;
            float v = t;
            v = EasingFunction.EaseInOutQuad(0.0f, 1.0f, t);
            Vector3 newPosition = Vector3.Lerp(startPos, endPos, v);
            newPosition.z = -10;

            Vector2 pixelPerfectMoveAmount = Utils.MakePixelPerfect(newPosition);
            Vector3 newPos = new Vector3(pixelPerfectMoveAmount.x, pixelPerfectMoveAmount.y, -10);
            Camera.main.transform.position = newPos;

            yield return new WaitForEndOfFrame();
        }
        if (!inDialogue) {
            CameraFollow.GetInstance().enabled = true;
            InputController.GetInstance().moveInputEnabled = true;
        }
    }

}

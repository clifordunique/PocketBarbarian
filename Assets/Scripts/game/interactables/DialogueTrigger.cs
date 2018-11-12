using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Dialogue))]
public class DialogueTrigger : MonoBehaviour {

    
    public LayerMask layerMaskToAcitvate;
    public bool detachCamera = true;
    public float cameraMoveSeconds;
    public float moveCameraDelay;

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
                inDialogue = true;
                InputController.GetInstance().moveInputEnabled = false;
                if (detachCamera) {
                    StartCoroutine(MoveCameraDelay());
                }
                dialogue.StartDialogue();                
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
                if (detachCamera) {
                    StartCoroutine(MoveCamera());
                } else {
                    InputController.GetInstance().moveInputEnabled = true;
                }
                inDialogue = false;
                alreadyDone = true;
            }
        }
    }

    IEnumerator MoveCameraDelay() {
        yield return new WaitForSeconds(moveCameraDelay);
        startPos = Camera.main.transform.position;
        endPos = transform.position;
        CameraFollow.GetInstance().enabled = false;
        StartCoroutine(MoveCamera());
    }

    IEnumerator MoveCamera() {
        float t = 0.0f;
        while (t <= 1.0) {
            t += Time.deltaTime / cameraMoveSeconds;
            float v = t;
            v = EasingFunction.EaseInOutQuad(0.0f, 1.0f, t);
            Vector3 newPosition = Vector3.Lerp(startPos, endPos, v);

            Vector2 pixelPerfectMoveAmount = Utils.MakePixelPerfect(newPosition);
            Vector3 newPos = new Vector3(pixelPerfectMoveAmount.x, pixelPerfectMoveAmount.y, Camera.main.transform.position.z);
            Camera.main.transform.position = newPos;

            yield return new WaitForEndOfFrame();
        }
        if (!inDialogue) {
            CameraFollow.GetInstance().enabled = true;
            InputController.GetInstance().moveInputEnabled = true;
        }
    }

}

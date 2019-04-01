using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTextManager : MonoBehaviour
{

    public CharacterDisplayer characterDisplayer;

    public TextAsset textAsset;

    public float moveSpeed;
    public float moveDistanceY;

    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        characterDisplayer.DisplayString(textAsset.text, transform, 0, 0);
    }


    private void Update() {
        if (!isMoving && Input.GetKeyDown(KeyCode.DownArrow)) {
            StartCoroutine(MoveFromTo(transform, transform.position, new Vector3(transform.position.x, transform.position.y + moveDistanceY, transform.position.z), moveSpeed));
        }
        if (!isMoving && Input.GetKeyDown(KeyCode.UpArrow)) {
            StartCoroutine(MoveFromTo(transform, transform.position, new Vector3(transform.position.x, transform.position.y - moveDistanceY, transform.position.z), moveSpeed));
        }
    }



    IEnumerator MoveFromTo(Transform objectToMove, Vector3 a, Vector3 b, float speed) {
        isMoving = true;
        float step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f) {
            t += step; // Goes from 0 to 1, incrementing by step each time
            objectToMove.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        objectToMove.position = b;
        isMoving = false;
    }
}

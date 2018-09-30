using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectParent : MonoBehaviour {

    void Update() {
        if (Time.frameCount % 10 == 0) {
            if (transform.childCount == 0) {
                Destroy(gameObject);
            }
        }
    }
}

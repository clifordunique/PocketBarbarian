using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnter : MonoBehaviour
{
    public LayerMask interactableLayer;

    void Start() {
        
    }

    public void OnTriggerEnter2D(Collider2D collider) {
        // react to trigger
        if (interactableLayer == (interactableLayer | (1 << collider.gameObject.layer))) {

        }
    }
}

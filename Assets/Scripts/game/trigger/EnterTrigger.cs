using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTrigger : MonoBehaviour
{
    public LayerMask interactableLayer;

    private ITriggerReactor triggerReactor;

    void Start() {
        triggerReactor = GetComponent<ITriggerReactor>();
        if (triggerReactor == null) {
            triggerReactor = transform.parent.GetComponent<ITriggerReactor>();
        }
    }

    public void OnTriggerEnter2D(Collider2D collider) {
        // react to trigger
        if (interactableLayer == (interactableLayer | (1 << collider.gameObject.layer))) {
            if (triggerReactor != null) {
                triggerReactor.TriggerActivated();
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collider) {
        // react to trigger
        if (interactableLayer == (interactableLayer | (1 << collider.gameObject.layer))) {
            if (triggerReactor != null) {
                triggerReactor.TriggerDeactivated();
            }
        }
    }
}

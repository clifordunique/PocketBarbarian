using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTrigger : MonoBehaviour   {

    public LayerMask interactableLayer;

    private TriggerManager triggerManager;

    void Start() {
        triggerManager = GetComponent<TriggerManager>();
        if (triggerManager == null) {
            Debug.Log("TriggeManager does not exist on " + gameObject.name);
        }
    }

    public void OnTriggerEnter2D(Collider2D collider) {
        // react to trigger
        if (interactableLayer == (interactableLayer | (1 << collider.gameObject.layer))) {
            if (triggerManager != null) {
                triggerManager.ActivateReactors();
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collider) {
        // react to trigger
        if (interactableLayer == (interactableLayer | (1 << collider.gameObject.layer))) {
            if (triggerManager != null) {
                triggerManager.DeactivateReactors();
            }
        }
    }
}

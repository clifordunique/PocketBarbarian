using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstactInteractable : MonoBehaviour {


    public GameObject actionArrow;
    public LayerMask reactLayer;
    public bool permanentDisabled = false;
    
    [HideInInspector]
    public bool actionFinished = false;

    public abstract void Activate();    


    public void OnTriggerEnter2D(Collider2D collider) {
        // react to trigger
        if (reactLayer == (reactLayer | (1 << collider.gameObject.layer))) {
            if (!permanentDisabled) {
                actionArrow.SetActive(true);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collider) {
        // react to trigger
        if (reactLayer == (reactLayer | (1 << collider.gameObject.layer))) {
            if (!permanentDisabled) {
                actionArrow.SetActive(false);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstactInteractable: MonoBehaviour {


    public GameObject actionArrow;
    public LayerMask reactLayer;
    public bool permanentDisabled = false;

    [HideInInspector]
    public bool actionFinished = true;

    public virtual void Start() {

    }

    public abstract void Activate();


    public virtual void OnTriggerEnter2D(Collider2D collider) {
        // react to trigger
        if (reactLayer == (reactLayer | (1 << collider.gameObject.layer))) {
            if (!permanentDisabled) {
                actionArrow.SetActive(true);
            }
        }
    }

    public virtual void OnTriggerExit2D(Collider2D collider) {
        // react to trigger
        if (reactLayer == (reactLayer | (1 << collider.gameObject.layer))) {
            if (!permanentDisabled) {
                actionArrow.SetActive(false);
            }
        }
    }
}
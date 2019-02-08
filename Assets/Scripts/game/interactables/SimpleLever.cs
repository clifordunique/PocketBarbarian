using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLever : AbstactInteractable {
    

    public bool activated = false;
    public Sprite leverSpriteOn;

    private Sprite leverSpriteOff;

    private SpriteRenderer sr;
    private TriggerManager triggerManager;

    public override void Start() {
        base.Start();
        sr = GetComponent<SpriteRenderer>();
        leverSpriteOff = sr.sprite;

        triggerManager = GetComponent<TriggerManager>();
        if (triggerManager == null) {
            Debug.Log("TriggeManager does not exist on " + gameObject.name);
        }
    }

    public override void Activate() {
        if (activated) {
            // deactivate
            bool startMoving = triggerManager.DeactivateReactors();
            if (startMoving) {
                sr.sprite = leverSpriteOff;
                activated = false;
            }
        } else {
            // activate
            bool startMoving = triggerManager.ActivateReactors();
            if (startMoving) {
                sr.sprite = leverSpriteOn;
                activated = true;
            }
        }
        actionFinished = true;
    }
    
}

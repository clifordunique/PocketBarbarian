using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLever : AbstactInteractable {

    public AbstractActivatable activatableObject;

    public bool activated = false;
    public Sprite leverSpriteOn;

    private Sprite leverSpriteOff;

    private SpriteRenderer sr;

    private void Start() {
        sr = GetComponent<SpriteRenderer>();
        leverSpriteOff = sr.sprite;
    }

    public override void Activate() {
        if (activated) {
            // deactivate
            bool startMoving = activatableObject.Activate();
            if (startMoving) {
                sr.sprite = leverSpriteOff;
                activated = false;
            }
        } else {
            // activate
            bool startMoving = activatableObject.DeActivate();
            if (startMoving) {
                sr.sprite = leverSpriteOn;
                activated = true;
            }
        }
        actionFinished = true;
    }
    
}

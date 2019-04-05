using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimInteractable : AbstactInteractable {


    public bool oneTimeUse = false;
    public bool activated = false;

    private Animator anim;
    private TriggerManager triggerManager;

    public override void Start() {
        base.Start();
        anim = GetComponent<Animator>();

        SetStopAnimation();

        triggerManager = GetComponent<TriggerManager>();
        if (triggerManager == null) {
            Debug.Log("TriggeManager does not exist on " + gameObject.name);
        }
    }

    public override void Activate() {
        actionFinished = false;
        if (activated && !permanentDisabled) {
            // deactivate
            bool startMoving = triggerManager.DeactivateReactors();
            if (startMoving) {
                anim.SetBool("STOP", false);
                anim.SetBool("INTERACTABLE", false);
                anim.SetBool("START", true);
                activated = false;
            }
        } else {
            // activate
            if (!permanentDisabled) {
                bool startMoving = triggerManager.ActivateReactors();
                if (startMoving) {
                    anim.SetBool("STOP", false);
                    anim.SetBool("INTERACTABLE", false);
                    anim.SetBool("START", true);
                    activated = true;
                }
                if (oneTimeUse) {
                    permanentDisabled = true;
                    actionArrow.SetActive(false);
                }
            }
        }
        
    }
    
    public void StopAction() {
        actionFinished = true;
        anim.SetBool("START", false);
        SetStopAnimation();        
    }
    

    private void SetStopAnimation() {
        Debug.Log("actionFinished:" + actionFinished);
        Debug.Log("permanentDisabled:" + permanentDisabled);
        if (actionFinished && !permanentDisabled) {
            anim.SetBool("STOP", false);
            anim.SetBool("INTERACTABLE", true);
        } else {
            anim.SetBool("STOP", true);
            anim.SetBool("INTERACTABLE", false);
        }
    }
}

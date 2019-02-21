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

        triggerManager = GetComponent<TriggerManager>();
        if (triggerManager == null) {
            Debug.Log("TriggeManager does not exist on " + gameObject.name);
        }
    }

    public override void Activate() {
        if (activated && !permanentDisabled) {
            // deactivate
            bool startMoving = triggerManager.DeactivateReactors();
            if (startMoving) {
                anim.SetBool("STOP", false);
                anim.SetBool("START", true);
                activated = false;
            }
        } else {
            // activate
            if (!permanentDisabled) {
                bool startMoving = triggerManager.ActivateReactors();
                if (startMoving) {
                    anim.SetBool("STOP", false);
                    anim.SetBool("START", true);
                    activated = true;
                }
                if (oneTimeUse) {
                    permanentDisabled = true;
                    actionArrow.SetActive(false);
                }
            }
        }
        actionFinished = true;
    }
    
    public void StopAction() {
        anim.SetBool("STOP", true);
        anim.SetBool("START", false);
    }
    
}

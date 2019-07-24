using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverInteractable: AbstactInteractable {


    public bool oneTimeUse = false;
    public bool activated = false;

    private Animator anim;
    private TriggerManager triggerManager;

    private string ON = "ON";
    private string ON_ACTIVE = "ON_ACTIVE";
    private string OFF = "OFF";
    private string OFF_ACTIVE = "OFF_ACTIVE";

    public override void Start() {
        base.Start();
        anim = GetComponent<Animator>();

        triggerManager = GetComponent<TriggerManager>();
        if (triggerManager == null) {
            Debug.Log("TriggeManager does not exist on " + gameObject.name);
        }
    }

    public override void Activate() {
        if (!permanentDisabled) {
            if (activated) {
                // deactivate
                bool changeState = true;
                if (triggerManager.HasReactors()) {
                    changeState = triggerManager.DeactivateReactors();
                }
                if (changeState) {
                    ChangeState(OFF_ACTIVE);
                    activated = false;
                }
            } else {
                // activate
                bool startMoving = true;
                if (triggerManager.HasReactors()) {
                    startMoving = triggerManager.ActivateReactors();
                }
                if (oneTimeUse) {
                    permanentDisabled = true;
                    actionArrow.SetActive(false);
                }
                if (startMoving) {
                    
                    if (!permanentDisabled) {
                        ChangeState(ON_ACTIVE);
                    } else {
                        ChangeState(ON);
                    }
                    activated = true;
                }

            }
        }
        actionFinished = true;
    }

    public void Disable() {
        permanentDisabled = true;
        actionArrow.SetActive(false);
        if (activated) {
            ChangeState(ON);
        } else {
            ChangeState(OFF);
        }
    }

    private void ChangeState(string state) {
        anim.SetBool(ON, false);
        anim.SetBool(ON_ACTIVE, false);
        anim.SetBool(OFF, false);
        anim.SetBool(OFF_ACTIVE, false);
        anim.SetBool(state, true);
    }
    
}

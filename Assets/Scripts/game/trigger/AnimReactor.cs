using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimReactor : CameraMoveReactor
{

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    
    public override bool TriggerActivated() {
        Debug.Log("Start Trigger");
        base.TriggerActivated();
        Debug.Log("Start Anim");
        anim.SetBool("START", true);
        return true;
    }

    public override bool TriggerDeactivated() {
        anim.SetBool("START", false);
        return true;
    }
}

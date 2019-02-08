using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyController : MonoBehaviour, ITriggerReactor
{
    public string evadeString;
    public string backToIdleString;

    private Animator animator;

    public void Start() {
        animator = GetComponent<Animator>();
    }

    public bool TriggerActivated() {
        animator.SetTrigger(evadeString);
        return true;
    }

    public bool TriggerDeactivated() {
        animator.SetTrigger(backToIdleString);
        return true;
    }
    
}

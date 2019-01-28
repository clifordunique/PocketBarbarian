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

    public void TriggerActivated() {
        animator.SetTrigger(evadeString);
    }

    public void TriggerDeactivated() {
        animator.SetTrigger(backToIdleString);
    }
    
}

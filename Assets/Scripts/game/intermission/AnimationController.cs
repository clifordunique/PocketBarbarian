using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

	public bool animationComplete = false;

    private Animator animator;
    private string lastParameter = null;

    void Start() {
        animator = GetComponent<Animator>();
    }

    public void AnimationCompleteEvent() {
        animationComplete = true;
    }

    public void TriggerClip(string parameter) {

        if (lastParameter != null) {
            animator.SetBool(lastParameter, false);
        }
        animator.SetBool(parameter, true);
        lastParameter = parameter;
        animationComplete = false;
    }
}

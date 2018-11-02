using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Animator))]
public class DelayAnimEffect : MonoBehaviour {

    public string animationParam = "START";
    public float delayTime = 0.1F;

    private Animator animator;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        StartCoroutine(StartAnimationWithDelay());
	}
	

    IEnumerator StartAnimationWithDelay() {
        yield return new WaitForSeconds(delayTime);
        animator.SetBool(animationParam, true);
    }
}

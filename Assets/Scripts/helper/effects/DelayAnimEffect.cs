using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Animator))]
public class DelayAnimEffect : MonoBehaviour {

    public string animationParam = "START";
    public float delayTime = 0.1F;
    public bool random = false;

    private float delayTimeExact = 0;

    private Animator animator;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        if (random) {
            delayTimeExact = Random.Range(0, delayTime);
        } else {
            delayTimeExact = delayTime;
        }
        StartCoroutine(StartAnimationWithDelay());
	}
	

    IEnumerator StartAnimationWithDelay() {
        yield return new WaitForSeconds(delayTimeExact);
        animator.SetBool(animationParam, true);
    }
}

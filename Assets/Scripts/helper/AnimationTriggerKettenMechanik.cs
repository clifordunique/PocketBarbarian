using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggerKettenMechanik : MonoBehaviour {

    private Animator[] animatorList;

    public void Start() {
        animatorList = GetComponentsInChildren<Animator>();

        Debug.Log("Animators found:" + animatorList.Length);
    }

    public void StartTrigger() {
        foreach(Animator animator in animatorList) {
            animator.SetBool("STOP", false);
            animator.SetBool("RUNNING", true);
        }
    }

    public void StopTrigger() {
        foreach (Animator animator in animatorList) {
            animator.SetBool("STOP", true);
            animator.SetBool("RUNNING", false);
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour {

    private Animator[] animatorList;

    public void Start() {
        if (GetComponent<Animator>()) {
            animatorList = new Animator[1];
            animatorList[0] = GetComponent<Animator>();
        } else {
            animatorList = GetComponentsInChildren<Animator>();
        }
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

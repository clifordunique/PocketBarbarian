using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTimer : MonoBehaviour {

    public string param1Name;
    public string param2Name;
    public float timeTransition;
    public float startTimeOffset;

    private Animator anim;
    private bool state1 = true;
    private float timer = 0;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        timer = Time.time + timeTransition + startTimeOffset;
	}
	
	// Update is called once per frame
	void Update () {
		if (timer < Time.time) {
            if (state1) {
                anim.SetBool(param1Name, false);
                anim.SetBool(param2Name, true);
                state1 = false;
            } else {
                anim.SetBool(param1Name, true);
                anim.SetBool(param2Name, false);
                state1 = true;
            }
            timer = Time.time + timeTransition;
        }
	}
}

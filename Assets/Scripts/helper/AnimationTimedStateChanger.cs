using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTimedStateChanger : MonoBehaviour
{
    public float timeDelay;
    public float timePassive;
    public float timeActive;

    private Animator anim;
    private bool active = false;
    private float time;
    private float buffer;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        time = Time.timeSinceLevelLoad + timeDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (active) {
            if ((time + timeActive) < Time.timeSinceLevelLoad) {
                buffer = Time.timeSinceLevelLoad - (time + timeActive);
                time = Time.timeSinceLevelLoad - buffer;
                anim.SetBool("STATE_ACTIVE", false);
                anim.SetBool("STATE_PASSIVE", true);
                active = false;
            }
        } else {
            if ((time + timePassive) < Time.timeSinceLevelLoad) {
                buffer = Time.timeSinceLevelLoad - (time + timePassive);
                time = Time.timeSinceLevelLoad - buffer;
                anim.SetBool("STATE_ACTIVE", true);
                anim.SetBool("STATE_PASSIVE", false);
                active = true;
            }
        }
    }
}

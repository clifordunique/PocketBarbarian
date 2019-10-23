using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeTrap : MonoBehaviour
{
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) {
            anim.SetBool("START", true);
            anim.SetBool("STOP", false);
        }

        if (Input.GetKeyDown(KeyCode.G)) {
            anim.SetBool("STOP", true);
            anim.SetBool("START", false);
        }
    }
}

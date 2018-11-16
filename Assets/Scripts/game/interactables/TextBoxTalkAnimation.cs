using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxTalkAnimation: TextBox {

    public float timeForCharacter = 0.1F;
    public string talkParameterAnimation = "TALK";
    public string returnParameterAnimation = "IDLE";


    private Animator animator;

    
    public override void Init() {
        base.Init();
        animator = transform.parent.GetComponent<Animator>();
        if (!animator) {
            Debug.Log("Animator not found!");
        }
    }

    public override void ShowTextBox(string text) {
        base.ShowTextBox(text);
        float timeTalking = text.Length * timeForCharacter;

        StartCoroutine(Talk(timeTalking));
    }

    private IEnumerator Talk(float seconds) {
        animator.SetBool(returnParameterAnimation, false);
        animator.SetBool(talkParameterAnimation, true);
        yield return new WaitForSeconds(seconds);

        animator.SetBool(returnParameterAnimation, true);
        animator.SetBool(talkParameterAnimation, false);
    }

    void OnDisable() {
        animator.SetBool(returnParameterAnimation, true);
        animator.SetBool(talkParameterAnimation, false);
    }
}

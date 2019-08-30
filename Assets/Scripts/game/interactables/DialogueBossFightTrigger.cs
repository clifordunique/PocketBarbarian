using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Dialogue))]
public class DialogueBossFightTrigger: DialogueTrigger {

    public float delayTextEffect = 1F;
    public float durationTextEffect = 1F;
    private bool inFight = false;

    public override void Update() {
        if (!alreadyDone && inDialogue) {
            // check if Dialogue finished

            if (!dialogue.inDialogue && !inFight) {
                inFight = true;
                StartCoroutine(StartEffect());
            }
        }
    }


    IEnumerator StartEffect() {
        yield return new WaitForSeconds(delayTextEffect);
        GuiController.GetInstance().ShowRandomFightText(durationTextEffect);
        StartCoroutine(EnableControlls());
    }

    IEnumerator EnableControlls() {
        yield return new WaitForSeconds(durationTextEffect);
        InputController.GetInstance().moveInputEnabled = true;
        TriggerManager tm = GetComponent<TriggerManager>();
        tm.ActivateReactors();
    }
}

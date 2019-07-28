using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue: MonoBehaviour {

    public TextAsset textAsset;
    public bool dialogueInteractive = true;
    [Header("Trigger")]
    public bool useTrigger = false;
    [ConditionalHide("useTrigger", true)]
    public bool activateOnStart = false;
    [ConditionalHide("useTrigger", true)]
    public bool activateOnEnd = false;
    [ConditionalHide("useTrigger", true)]
    public float waitTriggerTime = 0F;

    public float timeOffsetToBeginDialogue = 0.5F;
    public float pauseBetweenDialogues = 0.2F;

    public TextBox textBoxDialogue1;
    public TextBox textBoxDialogue2;
    public TextBox textBoxDialogue3;

    private string[] dialogueLines;
    private int currentPositionInDialogue = 0;
    private TextBox lastTextBox;

    private TriggerManager triggerManager;

    [HideInInspector]
    public bool inDialogue = false;

    private bool lastDialogueReady = true;

    // Use this for initialization
    void Start() {
        dialogueLines = textAsset.text.Split('\n');
        if (useTrigger) {
            triggerManager = GetComponent<TriggerManager>();
        }
    }

    public void StartDialogue() {
        inDialogue = true;
        Invoke("NextDialogueStep", timeOffsetToBeginDialogue);
        if (useTrigger && activateOnStart && triggerManager && triggerManager.HasReactors()) {
            StartCoroutine(ActivateTriggerManager());
        }
    }

    // Update is called once per frame
    void Update() {
        if (dialogueInteractive && inDialogue && lastDialogueReady) {
            if (InputController.GetInstance().AnyKeyDown() && currentPositionInDialogue <= dialogueLines.Length) {
                NextDialogueStep();
            }
        }
    }

    public void DisableLastTextBox() {
        if (lastTextBox) {
            lastTextBox.gameObject.SetActive(false);
        }
    }

    public void NextDialogueStep() {
        lastDialogueReady = false;
        if (currentPositionInDialogue == dialogueLines.Length) {
            StartCoroutine(EndDialogue(pauseBetweenDialogues));
        } else {

            DisableLastTextBox();

            string dialogueLine = dialogueLines[currentPositionInDialogue];
            string[] dialogueLineSplit = dialogueLine.Split('|');

            if (dialogueLineSplit[0].Contains("1")) {
                lastTextBox = textBoxDialogue1;
            } else {
                if (dialogueLineSplit[0].Contains("2")) {
                    lastTextBox = textBoxDialogue2;
                } else {
                    lastTextBox = textBoxDialogue3;
                }
            }
            StartCoroutine(ShowText(lastTextBox, dialogueLineSplit[1], pauseBetweenDialogues));

            currentPositionInDialogue++;
        }
    }

    public IEnumerator ShowText(TextBox textBoxDialogue, string text, float time) {
        yield return new WaitForSeconds(time);
        textBoxDialogue.gameObject.SetActive(true);
        textBoxDialogue.ShowTextBox(text);
        lastDialogueReady = true;
    }

    public IEnumerator EndDialogue(float time) {
        yield return new WaitForSeconds(time);
        if (lastTextBox) {
            lastTextBox.gameObject.SetActive(false);
        }
        inDialogue = false;
        lastDialogueReady = true;

        if (useTrigger && activateOnEnd && triggerManager && triggerManager.HasReactors()) {
            StartCoroutine(ActivateTriggerManager());
        }
    }

    public IEnumerator ActivateTriggerManager() {
        yield return new WaitForSeconds(waitTriggerTime);
        triggerManager.ActivateReactors();
    }
}

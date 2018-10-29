using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour {

    public TextAsset textAsset;
    public float pauseBetweenDialogues = 0.2F;

    public TextBox textBoxDialogue1;
    public TextBox textBoxDialogue2;

    private string[] dialogueLines;
    private int currentPositionInDialogue = 0;
    private TextBox lastTextBox;

    // Use this for initialization
    void Start () {
        InputController.GetInstance().moveInputEnabled = false;
        dialogueLines = textAsset.text.Split('\n');
    }
	
	// Update is called once per frame
	void Update () {
		if (InputController.GetInstance().AnyKeyDown() && currentPositionInDialogue <= dialogueLines.Length) {

            if (currentPositionInDialogue == dialogueLines.Length) {
                StartCoroutine(EndDialogue(pauseBetweenDialogues));
            } else {

                if (lastTextBox) {
                    lastTextBox.gameObject.SetActive(false);
                }

                string dialogueLine = dialogueLines[currentPositionInDialogue];
                string[] dialogueLineSplit = dialogueLine.Split('|');

                if (dialogueLineSplit[0].Contains("1")) {
                    lastTextBox = textBoxDialogue1;
                } else {
                    lastTextBox = textBoxDialogue2;
                }
                StartCoroutine(ShowText(lastTextBox, dialogueLineSplit[1], pauseBetweenDialogues));

                currentPositionInDialogue++;
            }
        }
	}

    public IEnumerator ShowText(TextBox textBoxDialogue, string text, float time) {
        textBoxDialogue.gameObject.SetActive(true);
        textBoxDialogue.ShowTextBox(text);
        yield return new WaitForSeconds(time);
        Debug.Log(text);
    }

    public IEnumerator EndDialogue(float time) {
        yield return new WaitForSeconds(time);
        if (lastTextBox) {
            lastTextBox.gameObject.SetActive(false);
        }
        InputController.GetInstance().moveInputEnabled = true;
    }
}

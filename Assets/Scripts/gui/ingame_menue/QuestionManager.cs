using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    public enum ANSWER_TYPE { YES, NO, NAN};
    public ANSWER_TYPE focusAnswer = ANSWER_TYPE.NO;
    public ANSWER_TYPE selectedAnswer = ANSWER_TYPE.NAN;
    public Sprite spriteNo;
    private Sprite spriteYes;

    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        spriteYes = sr.sprite;
    }

    public void Init() {
        sr.sprite = spriteYes;
        // default
        SetFocus(ANSWER_TYPE.NO);
        selectedAnswer = ANSWER_TYPE.NAN;
    }

    public void SetFocus(ANSWER_TYPE answerTypeNew) {
        focusAnswer = answerTypeNew;
        if (focusAnswer == ANSWER_TYPE.YES) {
            sr.sprite = spriteYes;
        }
        if (focusAnswer == ANSWER_TYPE.NO) {
            sr.sprite = spriteNo;
        }
    }

    private void Toggle() {
        if (focusAnswer == ANSWER_TYPE.YES) {
            SetFocus(ANSWER_TYPE.NO);
        } else {
            if (focusAnswer == ANSWER_TYPE.NO) {
                SetFocus(ANSWER_TYPE.YES);
            }
        }
    }

    public void SetSelectedAnswer(ANSWER_TYPE answertype) {
        selectedAnswer = answertype;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) {
            Toggle();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            SetSelectedAnswer(ANSWER_TYPE.NO);
        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            SetSelectedAnswer(focusAnswer);
        }
    }
}

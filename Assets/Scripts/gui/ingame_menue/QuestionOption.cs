using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionOption : MonoBehaviour
{
    public QuestionManager.ANSWER_TYPE answerType;

    private QuestionManager manager;


    // Start is called before the first frame update
    void Start()
    {
        manager = transform.parent.GetComponent<QuestionManager>();
        if (!manager) {
            Debug.LogError("No QuestionManager found in parent!");
        }        
    }

    void OnMouseDown() {
        manager.SetSelectedAnswer(answerType);
    }

    void OnMouseEnter() {
       manager.SetFocus(answerType);
    }   
}

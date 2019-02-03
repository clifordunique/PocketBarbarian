using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

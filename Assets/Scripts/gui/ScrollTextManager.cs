using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTextManager : MonoBehaviour
{

    public CharacterDisplayer characterDisplayer;

    public TextAsset textAsset;

    // Start is called before the first frame update
    void Start()
    {
        characterDisplayer.DisplayString(textAsset.text, transform, 0, 0);
    }

}

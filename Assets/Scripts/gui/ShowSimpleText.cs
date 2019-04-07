using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSimpleText : MonoBehaviour
{
    public string text;
    private GuiCharacterController charController;
    

    // Start is called before the first frame update
    void Start()
    {
        charController = transform.GetComponentInChildren<GuiCharacterController>();
        charController.Show(text);
    }
    
}

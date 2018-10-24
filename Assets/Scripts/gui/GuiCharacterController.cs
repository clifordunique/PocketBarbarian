using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiCharacterController : MonoBehaviour {

    public int offsetPixelsX;
    public int offsetPixelsY;
    public CharacterDisplayer charDisplayer;
    public GuiSimpleCharacterBackgroundController backgroundController;

    private bool shown = false;
    // Use this for initialization
    void Start () {


    }
	
	// Update is called once per frame
	public void Show (string text) {
        if (!shown) {
            Vector2 size = charDisplayer.DisplayString(text, transform, offsetPixelsX, offsetPixelsY);
            Vector3 newpos = new Vector3(transform.position.x - (size.x / 2), transform.position.y + (size.y / 2), transform.position.z);
            transform.position = Utils.MakePixelPerfect(newpos);
            backgroundController.ResizeBackground(size);
            shown = true;
        }
    }
}

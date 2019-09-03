using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiCharacterController : MonoBehaviour {

    public int offsetPixelsX;
    public int offsetPixelsY;
    public int additionalLineSpacePixel = 0;
    public CharacterDisplayer charDisplayer;
    public GuiSimpleCharacterBackgroundController backgroundController;

    private Vector3 originalPosition;
    private bool isInit = false;

    private void Init() {
        originalPosition = transform.localPosition;
        isInit = true;
    }

    
    public void Show (string text, bool xCentered = false, bool yCentered = true) {
        if (!isInit) {
            Init();
        }

        DestroyCharacters();

        transform.localPosition = originalPosition;
        Vector2 size = charDisplayer.DisplayString(text, transform, offsetPixelsX, offsetPixelsY, additionalLineSpacePixel, xCentered);

        float newY = transform.position.y;
        float newX = transform.position.x - (size.x / 2);
        if (yCentered) {
            newY = transform.position.y + (size.y / 2);
        } 
        if (xCentered) {
            newX = transform.position.x;
        }
        Vector3 newpos = new Vector3(newX, newY, transform.position.z);
        
        transform.position = Utils.MakePixelPerfect(newpos);
        if (backgroundController != null) {
            backgroundController.ResizeBackground(size);
        }
    }

    public void DestroyCharacters() {
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }
    }
}

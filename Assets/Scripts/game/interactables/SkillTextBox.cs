using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTextBox : MonoBehaviour {

    public GuiCharacterController charController;
    public GameObject background;
    public string text;
    private float offsetY;
    

    private void Start() {        
        // initial deaktivieren
        if (background) {
            background.SetActive(false);
            float height = Utils.GetHeightFromSpriteGO(background);
            offsetY = height / 2;
            Debug.Log("OffsetY:" + offsetY);
            offsetY = offsetY + background.transform.localPosition.y;
            Debug.Log("OffsetY:" + offsetY);
        }
        if (charController) {
            charController.gameObject.SetActive(false);
        }
    }

    public void ShowTextBox() {
        // zentriert auf bildschirm setzen
        Utils.SetGUIPosition(gameObject, 0.5F, 0.5F, 0, offsetY);

        background.SetActive(true);
        charController.gameObject.SetActive(true);
        charController.Show(text, true, false);
    }
    
    public void HideTextBox() {
        background.SetActive(false);
        charController.gameObject.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTextTrigger : MonoBehaviour {

    public GuiCharacterController charController;
    public LayerMask layerMaskToAcitvate;
    public string text;	

    public void OnTriggerEnter2D(Collider2D collision) {
        if (layerMaskToAcitvate == (layerMaskToAcitvate | (1 << collision.gameObject.layer))) {
            charController.gameObject.SetActive(true);
            charController.Show(text, true, false);
        }
    }

    public void OnTriggerExit2D(Collider2D collision) {
        if (layerMaskToAcitvate == (layerMaskToAcitvate | (1 << collision.gameObject.layer))) {
            charController.gameObject.SetActive(false);
        }
    }


}

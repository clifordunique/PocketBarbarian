using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxTrigger : MonoBehaviour {

    public TextBox textBox;
    public LayerMask layerMaskToAcitvate;
    public string text;	

    public void OnTriggerEnter2D(Collider2D collision) {
        if (layerMaskToAcitvate == (layerMaskToAcitvate | (1 << collision.gameObject.layer))) {
            textBox.gameObject.SetActive(true);
            textBox.ShowTextBox(text);
        }
    }

    public void OnTriggerExit2D(Collider2D collision) {
        if (layerMaskToAcitvate == (layerMaskToAcitvate | (1 << collision.gameObject.layer))) {
            textBox.gameObject.SetActive(false);
        }
    }


}

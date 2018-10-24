using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxTrigger : MonoBehaviour {

    public TextBox textBox;
    public LayerMask layerMaskToAcitvate;
    public string text;
    public int offsetPixelsY;

    private float offsetY;

    // Use this for initialization
    void Start () {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        offsetY = boxCollider.bounds.extents.y;
        if (offsetPixelsY > 0) {
            offsetY += offsetPixelsY / Constants.PPU;
        }
    }
	

    public void OnTriggerEnter2D(Collider2D collision) {
        if (layerMaskToAcitvate == (layerMaskToAcitvate | (1 << collision.gameObject.layer))) {
            textBox.gameObject.SetActive(true);
            textBox.ShowTextBox(offsetY, text);
        }
    }

    public void OnTriggerExit2D(Collider2D collision) {
        if (layerMaskToAcitvate == (layerMaskToAcitvate | (1 << collision.gameObject.layer))) {
            textBox.gameObject.SetActive(false);
        }
    }


}

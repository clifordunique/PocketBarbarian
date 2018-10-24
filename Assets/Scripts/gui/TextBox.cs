using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBox : MonoBehaviour {

    private GuiCharacterController charController;
    private GuiSimpleCharacterBackgroundController bgrController;

    // Use this for initialization
    void Start () {

    }
	
	public void ShowTextBox(float offsetY, string text) {
        
        charController = transform.GetComponentInChildren<GuiCharacterController>();
        bgrController = transform.GetComponentInChildren<GuiSimpleCharacterBackgroundController>();
        charController.Show(text);

        Vector2 sizeBox = bgrController.GetSize();
        Vector3 favoritePosition = new Vector3(transform.parent.position.x, transform.parent.position.y + offsetY + (sizeBox.y/2F), transform.parent.position.z);
        
        float minX = favoritePosition.x - (sizeBox.x / 2F);
        float maxX = favoritePosition.x + (sizeBox.x / 2F);

        Vector2 cameraLeft = Camera.main.ViewportToWorldPoint(new Vector3(0F, 0F, 0));
        Vector2 cameraRight = Camera.main.ViewportToWorldPoint(new Vector3(1F, 0F, 0));

        float distanceLeft = (minX - cameraLeft.x);
        float distanceRight = (cameraRight.x - maxX);

        if (distanceLeft < 0) {
            // nach rechts verschieben
            favoritePosition = new Vector3(favoritePosition.x + Mathf.Abs(distanceLeft), favoritePosition.y, favoritePosition.z);
            bgrController.MoveArrow(distanceLeft);
        }
        if (distanceRight < 0) {
            // nach rechts verschieben
            favoritePosition = new Vector3(favoritePosition.x - Mathf.Abs(distanceRight), favoritePosition.y, favoritePosition.z);
            bgrController.MoveArrow(Mathf.Abs(distanceRight));
        }
        transform.position = favoritePosition;
    }


}
